using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace OHQData
{


    public class Tile
    {
        #region Size

        private const int STANDARD_TILE_SIZE_PX = 40;
        /// <summary>
        /// The tile size in pixels
        /// </summary>
        private static int sizePx = STANDARD_TILE_SIZE_PX;
        /// <summary>
        /// The tile size in pixels
        /// </summary>
        public static int SizePx
        {
            get { return sizePx; }
        }

        #endregion

        #region Terrain

        private Terrain terrain;

        public Terrain Terrain
        {
            get { return terrain; }
            set
            {
                if (terrain != null)
                {
                    terrain.Count--;
                }
                terrain = value;
                if (terrain != null)
                {
                    terrain.Count++;
                }

            }
        }

        public Boolean IsWalkable
        {
            get { return terrain.IsWalkable; }
        }
        #endregion

        #region Position

        private Point position;

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion



        #region Border

        public enum BorderEnum
        {
            West = 1,
            North = 2,
            East = 4,
            South = 8,

            NorthWest = (1 << 4),
            NorthEast = (2 << 4),
            SouthEast = (4 << 4),
            SouthWest = (8 << 4)
        }

        private static int NORTH_AND_EAST = (int)(BorderEnum.North | BorderEnum.East);
        private static int NORTH_AND_WEST = (int)(BorderEnum.North | BorderEnum.West);
        private static int SOUTH_AND_EAST = (int)(BorderEnum.South | BorderEnum.East);
        private static int SOUTH_AND_WEST = (int)(BorderEnum.South | BorderEnum.West);

        private int border;
        public void addBorder(BorderEnum b)
        {
            switch (b)
            {
                case BorderEnum.NorthWest:
                    if ((border & NORTH_AND_WEST) != 0) { return; }
                    break;
                case BorderEnum.NorthEast:
                    if ((border & NORTH_AND_EAST) != 0) { return; }
                    break;
                case BorderEnum.SouthEast:
                    if ((border & SOUTH_AND_EAST) != 0) { return; }
                    break;
                case BorderEnum.SouthWest:
                    if ((border & SOUTH_AND_WEST) != 0) { return; }
                    break;
                default:
                    break;
            }
            border |= (int)b;

        }

        public int Border
        {
            get { return border; }
        }

        #endregion
        #region Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="terrain">the tile's terrain</param>
        public Tile(Terrain terrain)
        {
            this.Terrain = terrain;
        }

        #endregion


        #region Draw

        /// <summary>
        /// Draw the tile
        /// </summary>
        public void draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
        {
            // TODO: fix how larger terrains such as the mountain (50x50 px) are drawn

            // draw terrain
            //spriteBatch.Draw(terrain.Sprite, position, Color.White);
            int terrainIndex = terrain.SpriteSheet.GetIndex(terrain.Name);

            spriteBatch.Draw(terrain.SpriteSheet.Texture, // texture
                             destinationRectangle, // destination rectangle
                             terrain.SpriteSheet.SourceRectangle(terrainIndex), // source rectangle (from the sprite sheet)
                             Color.White); // background color

            // draw structure on this tile (dungeon, forest, town, etc)
            // TODO: draw structure
        }

        #endregion
    }
}
