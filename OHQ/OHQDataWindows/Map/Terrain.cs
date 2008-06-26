using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OHQData
{
    public class Terrain : ContentObject
    {
        #region Sprite

        /// <summary>
        /// The tile sprite
        /// </summary>
        private Texture2D sprite;

        /// <summary>
        /// The tile sprite
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        #endregion

        #region Count

        /// <summary>
        /// The number of terrains of this type on the map
        /// </summary>
        private int count = 0;

        /// <summary>
        /// The number of terrains of this type on the map
        /// </summary>
        [ContentSerializerIgnore]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// 
        /// When the terrain is first initialized, the count is 1.
        /// Increment the count whenever assigning a terrain to a tile to keep
        /// it up to date. // TODO: auto-increment the count when a terrain is added/removed from a tile
        /// </summary>
        public Terrain()
        {
            count = 1;
        }

        #region Content type reader

        /// <summary>
        /// Read a Terrain object from the content pipeline.
        /// </summary>
        public class TerrainReader : ContentTypeReader<Terrain>
        {
            protected override Terrain Read(ContentReader input, Terrain existingInstance)
            {
                Terrain terrain = existingInstance;
                if (terrain == null)
                {
                    terrain = new Terrain();
                }

                terrain.AssetName = input.AssetName;

                try
                {
                    terrain.Sprite = input.ContentManager.Load<Texture2D>(
                        System.IO.Path.Combine(@"Textures", terrain.AssetName));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(0);
                }

                return terrain;
            }
        }


        #endregion
    }
}
