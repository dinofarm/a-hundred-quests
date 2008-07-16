#region File Description
//-----------------------------------------------------------------------------
// Map.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using OHQData;
using OHQData.Helpers;
using System.IO;
using System.Text.RegularExpressions;
#endregion

namespace OHQData
{



    /// <summary>
    /// The overmap where non-combat gameplay takes place
    /// </summary>
    public class Map
    {

        #region Members

        private ResizableGrid<Tile> tileGrid;
        //private Tile[,] tiles;

        /// <summary>
        /// Each bit of the border int is a flag for the direction
        /// For example, a border of 11111111 means a border in all 8 directions
        /// 
        /// 00000001  east
        /// 00000010  north
        /// 00000100  west
        /// 00001000  south
        ///
        /// 00010000  north-east
        /// 00100000  north-west
        /// 01000000  south-west
        /// 10000000  south-east
        /// </summary>
        //private int[,]  borders;

        protected Terrain grass, hill, mountain, wasteland, water;

        /// <summary>
        /// The dimensions of the map, in tiles.
        /// </summary>
        //private int size;

        private MapSize mapSize;

        /// <summary>
        /// The dimensions of the map, in tiles.
        /// </summary>
        public int Size
        {
            get { return Math.Max(tileGrid.Width, tileGrid.Height); }
            //set { size = value; }
        }

        /// <summary>
        /// The number of tiles on this map
        /// </summary>
        public int NumTiles
        {
            //get { return size * size; }
            get { return tileGrid.Width * tileGrid.Height; }
        }

        public int WalkableTiles
        {
            //get { return size * size; }
            get { return grass.Count + wasteland.Count + hill.Count; }
        }

        #endregion


        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public Map(MapSize mapSize)
        {
            this.mapSize = mapSize;

            // The tileGrid is created when the map is generated.
        }

        #endregion

        public Tile getTile(Vector2 position)
        {
            return getTile((int)position.X, (int)position.Y);
        }

        public Tile getTile(int x, int y)
        {
            if (!tileGrid.containsCoordinate(x, y))
            {
                Tile t = new Tile(water);
                t.Position = new Point(x, y);
                return t;
            }
            return getExpandingTile(x, y);
        }

        private Tile getExpandingTile(int x, int y)
        {
            tileGrid.containCoordinate(x, y);
            Tile t = tileGrid.get(x, y);
            if (t == null)
            {
                t = new Tile(water);
                t.Position = new Point(x, y);
                tileGrid.set(x, y, t);
            }
            return t;
        }


        #region Load and unload content

        public void LoadContent(ContentManager content)
        {
            loadTerrains(content);

            // terrains must be loaded before random map generation,
            // so here it is in LoadContent instead of the constructor
            generateRandom();
        }

        private void loadTerrains(ContentManager content)
        {
            grass = content.Load<Terrain>(Path.Combine(@"Map\Terrains", "Grass"));
            hill = content.Load<Terrain>(Path.Combine(@"Map\Terrains", "Hill"));
            mountain = content.Load<Terrain>(Path.Combine(@"Map\Terrains", "Mountain"));
            wasteland = content.Load<Terrain>(Path.Combine(@"Map\Terrains", "Wasteland"));
            water = content.Load<Terrain>(Path.Combine(@"Map\Terrains", "Water"));

            // TODO: This stuff should be stored in the Content files. 
            mountain.IsWalkable = false;
            water.IsWalkable = false;
        }

        private void resetMap()
        {
            tileGrid = new ResizableGrid<Tile>();
            grass.Count = 0;
            hill.Count = 0;
            mountain.Count = 0;
            wasteland.Count = 0;
            water.Count = 0;
        }
        #endregion

        #region Random generation

        /// <summary>
        /// The idea of this terrain generatio is first to place 
        /// </summary>
        private void generateRandom()
        {

            do
            {
                resetMap();
                createMainIsland();
            }
            while (!isSingleLandMass());

            plotTerrain(wasteland, 15, 20, 1, 3);
            placeMountainsAndHills();
            plotTerrain(hill, 15, 20, 1, 1);

            int waterPercent = ((water.Count * 100) / WalkableTiles);
            waterPercent = Math.Max(20, waterPercent);
            plotTerrain(water, waterPercent + 5, waterPercent + 10, 1, 2);

            calculateBorders();
        }


