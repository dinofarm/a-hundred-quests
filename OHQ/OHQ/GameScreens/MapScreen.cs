#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OHQData;
#endregion

namespace OHQ
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class MapScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        Vector2 playerPosition = new Vector2(0, 0);

        Random random = new Random();

        GameState gameState;

        Texture2D character;
        AnimatedTexture sprite;

        Map map;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public MapScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            // temp texture
            float rotation = 0;
            float scale = 1.0f;
            float depth = 0.5f;
            sprite = new AnimatedTexture(Vector2.Zero, rotation, scale, depth);

            gameState = GameState.Waiting;

            map = new Map(MapSize.Huge);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("gamefont");

            character = content.Load<Texture2D>(@"Textures\Characters\HumanAvatarMale1");

            // character sprite
            sprite.Load(content, @"Textures\Characters\HumanAvatarMale1", 8, 2);

            // map
            map.LoadContent(content);


            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                sprite.UpdateFrame(elapsed);
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.PauseGame)
            {
                // If they pressed pause, bring up the pause menu screen.
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;

                for (int i = 0; i < InputState.MaxInputs; i++)
                {
                    if (input.CurrentKeyboardStates[i].IsKeyDown(Keys.Left))
                        movement.X--;

                    if (input.CurrentKeyboardStates[i].IsKeyDown(Keys.Right))
                        movement.X++;

                    if (input.CurrentKeyboardStates[i].IsKeyDown(Keys.Up))
                        movement.Y--;

                    if (input.CurrentKeyboardStates[i].IsKeyDown(Keys.Down))
                        movement.Y++;

                    //Vector2 thumbstick = input.CurrentGamePadStates[i].ThumbSticks.Left;

                    //movement.X += thumbstick.X;
                   // movement.Y -= thumbstick.Y;
                }

                if (movement.Length() > 1)
                    movement.Normalize();

                playerPosition += movement;
                if (!map.getTile(playerPosition).IsWalkable)
                {
                    playerPosition -= movement;
                }
                //playerPosition += movement * 2;
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // draw the map
            map.draw(spriteBatch, (int)playerPosition.X, (int)playerPosition.Y);

            Vector2 centerPlayer = new Vector2(16 * Tile.SizePx, 10 * Tile.SizePx);
            sprite.DrawFrame(spriteBatch, centerPlayer);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }


        #endregion
    }

    /// <summary>
    /// An enumeration of game states
    /// </summary>
    public enum GameState
    {
        Waiting,
        Walking,
    }

    /// <summary>
    /// An enumeration of menu options
    /// </summary>
    public enum MenuOption
    {
        Move,
        Magic,
        Equip,
        Party,
        Items,
        Stats,
        Config,
        Enter,
        EndTurn,
    }
}
