#region File Description
//-----------------------------------------------------------------------------
// FightingCharacter.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using OHQData.Data;
#endregion

namespace OHQData.Characters
{
    /// <summary>
    /// A character that engages in combat.
    /// </summary>
    public abstract class FightingCharacter : Character
    {
        #region Graphics Data


        /// <summary>
        /// The animating sprite for the combat view of this character.
        /// </summary>
        private AnimatingSprite sprite;

        /// <summary>
        /// The animating sprite for the combat view of this character.
        /// </summary>
        public AnimatingSprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }


        /// <summary>
        /// Reset the animations for this character.
        /// </summary>
        public override void ResetAnimation(bool isWalking)
        {
            base.ResetAnimation(isWalking);
            if (sprite != null)
            {
                sprite.PlayAnimation("Idle");
            }
        }


        #endregion


        #region Static Animation Data


        /// <summary>
        /// The default animation interval for the combat map sprite.
        /// </summary>
        private int animationInterval = 100;

        /// <summary>
        /// The default animation interval for the combat map sprite.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public int AnimationInterval
        {
            get { return animationInterval; }
            set { animationInterval = value; }
        }


        /// <summary>
        /// Add the standard character walk animations to this character.
        /// </summary>
        private void AddStandardCharacterAnimations()
        {
            if (sprite != null)
            {
                sprite.AddAnimation(new Animation("Idle", 37, 42,
                    AnimationInterval, true));
                sprite.AddAnimation(new Animation("Walk", 25, 30,
                    AnimationInterval, true));
            }
        }


        #endregion


        #region Content Type Reader


        /// <summary>
        /// Reads a FightingCharacter object from the content pipeline.
        /// </summary>
        public class FightingCharacterReader : ContentTypeReader<FightingCharacter>
        {
            /// <summary>
            /// Reads a FightingCharacter object from the content pipeline.
            /// </summary>
            protected override FightingCharacter Read(ContentReader input,
                FightingCharacter existingInstance)
            {
                FightingCharacter fightingCharacter = existingInstance;
                if (fightingCharacter == null)
                {
                    throw new ArgumentNullException("existingInstance");
                }

                input.ReadRawObject<Character>(fightingCharacter as Character);

                fightingCharacter.AnimationInterval = input.ReadInt32();
                fightingCharacter.Sprite = input.ReadObject<AnimatingSprite>();
                fightingCharacter.AddStandardCharacterAnimations();
                fightingCharacter.ResetAnimation(false);

                return fightingCharacter;
            }
        }


        #endregion
    }
}
