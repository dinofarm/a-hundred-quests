#region File Description
//-----------------------------------------------------------------------------
// SpriteSheet.cs
//
// Microsoft Game Technology Group
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace OHQData.Sprites
{

    /// <summary>
    /// This is an encapsulation of a rectangular piece of graphics that is stored in a SpriteSheet.
    /// This class might also contain the center point of a graphic, though I'd say
    /// it should just be the "blob o' pixels".
    /// </summary>
    public class Frame
    {
        // Source of the rectangle.
        private SpriteSheet sheet;

        // What area to draw from the spriteSheet
        private Rectangle SourceRectangle;

        internal Frame(SpriteSheet sheet, Rectangle source)
        {
            this.sheet = sheet;
            SourceRectangle = source;
        }

        public void draw(SpriteBatch spriteBatch, Point topLeft)
        {
            Rectangle destination = new Rectangle(topLeft.X, topLeft.Y, SourceRectangle.Width, SourceRectangle.Height);
            spriteBatch.Draw(sheet.Texture, destination, SourceRectangle, Color.White);
        }
    }

    /// <summary>
    /// A sprite sheet contains many individual sprite images, packed into different
    /// areas of a single larger texture, along with information describing where in
    /// that texture each sprite is located. Sprite sheets can make your game drawing
    /// more efficient, because they reduce the number of times the graphics hardware
    /// needs to switch from one texture to another.
    /// </summary>
    public class SpriteSheet : ContentObject
    {
        // Single texture contains many separate sprite images.
        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        // Remember where in the texture each sprite has been placed.
        private Rectangle[] spriteRectangles;
        public Rectangle[] SpriteRectangles
        {
            get { return spriteRectangles; }
            set { spriteRectangles = value; }
        }

        // Store the original sprite filenames, so we can look up sprites by name.
        private Dictionary<string, int> spriteNames;
        public Dictionary<string, int> SpriteNames
        {
            get { return spriteNames; }
            set { spriteNames = value; }
        }


        /// <summary>
        /// The constructor is internal: this should only be
        /// called by the SpriteSheetReader support class.
        /// </summary>
        internal SpriteSheet(ContentReader input)
        {
            texture = input.ReadObject<Texture2D>();
            spriteRectangles = input.ReadObject<Rectangle[]>();
            spriteNames = input.ReadObject<Dictionary<string, int>>();
        }


        /// <summary>
        /// Looks up the location of the specified sprite within the big texture.
        /// </summary>
        public Frame SourceRectangle(string spriteName)
        {
            int spriteIndex = GetIndex(spriteName);

            return new Frame(this, spriteRectangles[spriteIndex]);
        }


        /// <summary>
        /// Looks up the location of the specified sprite within the big texture.
        /// </summary>
        public Frame SourceRectangle(int spriteIndex)
        {
            if ((spriteIndex < 0) || (spriteIndex >= spriteRectangles.Length))
                throw new ArgumentOutOfRangeException("spriteIndex");

            return new Frame(this, spriteRectangles[spriteIndex]);
        }


        /// <summary>
        /// Looks up the numeric index of the specified sprite. This is useful when
        /// implementing animation by cycling through a series of related sprites.
        /// </summary>
        public int GetIndex(string spriteName)
        {
            int index;

            if (!spriteNames.TryGetValue(spriteName, out index))
            {
                string error = "SpriteSheet does not contain a sprite named '{0}'.";

                throw new KeyNotFoundException(string.Format(error, spriteName));
            }

            return index;
        }

    }

    /// <summary>
    /// Content pipeline support class for reading sprite sheet data from XNB format.
    /// </summary>
    public class SpriteSheetReader : ContentTypeReader<SpriteSheet>
    {
        /// <summary>
        /// Loads sprite sheet data from an XNB file.
        /// </summary>
        protected override SpriteSheet Read(ContentReader input,
                                            SpriteSheet existingInstance)
        {
            return new SpriteSheet(input);
        }
    }
}
