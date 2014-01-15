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
    using MonoKle.Messaging;
    using MonoKle.Logging;
    using System.IO;

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
        public static PrimitiveDrawer PrimitiveDrawer { get; private set; }
        public static MessagePasser MessagePasser { get; private set; }
        
        /// <summary>
        /// Loggin utility, same as <see cref="Logger.GetGlobalInstance()"/>.
        /// </summary>
        public static Logger Logger { get; private set; }

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
            MessagePasser = new MessagePasser();
            MonoKleGame.Logger = Logger.GetGlobalInstance();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MonoKleGame.Logger.AddLog(e.ExceptionObject.ToString(), LogLevel.Error);
            FileStream fs = new FileStream("./crashdump.log", FileMode.OpenOrCreate | FileMode.Truncate);
            MonoKleGame.Logger.WriteLog(fs); // TODO: Remove magic constant. Not into a constants class, but into settings! E.g. Settings.GetValue("crashdump").
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            double seconds = gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            StateManager.Draw(seconds);
            PrimitiveDrawer.Render();
        }

        protected override void LoadContent()
        {
            TextureManager = new TextureManager(GraphicsManager.GetGraphicsDevice());
            FontManager = new FontManager(GraphicsManager.GetGraphicsDevice());
            PrimitiveDrawer = new PrimitiveDrawer(GraphicsManager.GetGraphicsDevice());
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