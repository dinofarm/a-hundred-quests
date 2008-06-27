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
using OHQData.Characters;
using OHQData;
using OHQData.Helpers;
using System.IO;
#endregion

namespace OHQData
{
    /// <summary>
    /// The overmap where non-combat gameplay takes place
    /// </summary>
    public class Map
    {
        #region Members

        // indexed as [column, row]
        private Tile[,] tiles;

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
        private int[,]  borders;

        private Terrain grass, hill, mountain, wasteland, water;

        /// <summary>
        /// The dimensions of the map, in tiles.
        /// </summary>
        private int size;
        /// <summary>
        /// The dimensions of the map, in tiles.
        /// </summary>
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// The number of tiles on this map
        /// </summary>
        public int NumTiles
        {
            get { return size * size; }
        }

        #endregion


        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public Map(MapSize mapSize)
        {
            // map size
            this.size = (int)mapSize;

            // tiles
            tiles = new Tile[size, size];

            // borders
            borders = new int[size + 2, size + 2];
        }

        #endregion


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
            grass     = content.Load<Terrain>(Path.Combine(@"Map\Terrains", "Grass"));
            hill      = content.Load<Terrain>(Path.Combine(@"Map\Terrains", "Hill"));
            mountain  = content.Load<Terrain>(Path.Combine(@"Map\Terrains", "Mountain"));
            wasteland = content.Load<Terrain>(Path.Combine(@"Map\Terrains", "Wasteland"));
            water     = content.Load<Terrain>(Path.Combine(@"Map\Terrains", "Water"));
        }

        #endregion


        #region Random generation

        private void generateRandom()
        {
            placeGrass();
            terraformMainIsland();
            plotTerrain(wasteland, 15, 20, 2, 4);
            placeMountainsAndHills();
            plotTerrain(water, 20, 25, 1, 2);
            circumscribeWithWater();
            calculateBorders();
        }

        private void placeGrass()
        {
            // start with all grass tiles
            for (int column = 0; column < size; column++)
            {
                for (int row = 0; row < size; row++)
                {
                    tiles[column, row] = new Tile(grass);
                }
            }
            grass.Count = NumTiles;
        }

        private void terraformMainIsland()
        {
            water.Count = 0;

            int maxGrassTerrains = (grass.Count * 90) / 100;

            while (grass.Count > maxGrassTerrains)
            {
                // Creates a polygon, roughly square in shape. Everything outside
                // this polygon is then turned into water.
                double boardCenter = (size / 2) - 0.5;
                Polygon polygon = new RadialCirclePolygon(boardCenter, boardCenter,
                                                          size / 2 - 3, size / 2 + 6);

                // Check each tile to see if it's outside the polygon,
                // but in a different order.
                // First we check all the tiles in the outer layer.
                // Then the ones in the layer inside that, and so on.
                for (int radius = 0; radius < size / 2; radius++)
                {
                    for (int i = 0; i < size; i++)
                    {
                        int invertedRadius = size - 1 - radius;
                        makeCoastTile(polygon, radius, i);          // north
                        makeCoastTile(polygon, invertedRadius, i);  // south
                        makeCoastTile(polygon, i, radius);          // east
                        makeCoastTile(polygon, i, invertedRadius);  // west
                    }

                    if (grass.Count <= maxGrassTerrains) { return; }
                }
            }
        }

        /// <summary>
        /// Set the tile to a coastline (water) terrain if it is not contained in the specified polygon
        /// </summary>
        /// <param name="polygon">polygonal bounding area</param>
        /// <param name="column">column coordinate of tile</param>
        /// <param name="row">row coordinate of tile</param>
        private void makeCoastTile(Polygon polygon, int column, int row)
        {
            if (!polygon.contains(row, column))
            {
                if (tiles[column,row].Terrain == grass)
                {
                    setTileTerrain(column, row, water);
                }
            }
        }