        /// <summary>
        /// This function turns the square map entirely made out of grass into an island,
        /// which is roughly square, but not completely.
        /// </summary>
        private void createMainIsland()
        {
            int size = (int)mapSize;
            int minGrassTerrains = (int)(mapSize);
            minGrassTerrains = minGrassTerrains * minGrassTerrains;


            while (grass.Count < minGrassTerrains)
            {
                // Creates a jagged polygon (see RadialCirclePolygon), rougly centered around (0,0).
                // Everything inside this polygon is turned into grass.
                int range = size / 2;
                double xc = RandomAid.NextFloatInRange(-range, range);
                double yc = RandomAid.NextFloatInRange(-range, range);
                double minimumRadius = size * 0.1;
                double maximumRadius = size * 0.3;
                Polygon polygon = new RadialCirclePolygon(xc, yc,
                                                          minimumRadius, maximumRadius);

                // Check each tile to see if it's outside the polygon.
                // First we check all the tiles in the outer layer.                
                // Then the ones in the layer inside that, and so on.
                // So if we we reach maxGrassTerrains too fast, then
                // the water will be evenly distributed around the map.
                for (int y = (int)polygon.TopBound(); y <= polygon.BottomBound(); y++)
                {
                    for (int x = (int)polygon.LeftBound(); x <= polygon.RightBound(); x++)
                    {
                        createGrass(polygon, x, y);   // north
                    }
                }
            }
        }

        private void createGrass(Polygon polygon, int column, int row)
        {
            if (polygon.contains(column, row))
            {
                if (getExpandingTile(column, row).Terrain == water)
                {
                    setTileTerrain(column, row, grass);
                }
            }
        }


        /// <summary>
        /// This more complext method places water onto the specified coordinates <b>IF</b>
        /// the block is not the only block to keep two parts of the land mass together.
        /// Stated differently: No new islands may be created by placing water 
        /// on this tile.
        /// If it detects that an island would be created, then it simply does nothing.
        /// This can be detected by checking the value of Map.water.Count.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void placeInwalkableTerrain(int column, int row, Terrain newterrain)
        {
            Tile tile = getTile(column, row);

            Terrain oldTerrain = tile.Terrain;
            if (oldTerrain == newterrain) { return; }

            // We mark the terrain as water.
            tile.Terrain = newterrain;

            // If this tile obviously didn't split anything, then we can safely return..
            if (!checkForLocalLandSplit(column, row))
            {
                return;
            }

            // This tile might have split something and we must do a thorough check
            // to see that the landmass is still intact.
            if (!isSingleLandMass())
            {
                tile.Terrain = oldTerrain;
                return;
            }

        }

