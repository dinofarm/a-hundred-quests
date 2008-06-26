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
            set { terrain = value; }
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
        public void draw(SpriteBatch spriteBatch, Rectangle position)
        {
            // TODO: fix how larger terrains such as the mountain (50x50 px) are drawn

            // draw terrain
            spriteBatch.Draw(terrain.Sprite, position, Color.White);

            // draw structure on this tile (dungeon, forest, town, etc)
            // TODO: draw structure
        }

        #endregion
    }
}