	    private void placeWater(int column, int row)
        {
            Tile tile = tiles[column, row];

		    Terrain oldTerrain = tile.Terrain;
		    if (oldTerrain == water) { return; }

		    tile.Terrain = water;

            int[,] locals = new int[3,3];
		    int max = 0;
		    int maxX = 0;
		    int maxY = 0;
		    int landTileCount = 0;
		    if (isLandTile(column - 1, row)) {
			    locals[0,1] = 1;
			    max = 1;
			    maxX = row;
			    maxY = column - 1;
			    landTileCount++;
		    }
		    if (isLandTile(column + 1, row)) {
			    locals[2,1] = 2;
			    max = 2;
			    maxX = row;
			    maxY = column + 1;
			    landTileCount++;
		    }
		    if (isLandTile(column, row - 1)) {
			    locals[1,0] = 3;
			    max = 3;
			    maxX = row - 1;
			    maxY = column;
			    landTileCount++;
		    }
		    if (isLandTile(column, row + 1)) {
			    locals[1,2] = 4;
			    max = 4;
			    maxX = row + 1;
			    maxY = column;
			    landTileCount++;
		    }

		    oldTerrain.Count--;
		    water.Count++;
    		
		    if (landTileCount <= 1 ) { return; }
    		
		    placeWaterJoinLands(locals, column, row, -1, -1);
		    placeWaterJoinLands(locals, column, row, -1, +1);
		    placeWaterJoinLands(locals, column, row, +1, -1);
		    placeWaterJoinLands(locals, column, row, +1, +1);
		    Point maxPoint = new Point(maxX,maxY);
		    for (int i = 0; i < 3; i++) {
			    for (int j = 0; j < 3; j++) {				
    				
				    if (0 < locals[i,j] && locals[i,j] < max) {
					    Point p = new Point(column - 1 + i, row - 1 + j);
					    if ( !tilesConnected(maxPoint, p) ) {
						    tile.Terrain = oldTerrain;
						    oldTerrain.Count++;
						    water.Count--;
						    return;
					    } else {
						    int low = locals[i,j];
						    connectLocals(low, max, locals);							
					    }
				    }
			    }
		    }
	    }

