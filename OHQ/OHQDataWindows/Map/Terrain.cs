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
        #region Name
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
        #endregion

        #region Walkable
        private bool isWalkable = true;

        [ContentSerializer(Optional = true)]
        public bool IsWalkable
        {
            get { return isWalkable; }
            set { isWalkable = value; }
        }
        #endregion
        #region Borders
        private string borderName = "";

        [ContentSerializer(Optional = true)]
        public string BorderName
        {
            get { return borderName; }
            set { borderName = value; }
        }

        private bool border = false;

        [ContentSerializer(Optional = true)]
        public bool Borders
        {
            get { return border; }
            set { border = value; }
        }
        #endregion

        #region AnimationFrames
        private int animationFrames = 1;

        [ContentSerializer(Optional = true)]
        public int AnimationFrames
        {
            get { return animationFrames; }
            set { animationFrames = value; }
        }
        #endregion

        #region Sprite Data
        private static SpriteSheet spriteSheet;

        [ContentSerializerIgnore]
        public SpriteSheet SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }

        private TerrainSprite sprite;
        /*
        [ContentSerializerIgnore]
        public Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }*/
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

        #region Update and draw

        public void update()
        {

        }

        public void draw(SpriteBatch spriteBatch, Point point, int BorderSides,int BorderCorners)
        {
            sprite.draw(spriteBatch, point, BorderSides,BorderCorners);
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

                terrain.AssetName = input.ReadString();
                terrain.SpriteSheet = input.ContentManager.Load<SpriteSheet>(input.ReadString());
                terrain.IsWalkable = input.ReadBoolean();
                terrain.Borders = input.ReadBoolean();
                terrain.BorderName = input.ReadString();
                terrain.AnimationFrames = input.ReadInt32();

                terrain.sprite = new TerrainSprite(terrain);

                return terrain;
            }
        }

        #endregion
    }

    // TODO: TEliminate this abomination! Or at least make it differently.
    internal class TerrainSprite
    {
        Terrain terrain;

        public TerrainSprite(Terrain t)
        {
            terrain = t;
        }


        public void draw(SpriteBatch spriteBatch, Point point, int BorderSides, int BorderCorners) 
        {
            // TODO: fix how larger terrains such as the mountain (50x50 px) are drawn

            // draw terrain
            if (terrain.AnimationFrames > 1)
            {
                int terrainIndex = terrain.SpriteSheet.GetIndex(terrain.Name + "_0");
                drawTileTerrain(spriteBatch, point, terrainIndex);
            }
            else
            {
                int terrainIndex = terrain.SpriteSheet.GetIndex(terrain.Name);
                drawTileTerrain(spriteBatch, point, terrainIndex);
            }

            if (terrain.Borders)
            {
                string tileType = terrain.BorderName + "_";
                if (BorderSides != 0)
                {
                    int terrainIndex = terrain.SpriteSheet.GetIndex(tileType + BorderSides);
                    drawTileTerrain(spriteBatch, point, terrainIndex);
                }
                if (BorderCorners != 0)
                {
                    int terrainIndex = terrain.SpriteSheet.GetIndex(tileType + BorderCorners);
                    drawTileTerrain(spriteBatch, point, terrainIndex);
                }
            }

        }

        private void drawTileTerrain(SpriteBatch spriteBatch, Point point, int terrainIndex)
        {
            terrain.SpriteSheet.SourceRectangle(terrainIndex).draw(spriteBatch, point);
        }

    }
}
