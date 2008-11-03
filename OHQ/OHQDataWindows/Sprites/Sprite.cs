using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OHQData.Sprites
{
    // TODO: refactor the package this is in

    /// <summary>
    /// A two-dimensional image taken from a sprite sheet
    /// </summary>
    public class Sprite
    {
        // A basic sprite only has a single frame.
        private Frame frame;

        public Sprite(string name, SpriteSheet spriteSheet)
        {
            Debug.Assert(name.Length > 0, "sprite name was an empty string");
            Debug.Assert(spriteSheet != null, "sprite sheet was null");

            frame = spriteSheet.SourceRectangle(name);
            //this.sourceRectangle = spriteSheet.SourceRectangle(name);
            //this.spriteSheet = spriteSheet;
        }

        public void update()
        {

        }
        public void draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
        {
            Point p = new Point(destinationRectangle.X,destinationRectangle.Y);
            frame.draw(spriteBatch,p);
        }
    }
}
