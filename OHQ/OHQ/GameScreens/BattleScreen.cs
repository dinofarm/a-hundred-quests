using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OHQData;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace OHQ.GameScreens
{
    class BattleScreen : GameScreen
    {
        Texture2D background;
        Texture2D character;
        AnimatedTexture sprite;
        ContentManager content;

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            background = content.Load<Texture2D>("forestbackground");
        }
          /// <summary>
        /// Constructor.
        /// </summary>
        public BattleScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(1.0);

            // temp texture
            const float rotation = 0;
            const float scale = 1.0f;
            const float depth = 0.5f;
            sprite = new AnimatedTexture(Vector2.Zero, rotation, scale, depth);
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            spriteBatch.Begin(SpriteBlendMode.None);

            spriteBatch.Draw(background, fullscreen,
                             new Color(fade, fade, fade));

            spriteBatch.End();
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


                for (int i = 0; i < InputState.MaxInputs; i++)
                {
                    if (input.CurrentKeyboardStates[i].IsKeyDown(Keys.B))
                        ScreenManager.RemoveScreen(this);

                }
            }
        }
    }
}
