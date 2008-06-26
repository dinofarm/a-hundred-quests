using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace OHQ
{
    /// <summary>
    /// 100 Quest's game class
    /// </summary>
    public class OHQGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        private const int PREFFERED_SCREEN_WIDTH_PX  = 1280;
        private const int PREFERRED_SCREEN_HEIGHT_PX = 720;

        public OHQGame()
        {
            // content directory
            Content.RootDirectory = "Content";

            // graphics system
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth  = PREFFERED_SCREEN_WIDTH_PX;
            graphics.PreferredBackBufferHeight = PREFERRED_SCREEN_HEIGHT_PX;

            // screen manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new MenuBackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: initialize the input manager

            base.Initialize();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // clear the display
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }


        #region Entry Point


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (OHQGame game = new OHQGame())
            {
                game.Run();
            }
        }


        #endregion
    }
}
