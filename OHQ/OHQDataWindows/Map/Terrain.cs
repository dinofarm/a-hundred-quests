using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OHQData.Sprites;

namespace OHQData
{
    public class Terrain : ContentObject
    {
        public string Name
        {
            get
            {
                // Remove the path part of the assetname, leaving just the filename
                int start = AssetName.LastIndexOf('\\') + 1;
                string name = AssetName.Substring(start);

                return name;
            }
        }


        private bool isWalkable = true;

        [ContentSerializerIgnore]
        public bool IsWalkable
        {
            get { return isWalkable; }
            set { isWalkable = value; }
        }

        private static SpriteSheet spriteSheet;

        [ContentSerializerIgnore]
        public SpriteSheet SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }

        private Sprite sprite;

        [ContentSerializerIgnore]
        public Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

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

        #region Update and draw

        public void update()
        {

        }

        public void draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
        {
            sprite.draw(spriteBatch, destinationRectangle);
        }

        #endregion 


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

                if (terrain.SpriteSheet == null)
                {
                    string path = @"Textures\Map\Terrains\SpriteSheet";
                    terrain.SpriteSheet = input.ContentManager.Load<SpriteSheet>(path);
                }

                terrain.Sprite = new Sprite(terrain.Name, terrain.SpriteSheet);


                return terrain;
            }
        }


        #endregion
    }
}
