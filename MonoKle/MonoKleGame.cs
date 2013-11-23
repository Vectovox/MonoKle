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
    using MonoKle.Assets.Font;

    public class MonoKleGame : Game
    {
        public static bool IsRunningSlowly { get; private set; }
        public static TimeSpan TotalGameTime { get; private set; }
        
        public static StateManager StateManager { get; private set; }
        public static TextureManager TextureManager { get; private set; }
        public static FontManager FontManager { get; private set; }
        public static GraphicsManager GraphicsManager { get; private set; }
        public static MouseInput Mouse { get; private set; }
        public static KeyboardInput Keyboard { get; private set; }
        public static GamePadInput GamePad { get; private set; }

        private static MonoKleGame gameInstance;
        
        public static MonoKleGame GetInstance()
        {
            if (gameInstance == null)
            {
                gameInstance = new MonoKleGame();
            }
            return gameInstance;
        }

        private MonoKleGame()
            : base()
        {
            Content.RootDirectory = "Content";
            StateManager = new StateManager();
            GraphicsManager = new GraphicsManager(new GraphicsDeviceManager(this));
            Mouse = new MouseInput();
            GamePad = new GamePadInput();
            Keyboard = new KeyboardInput();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            double seconds = gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            StateManager.Draw(seconds);
        }

        protected override void LoadContent()
        {
            TextureManager = new TextureManager(GraphicsManager.GetGraphicsDevice());
            FontManager = new FontManager(GraphicsManager.GetGraphicsDevice());
        }

        protected override void Update(GameTime gameTime)
        {
            double seconds = gameTime.ElapsedGameTime.TotalSeconds;

            IsRunningSlowly = gameTime.IsRunningSlowly;
            TotalGameTime = gameTime.TotalGameTime;

            GamePad.Update(seconds);
            Keyboard.Update(seconds);
            Mouse.Update(seconds, GraphicsManager.ScreenSize);
            StateManager.Update(seconds);
        }
    }
}