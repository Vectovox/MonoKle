namespace MonoKle
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using MonoKle.Input;
    using MonoKle.Graphics;
    using MonoKle.State;
    using MonoKle.Assets;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MonoKleGame : Game
    {
        private static MonoKleGame gameInstance = null;

        public static MonoKleGame GetInstance()
        {
            if (gameInstance == null)
            {
                gameInstance = new MonoKleGame();
            }
            return gameInstance;
        }

        public MonoKleGame()
            : base()
        {
            GraphicsManager.SetGraphicsDeviceManager(new GraphicsDeviceManager(this));
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            double seconds = gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            StateManager.Draw(seconds);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            TextureManager.Initialize();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            double seconds = gameTime.ElapsedGameTime.TotalSeconds;
            
            KeyboardInput.Update(seconds);
            MouseInput.Update(seconds);
            StateManager.Update(seconds);
        }
    }
}