        /// <summary>
        /// This method checks if there are multiple land masses.
        /// </summary>
        /// <returns></returns>
        private bool isSingleLandMass()
        {
            Dictionary<Tile, Object> paintedTiles = null;
            bool paintingDone = false;

            for (int y = tileGrid.MinY; y <= tileGrid.MaxY; y++)
            {
                for (int x = tileGrid.MinX; x <= tileGrid.MaxX; x++)
                {
                    Tile t = getTile(x, y);
                    if (isWalkableTerrain(t.Terrain))
                    {
                        if (!paintingDone)
                        {
                            paintedTiles = paintTilesAdjacentTo(t);
                            paintingDone = true;
                        }
                        else
                        {
                            if (!paintedTiles.ContainsKey(t))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private Dictionary<Tile, object> paintTilesAdjacentTo(Tile startTile)
        {
            Dictionary<Tile, object> visitedTiles = new Dictionary<Tile, object>();
            Stack<Tile> tileStack = new Stack<Tile>();
            tileStack.Push(startTile);
            visitedTiles.Add(startTile, null);
            while (tileStack.Count > 0)
            {
                Tile tile = tileStack.Pop();
                addIfPossible(visitedTiles, tileStack, tile.Position.X - 1, tile.Position.Y);
                addIfPossible(visitedTiles, tileStack, tile.Position.X + 1, tile.Position.Y);
                addIfPossible(visitedTiles, tileStack, tile.Position.X, tile.Position.Y - 1);
                addIfPossible(visitedTiles, tileStack, tile.Position.X, tile.Position.Y + 1);
            }

            return visitedTiles;
        }

        private void addIfPossible(Dictionary<Tile, object> visitedTiles, Stack<Tile> tileStack, int x, int y)
        {
            Tile t = getTile(x, y);
            if (!isWalkableTerrain(t))
            {
                return;
            }

            if (visitedTiles.ContainsKey(t))
            {
                return;
            }

            tileStack.Push(t);
            visitedTiles.Add(t, null);
        }


        private bool isWalkableTerrain(int column, int row)
        {
            return isWalkableTerrain(getTile(column, row).Terrain);
        }
        private bool isWalkableTerrain(Tile t)
        {
            return isWalkableTerrain(t.Terrain);
        }
        private bool isWalkableTerrain(Terrain t)
        {
            return t != water && t != mountain;
        }

        private bool checkForLocalLandSplit(int column, int row)
        {
            string adjacentTiles = "";
            adjacentTiles += getAdjecentStringLetter(column - 1, row - 1);
            adjacentTiles += getAdjecentStringLetter(column, row - 1);
            adjacentTiles += getAdjecentStringLetter(column + 1, row - 1);

            adjacentTiles += getAdjecentStringLetter(column + 1, row);

            adjacentTiles += getAdjecentStringLetter(column + 1, row + 1);
            adjacentTiles += getAdjecentStringLetter(column, row + 1);
            adjacentTiles += getAdjecentStringLetter(column - 1, row + 1);

            adjacentTiles += getAdjecentStringLetter(column - 1, row);

            // This string will consist of 1's and 0's where 1=walkable and 0=unwalkable.
            // They are in clockwise order around the specified tile.
            // If there is only one coherent row of 1's, then there is no split.
            Match m = Regex.Match(adjacentTiles, "(^0*1*0*$)|(^1+0+1+$)");
            if (m.Success)
            {
                return false;
            }
            return true;
        }
        private char getAdjecentStringLetter(int column, int row)
        {
            if (isWalkableTerrain(column, row))
            {
                return '1';
            }
            else
            {
                return '0';
            }
        }


        private bool tilesConnected(Point start, Point goal)
        {

            if (start.Equals(goal))
            {
                return true;
            }
            Queue<Point> queue = new Queue<Point>();
            bool[,] connected = new bool[tileGrid.Height, tileGrid.Width];

            connected[start.Y - tileGrid.MinY, start.X - tileGrid.MinX] = true;

            queue.Enqueue(start);
            while (queue.Count != 0)
            {

                Point p = queue.Dequeue();
                if (tilesConnectedHelper(queue, connected, new Point(p.X, p.Y + 1), goal) || // east
                    tilesConnectedHelper(queue, connected, new Point(p.X + 1, p.Y), goal) || // north
                    tilesConnectedHelper(queue, connected, new Point(p.X, p.Y - 1), goal) || // west
                    tilesConnectedHelper(queue, connected, new Point(p.X - 1, p.Y), goal))  // south
                {
                    return true;
                }
            }

            return false;
        }

        private bool tilesConnectedHelper(Queue<Point> queue,
                                          bool[,] connected,
                                          Point p, Point goal)
        {
            if (p.Y == goal.Y && p.X == goal.X) { return true; }
            int xMod = tileGrid.MinX;
            int yMod = tileGrid.MinY;
            if (isLandTile(p.X, p.Y))
            {
                if (connected[p.Y - yMod, p.X - xMod] == false)
                {
                    connected[p.Y - yMod, p.X - xMod] = true;
                    queue.Enqueue(p);
                }
            }

            return false;
        }

        private void plotTerrain(Terrain terrain,
                                 int minPercent, int maxPercent,
                                 int polygonLow, int polygonHigh)
        {
            int mininum = (WalkableTiles * minPercent) / 100;
            int maximum = (WalkableTiles * maxPercent) / 100;
            while (terrain.Count < mininum)
            {
                int centerX = getRandomXInsideIsland();
                int centerY = getRandomYInsideIsland();

                Polygon polygon = new RadialCirclePolygon(centerX, centerY,
                                                          polygonLow, polygonHigh);

                terraformIfInside(grass, terrain, maximum, polygon);
            }
        }

        private void terraformIfInside(Terrain oldTerrain, Terrain newTerrain,
                                       int newTerrainMax,
                                       Polygon polygon)
        {
            // assert(0 < maxToTerrain); // TODO: convert assertion to C#


            for (int row = tileGrid.MinY; row <= tileGrid.MaxY; row++)
            {
                for (int column = tileGrid.MinX; column <= tileGrid.MaxX; column++)
                {
                    terraformIfInside(row, column,
                                      oldTerrain, newTerrain,
                                      polygon);

                    if (newTerrain.Count > newTerrainMax) { return; }
                }
            }
        }

        private void terraformIfInside(int row, int column,
                                       Terrain oldTerrain, Terrain newTerrain,
                                       Polygon polygon)
        {
            if (polygon.contains(row, column))
            {
                if (getExpandingTile(column, row).Terrain == oldTerrain)
                {
                    setTileTerrain(column, row, newTerrain);
                }
            }
        }

        private int getRandomXInsideIsland()
        {
            return RandomAid.NextIntInRange(tileGrid.MinX, tileGrid.MaxX);
        }
        private int getRandomYInsideIsland()
        {
            return RandomAid.NextIntInRange(tileGrid.MinY, tileGrid.MaxY);
        }

        private void placeMountainsAndHills()
        {
            int minMountains = (grass.Count * 8) / 100;

            while (mountain.Count < minMountains)
            {

                int mountainRangeLength = (RandomAid.NextIntInRange(1, 10) + RandomAid.NextIntInRange(1, 10)) / 2;
                int row = 0;
                int column = 0;
                do
                {
                    column = getRandomXInsideIsland();
                    row = getRandomYInsideIsland();
                } while (getTile(column, row).Terrain != grass);

                int xMod = 0;
                int yMod = 0;
                for (int i = 0; i < mountainRangeLength; i++)
                {

                    if (getTile(column, row).Terrain == grass)
                    {
                        setTileTerrain(column, row, mountain);
                    }
                    else if (getTile(column, row).Terrain != mountain)
                    {
                        break;
                    }

                    do
                    {
                        xMod = RandomAid.NextIntInRange(-1, 1);
                        yMod = RandomAid.NextIntInRange(-1, 1);
                    } while (xMod == 0 && yMod == 0);

                    row = row + xMod;
                    column = column + yMod;

                    if (getTile(column, row).Terrain == water)
                    {
                        break;
                    }
                }
            }

            createHillsAroundMountains();


        }

        private void createHillsAroundMountains()
        {
            for (int row = tileGrid.MinY; row <= tileGrid.MaxY; row++)
            {
                for (int column = tileGrid.MinX; column <= tileGrid.MaxX; column++)
                {

                    if (getTile(column, row).Terrain == mountain)
                    {
                        createHillIfLegal(column, row - 1);
                        createHillIfLegal(column, row + 1);
                        createHillIfLegal(column - 1, row);
                        createHillIfLegal(column + 1, row);
                    }
                }
            }

        }

        private void createHillIfLegal(int column, int row)
        {
            Tile tile = getTile(column, row);
            if (tile.Terrain == grass || tile.Terrain == wasteland)
            {
                // There's a chance of a hill appearing next to a mountain.
                if (RandomAid.NextFloat() < 0.6)
                {
                    setTileTerrain(column, row, hill);
                }
            }
        }

        private void calculateBorders()
        {
            // This gives the map an edge that we can paint border's on.
            getExpandingTile(tileGrid.MinX - 1, tileGrid.MinY - 1);
            getExpandingTile(tileGrid.MaxX + 1, tileGrid.MaxY + 1);

            for (int row = tileGrid.MinY; row <= tileGrid.MaxY; row++)
            {
                for (int column = tileGrid.MinX; column <= tileGrid.MaxX; column++)
                {

                    Tile tile = getTile(column, row);
                    Terrain terrain = tile.Terrain;// terrainAt(column, row);

                    if (terrain == wasteland || terrain == water)
                    {
                        if (terrainAt(column - 1, row) != terrain) { tile.addBorder(Tile.BorderEnum.West); }
                        if (terrainAt(column, row - 1) != terrain) { tile.addBorder(Tile.BorderEnum.North); }
                        if (terrainAt(column + 1, row) != terrain) { tile.addBorder(Tile.BorderEnum.East); }
                        if (terrainAt(column, row + 1) != terrain) { tile.addBorder(Tile.BorderEnum.South); }

                        // Diagonal border checks must be done AFTER the horizontal and vertical border checks.
                        if (terrainAt(column - 1, row - 1) != terrain) { tile.addBorder(Tile.BorderEnum.NorthWest); }
                        if (terrainAt(column + 1, row - 1) != terrain) { tile.addBorder(Tile.BorderEnum.NorthEast); }
                        if (terrainAt(column + 1, row + 1) != terrain) { tile.addBorder(Tile.BorderEnum.SouthEast); }
                        if (terrainAt(column - 1, row + 1) != terrain) { tile.addBorder(Tile.BorderEnum.SouthWest); } 
                    }
                }
            }

        }

        /// <summary>
        /// Get the terrain of the tile at the specified coordinates.
        /// Defaults to water if the coordinates are not valid
        /// </summary>
        /// <param name="column">column coordinate of tile</param>
        /// <param name="row">row coordinate of tile</param>
        /// <returns></returns>
        public Terrain terrainAt(int column, int row)
        {
            return getTile(column, row).Terrain;
        }

        #endregion


        #region Tile information


        public void setTileTerrain(int column, int row, Terrain terrain)
        {

            Tile tile = getExpandingTile(column, row);

            if (tile.Terrain == terrain) { return; }

            if (this.isWalkableTerrain(terrain))
            {
                // remove the old terrain
                tile.Terrain = terrain;
            }
            else
            {
                placeInwalkableTerrain(column, row, terrain);
            }
        }

        private bool isLandTile(int column, int row)
        {
            //if (!isLegalTileCoordinate(column, row)) { return false; }
            if (getTile(column, row).Terrain == water) { return false; }

            return true;
        }


        #endregion


        #region Draw

        /// <summary>
        /// Draw the map
        /// </summary>
        public void draw(SpriteBatch spriteBatch, int centerX, int centerY)
        {
            drawTiles(spriteBatch, centerX, centerY);
        }

        /// <summary>
        /// Draw the map's tiles
        /// </summary>
        public void drawTiles(SpriteBatch spriteBatch, int centerX, int centerY)
        {

            int y = 0;
            for (int row = centerY - 10; row <= centerY + 10; row++)
            {
                int x = 0;
                for (int column = centerX - 16; column <= centerX + 16; column++)
                {
                    drawTile(spriteBatch, row, column, x, y);
                    x += Tile.SizePx;
                }
                y += Tile.SizePx;
            }
        }

        /// <summary>
        /// Draw the specified tile
        /// </summary>
        /// <param name="row">row coordinate of tile</param>
        /// <param name="column">column coordinate of tile</param>
        public void drawTile(SpriteBatch spriteBatch,
                             int row, int column,
                             int x, int y)
        {
            Tile tile = getTile(column, row);//getTile(row, column);
            Point position = new Point(x, y);
            tile.draw(spriteBatch, position);
        }

        #endregion
    }

    /// <summary>
    /// The map size
    /// </summary>
    public enum MapSize
    {
        Small = 12,
        Medium = 16,
        Large = 20,
        Huge = 30,
    }
}