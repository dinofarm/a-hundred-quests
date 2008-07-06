#region File Description
//-----------------------------------------------------------------------------
// Animation.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
#endregion

namespace OHQData.Sprites
{
    /// <summary>
    /// An animation description for an AnimatingSprite object.
    /// </summary>
#if !XBOX
    [DebuggerDisplay("Name = {Name}")]
#endif
    public class Animation : ContentObject
    {
        #region Members

        /// <summary>
        /// The name of the animation.
        /// </summary>
        private string textureName;
        /// <summary>
        /// The name of the animation.
        /// </summary>
        public string TextureName
        {
            get { return textureName; }
            set { textureName = value; }
        }


        /// <summary>
        /// Number of frames in the animation
        /// </summary>
        private int frameCount;
        /// <summary>
        /// Number of frames in the animation
        /// </summary>
        public int FrameCount
        {
            get { return frameCount; }
            set { frameCount = value; }
        }


        /// <summary>
        /// Number of animation frames to display per second
        /// </summary>
        private int framesPerSecond;
        /// <summary>
        /// Number of animation frames to display per second
        /// </summary>
        public int FramesPerSecond
        {
            get { return framesPerSecond; }
            set { framesPerSecond = value; }
        }

        /// <summary>
        /// If true, the animation loops.
        /// </summary>
        private bool looping;
        /// <summary>
        /// If true, the animation loops.
        /// </summary>
        public bool Looping
        {
            get { return looping; }
            set { looping = value; }
        }
        
        private AnimationState state;
        [ContentSerializerIgnore]
        public AnimationState State
        {
            get { return state; }
            set { state = value; }
        }

        
        private int currentFrame;
        [ContentSerializerIgnore]
        public int CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        /// <summary>
        /// Time elapsed since the beginning of this animation.
        /// Does not increase while the animation is stopped.
        /// </summary>
        private double timeElapsed;
        [ContentSerializerIgnore]
        public double TimeElapsed
        {
            get { return timeElapsed; }
            set { timeElapsed = value; }
        }

        #endregion


        #region Constructors


        /// <summary>
        /// Creates a new Animation object.
        /// </summary>
        private Animation()
        {
            CurrentFrame = 1;
            State = AnimationState.Stopped;
            TimeElapsed = 0;
        }


        /// <summary>
        /// Creates a new Animation object by full specification.
        /// </summary>
        public Animation(string textureName, int frameCount, int framesPerSecond, bool looping) : this()
        {
            this.TextureName = textureName;
            this.FrameCount = frameCount;
            this.FramesPerSecond = framesPerSecond;
            this.Looping = looping;
        }


        #endregion


        #region Controls

        public void stop()
        {
            state = AnimationState.Stopped;
        }
        public void play()
        {
            state = AnimationState.Playing;
        }
        public void pause()   {
            if (state == AnimationState.Playing)
            {
                state = AnimationState.Paused;
            }
        }
        public void unpause()
        {
            if (state == AnimationState.Paused)
            {
                state = AnimationState.Playing;
            }
        }
        public void reset()
        {
            CurrentFrame = 1;
        }

        #endregion


        #region Content Type Reader


        /// <summary>
        /// Read an Animation object from the content pipeline.
        /// </summary>
        public class AnimationReader : ContentTypeReader<Animation>
        {
            /// <summary>
            /// Read an Animation object from the content pipeline.
            /// </summary>
            protected override Animation Read(ContentReader input,
                Animation existingInstance)
            {
                Animation animation = existingInstance;
                if (animation == null)
                {
                    animation = new Animation();
                }

                animation.AssetName = input.AssetName;

                animation.TextureName = input.ReadString();
                animation.FrameCount = input.ReadInt32();
                animation.FramesPerSecond = input.ReadInt32();
                animation.Looping = input.ReadBoolean();

                return animation;
            }
        }


        #endregion
    }

    public enum AnimationState
    {
        Paused,
        Stopped,
        Playing,
    }
}
