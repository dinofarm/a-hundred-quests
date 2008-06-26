#region File Description
//-----------------------------------------------------------------------------
// Player.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OHQData.Characters;
using OHQData.Data;
#endregion

namespace OHQData.Characters
{
    /// <summary>
    /// A member of the player's party, also represented in the world before joining.
    /// </summary>
    /// <remarks>
    /// There is only one of a given Player in the game world at a time, and their 
    /// current statistics persist after combat.  Thererefore, current statistics 
    /// are tracked here.
    /// </remarks>
    public class Player : FightingCharacter
    {
        #region Content Type Reader


        /// <summary>
        /// Read a Player object from the content pipeline.
        /// </summary>
        public class PlayerReader : ContentTypeReader<Player>
        {
            protected override Player Read(ContentReader input, Player existingInstance)
            {
                Player player = existingInstance;
                if (player == null)
                {
                    player = new Player();
                }

                input.ReadRawObject<FightingCharacter>(player as FightingCharacter);

                return player;
            }
        }


        #endregion
    }
}