        private void placeWaterJoinLands(int[,] locals,
                                         int column, int row,
                                         int yMod, int xMod)
        {
            if (isLandTile(column + yMod, row + xMod))
            {
                int v1 = locals[1, 1 + xMod];
                int v2 = locals[1 + yMod, 1];
                int highest = Math.Max(v1, v2);
                int lowest = Math.Min(v1, v2);
                locals[1 + yMod, 1 + xMod] = highest;

                if (v1 == 0 || v2 == 0) { return; }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (locals[i, j] == lowest) { locals[i, j] = highest; }
                    }
                }
            }
        }

        private void connectLocals(int low, int max, int[,] locals)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (locals[i,j] == low) { locals[i,j] = max; }
                }
            }
        }

	    private bool tilesConnected(Point start, Point goal) { 
    	
            Queue<Point> queue = new Queue<Point>();
		    bool[,] connected = new bool[size, size];

		    connected[start.Y,start.X] = true;
            
            queue.Enqueue(start);
		    while (queue.Count != 0) {

                Point p = queue.Dequeue();
			    if (tilesConnectedHelper(queue, connected, new Point(p.Y, p.X + 1), goal) || // east
                    tilesConnectedHelper(queue, connected, new Point(p.Y + 1, p.X), goal) || // north
                    tilesConnectedHelper(queue, connected, new Point(p.Y, p.X - 1), goal) || // west
                    tilesConnectedHelper(queue, connected, new Point(p.Y - 1, p.X), goal) )  // south
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

            if (isLandTile(p.Y, p.X))
            {
                if (connected[p.Y,p.X] == false)
                {
                    connected[p.Y,p.X] = true;
                    queue.Enqueue(p);
                }
            }

            return false;
        }

        private void plotTerrain(Terrain terrain,
                                 int minPercent, int maxPercent,
                                 int polygonLow, int polygonHigh)
        {
            int mininum = (NumTiles * minPercent) / 100;
            int maximum = (NumTiles * maxPercent) / 100;
            while (terrain.Count < mininum)
            {
                int centerX = RandomAid.NextIntInRange(1, size - 2);
                int centerY = RandomAid.NextIntInRange(1, size - 2);

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

            for (int column = 0; column < size; column++)
            {
                for (int row = 0; row < size; row++)
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
                if (tiles[column, row].Terrain == oldTerrain)
                {
                    setTileTerrain(column, row, newTerrain);
                }
            }
        }

        private void placeMountainsAndHills()
        {
            int minMountains = (NumTiles * 7) / 100;

            while (mountain.Count < minMountains)
            {

                int mountainRangeLength = RandomAid.NextIntExclusive(4) + 3;
                int row = 0;
                int column = 0;
                do
                {
                    row = RandomAid.NextIntExclusive(size);
                    column = RandomAid.NextIntExclusive(size);
                } while (tiles[column, row].Terrain != grass);

                int xMod = 0;
                int yMod = 0;
                for (int i = 0; i < mountainRangeLength; i++)
                {

                    if (tiles[column, row].Terrain == grass)
                    {
                        setTileTerrain(column, row, mountain);
                        mountain.Count++;
                        grass.Count--;
                    }
                    else if (tiles[column, row].Terrain != mountain)
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

                    if (column < 0 || row < 0 ||
                        size <= row || size <= column) { break; }
                }
            }

            createHillsAroundMountains();

            plotTerrain(hill, 20, 25, 1, 1);
        }

        private void createHillsAroundMountains()
        {
            for (int column = 0; column < size; column++)
            {
                for (int row = 0; row < size; row++)
                {
                    if (tiles[column, row].Terrain == mountain)
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
            if (!isLegalTileCoordinate(column, row)) { return; }

            if (tiles[column, row].Terrain == grass || tiles[column, row].Terrain == wasteland)
            {
                setTileTerrain(column, row, hill);
            }
        }

        private void circumscribeWithWater()
        {
            // pad the tile grid with an extra line at each edge
		    Tile[,] newTiles = new Tile[size + 2, size + 2];
		    for (int i = 1; i < size + 1; i++) {
			    for (int j = 1; j < size + 1; j++) {
				    newTiles[i, j] = tiles[i - 1, j - 1];
			    }
		    }
		    tiles = newTiles;

            // fill the padding with water tiles
		    for (int i = 0; i < size + 2; i++) {
			    tiles[i, 0] = new Tile(water);        // northern edge
			    tiles[0, i] = new Tile(water);        // western edge
			    tiles[i, size + 1] = new Tile(water); // southern edge
			    tiles[size + 1, i] = new Tile(water); // eastern edge
		    }
        }

        private void calculateBorders()
        {
            for (int column = -1; column < size + 1; column++)
            {
                for (int row = -1; row < size + 1; row++)
                {
                    Terrain terrain = terrainAt(column, row);

                    if (terrain == wasteland || terrain == water)
                    {
                        int border = 0;

                        if (terrainAt(column, row - 1) != terrain) { border |= 1; } // 00000001  east
                        if (terrainAt(column - 1, row) != terrain) { border |= 2; } // 00000010  north
                        if (terrainAt(column, row + 1) != terrain) { border |= 4; } // 00000100  west
                        if (terrainAt(column + 1, row) != terrain) { border |= 8; } // 00001000  south

                        if ((border & 3)  == 0 && terrainAt(column - 1, row - 1) != terrain) { border |= 1 << 4; } // 00010000  north-east
                        if ((border & 6)  == 0 && terrainAt(column - 1, row + 1) != terrain) { border |= 2 << 4; } // 00100000  north-west
                        if ((border & 12) == 0 && terrainAt(column + 1, row + 1) != terrain) { border |= 4 << 4; } // 01000000  south-west
                        if ((border & 9)  == 0 && terrainAt(column + 1, row - 1) != terrain) { border |= 8 << 4; } // 10000000  south-east

                        borders[column + 1, row + 1] = border;
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
            row++;
            column++;

            if (!isValid(column, row)) { return water; }

            return tiles[column, row].Terrain;
        }

        private bool isValid(int column, int row)
        {
            if (row > size + 1 || column > size + 1) { return false; }
            if (row < 0 || column < 0) { return false; }

            return true;
        }

        /// <summary>
        /// Get the border for the tile at the specified coordinates
        /// </summary>
        /// <param name="row">row coordinate for the tile</param>
        /// <param name="column">column coordinate for the tile</param>
        /// <returns></returns>
        private int border(int row, int column)
        {
            row++;
            column++;

            if (!isLegalBorderCoordinate(row, column)) { return 0; }

            return borders[column, row];
        }

        #endregion


        #region Tile information


        public void setTileTerrain(int column, int row, Terrain terrain)
        {
            // make sure tile coordinates are legal
            if (!isLegalTileCoordinate(column, row)) { return; }

            Tile tile = tiles[column, row];

            if (tile.Terrain == terrain) { return; }

            if (terrain != water)
            {
                // remove the old terrain
                tile.Terrain.Count -= 1;

                // add the new terrain      // TODO: make add/remove terrain a tile method so it updates Count automatically
                tile.Terrain = terrain;
                tile.Terrain.Count += 1;
            }
            else
            {
                placeWater(column, row);
            }
        }

        private bool isLegalTileCoordinate(int column, int row)
        {
            return ((0 <= column) && (column < size) && (0 <= row) && (row < size));
        }

        private bool isLegalBorderCoordinate(int row, int col)
        {
            if (row > size + 1 || col > size + 1) { return false; }
            if (row < 0 || col < 0) { return false; }

            return true;
        }

        private bool isLandTile(int column, int row)
        {
            if (!isLegalTileCoordinate(column, row)) { return false; }
            if (tiles[column, row].Terrain == water) { return false; }

            return true;
        }


        #endregion


        #region Draw

        /// <summary>
        /// Draw the map
        /// </summary>
        public void draw(SpriteBatch spriteBatch)
        {
            drawTiles(spriteBatch);
        }

        /// <summary>
        /// Draw the map's tiles
        /// </summary>
        public void drawTiles(SpriteBatch spriteBatch)
        {
            int x = 0;
            int y;

            for (int column = 1; column <= size; column++)
            {
                y = 0;
                for (int row = 1; row <= size; row++)
                {
                    drawTile(spriteBatch, row, column, x, y, Tile.SizePx, Tile.SizePx);
                    //drawBorder();
                    y += Tile.SizePx;
                }
                x += Tile.SizePx;
            }
        }

        public void drawBorder(SpriteBatch spriteBatch,
                               int row, int column)
        {
            /*
            Tile tile = tile[column, row];

            // if water or wasteland
            if (tile.Terrain == water || tile.Terrain == wasteland)
            {
                int xIncrement = row * Tile.SizePx;
                int yIncrement = column * Tile.SizePx;
                int border = border[row, column];

                if (tile.Terrain == wasteland)
                {
                    //spriteBatch.Draw(terrain.Sprite, position, Color.White);

                    gMap.drawImage(tileType.getBorder(TileLoader.GRASS_BORDER,
                            border, animateSlowTerrain), xIncrement, yIncrement, null);
                    gMap.drawImage(tileType.getCornerBorder(
                            TileLoader.GRASS_BORDER, border, animateSlowTerrain),
                            xIncrement, yIncrement, null);
                }
                else
                {
                    gMap.drawImage(tileType.getBorder(TileLoader.WATER_BORDER,
                            border, animateSlowTerrain), xIncrement, yIncrement, null);
                    gMap.drawImage(tileType.getCornerBorder(
                            TileLoader.WATER_BORDER, border, animateSlowTerrain),
                            xIncrement, yIncrement, null);
                }
            }
             */
        }

        /// <summary>
        /// Draw the specified tile
        /// </summary>
        /// <param name="row">row coordinate of tile</param>
        /// <param name="column">column coordinate of tile</param>
        public void drawTile(SpriteBatch spriteBatch,
                             int row, int column,
                             int x, int y, int width, int height)
        {
            // TODO: throw exception if row/column is beyond array bounds

            Tile tile = tiles[row-1, column-1];
            Rectangle destinationRectangle = new Rectangle(x, y, width, height);
            tile.draw(spriteBatch, destinationRectangle);
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