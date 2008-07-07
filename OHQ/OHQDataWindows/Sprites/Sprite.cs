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
        private Rectangle sourceRectangle;
        private SpriteSheet spriteSheet;

        public Sprite(string name, SpriteSheet spriteSheet)
        {
            Debug.Assert(name.Length > 0, "sprite name was an empty string");
            Debug.Assert(spriteSheet != null, "sprite sheet was null");

            this.sourceRectangle = spriteSheet.SourceRectangle(name);
            this.spriteSheet = spriteSheet;
        }

        public void update()
        {

        }
        public void draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
        {
            // This makes sure that sprites are drawn in their natural size.
            Rectangle target = AdjustDestinationRectangle(destinationRectangle);

            spriteBatch.Draw(spriteSheet.Texture,
                             target,//destinationRectangle,
                             sourceRectangle,
                             Color.White); // TODO: refactor the background color as a property?
        }

        private Rectangle AdjustDestinationRectangle(Rectangle destinationRectangle)
        {
            int xMod = (sourceRectangle.Width - destinationRectangle.Width)/2;
            int yMod = (sourceRectangle.Height - destinationRectangle.Height);


            Rectangle target = new Rectangle(destinationRectangle.X-xMod,
                                             destinationRectangle.Y-yMod,
                                             sourceRectangle.Width,
                                             sourceRectangle.Height);
            return target;
        }
    }
}
