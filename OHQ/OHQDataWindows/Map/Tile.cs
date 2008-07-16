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

        private int BorderSides()
        {
            return border & 0xF;
        }

        private int BorderCorners()
        {
            return 16 + ((border >> 4) & 0xFF);
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
        public void draw(SpriteBatch spriteBatch, Point point)
        {
            // TODO: Drawing of a tile is dependent on information in the tile, not
            // just information in the Terrain class. So the draing logic should be in the 
            // Tile class, with information taken from the Terrain class.
            terrain.draw(spriteBatch, point, BorderSides(), BorderCorners());
        }

        #endregion
    }
}
