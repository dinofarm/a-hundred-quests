#region File Description
//-----------------------------------------------------------------------------
// AnimatingSprite.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace OHQData.Sprites
{
    /// <summary>
    /// A sprite with multiple animations
    /// </summary>
    public class AnimatingSprite : Sprite
    {
        #region Members

        private List<Animation> animations;

        private Animation currentAnimation;
        public Animation CurrentAnimation
        {
            get { return currentAnimation; }
            set { currentAnimation = value; }
        }

        #endregion

        public AnimatingSprite(string name, SpriteSheet spriteSheet) : base(name, spriteSheet)
        {
            animations = new List<Animation>();
        }

        /// <summary>
        /// Stop the current animation
        /// </summary>
        public void stopAnimation()
        {
            try { currentAnimation.stop(); }
            catch (NullReferenceException ex) { Console.Out.WriteLine(ex); }
        }

        /// <summary>
        /// Start the current animation
        /// </summary>
        public void startAnimation()
        {
            try { currentAnimation.play(); }
            catch (NullReferenceException ex) { Console.Out.WriteLine(ex); }
        }
    }
}
