namespace MonoKle
{
    using System;
    using System.IO;

    using Microsoft.Xna.Framework;

    using MonoKle.Assets;
    using MonoKle.Assets.Font;
    using MonoKle.Console;
    using MonoKle.Graphics;
    using MonoKle.Input;
    using MonoKle.Logging;
    using MonoKle.Messaging;
    using MonoKle.State;
    //using MonoKle.Script.Interface;
    using System.Text.RegularExpressions;
    using System.Text;
    using System.Collections.Generic;
    using MonoKle.Assets.Effect;
    using MonoKle.Script;
    using Assets.Texture;
    using Microsoft.Xna.Framework.Graphics;
    using Resources;

    /// <summary>
    /// Main game class for MonoKle. Takes care of initiating utilities and making them draw and update themselves.
    /// </summary>
    public class MonoKleGame : Game
    {
        private static MonoKleGame gameInstance;

        private MonoKleGame()
            : base()
        {
            base.Content.RootDirectory = "Content";
            MonoKleGame.StateManager = new StateManager();
            MonoKleGame.GraphicsManager = new GraphicsManager(new GraphicsDeviceManager(this));
            MonoKleGame.GamePad = new GamePadInput();
            MonoKleGame.Keyboard = new KeyboardInput();
            MonoKleGame.MessagePasser = new MessagePasser();
            MonoKleGame.Logger = Logger.Global;
            //MonoKleGame.ScriptInterface = new ScriptInterface();
            //MonoKleGame.ScriptInterface.CompilationError += HandleScriptCompilationError;
            //MonoKleGame.ScriptInterface.Print += HandleScriptPrint;
            //MonoKleGame.ScriptInterface.RuntimeError += HandleScriptRuntimeError;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        //private void HandleScriptRuntimeError(object sender, Script.VM.Event.RuntimeErrorEventArgs e)
        //{
        //    MonoKleGame.Logger.Log(e.Message, LogLevel.Error);
        //}

        //private void HandleScriptPrint(object sender, Script.VM.Event.PrintEventArgs e)
        //{
        //    MonoKleGame.Console.WriteLine(e.Message);
        //}

        //private void HandleScriptCompilationError(object sender, Script.Interface.Event.CompilationErrorEventArgs e)
        //{
        //    StringBuilder sb = new StringBuilder("Script compilation error in [");
        //    sb.Append(e.Script);
        //    sb.AppendLine("]:");
        //    foreach(string s in e.Messages)
        //    {
        //        sb.AppendLine(s);
        //    }
        //    MonoKleGame.Logger.Log(sb.ToString(), LogLevel.Error);
        //}


        /// <summary>
        /// Gets the script environment. Used for scripting.
        /// </summary>
        public static ScriptEnvironment ScriptEnvironment
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the game console, printing logs and accepting input.
        /// </summary>
        public static GameConsole Console
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the font storage, loading and providing fonts.
        /// </summary>
        public static FontStorage FontStorage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the gamepad input utility.
        /// </summary>
        public static GamePadInput GamePad
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the graphics manager. This is in charge of screen settings (resolution, full-screen, etc.) and provides the <see cref="GraphicsDevice"/>.
        /// </summary>
        public static GraphicsManager GraphicsManager
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets wether the game is running slowly or not. True indicates that the game is running slowly.
        /// </summary>
        public static bool IsRunningSlowly
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the keyboard input utility.
        /// </summary>
        public static KeyboardInput Keyboard
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the logging utility, same as <see cref="Logger.Global"/>.
        /// </summary>
        public static Logger Logger
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the message parsing utility.
        /// </summary>
        public static MessagePasser MessagePasser
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the mouse input.
        /// </summary>
        public static IMouseInput Mouse
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the state manager. This keeps track of the states and is used to switch between them.
        /// </summary>
        public static StateManager StateManager
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the texture storage, loading and providing textures.
        /// </summary>
        public static TextureStorage TextureStorage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the effect storage, loading and providing effects.
        /// </summary>
        public static EffectStorage EffectStorage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total time spent in the game.
        /// </summary>
        public static TimeSpan TotalGameTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets an instance of <see cref="MonoKleGame"/>.
        /// </summary>
        /// <returns><see cref="MonoKleGame"/> instance.</returns>
        public static MonoKleGame GetInstance()
        {
            if (MonoKleGame.gameInstance == null)
            {
                MonoKleGame.gameInstance = new MonoKleGame();
            }
            return MonoKleGame.gameInstance;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            base.GraphicsDevice.Clear(Color.CornflowerBlue);
            double seconds = gameTime.ElapsedGameTime.TotalSeconds;
            MonoKleGame.StateManager.Draw(seconds);
            MonoKleGame.Console.Draw();
        }

        protected override void LoadContent()
        {
            this.SetUpTextureStorage();
            this.SetUpFontStorage();
            MonoKleGame.EffectStorage = new EffectStorage(GraphicsManager.GetGraphicsDevice());
            this.SetUpConsole();
            MonoKleGame.Mouse = new MouseInput(GraphicsManager.ScreenSize);
        }

        private void SetUpTextureStorage()
        {
            MonoKleGame.TextureStorage = new TextureStorage(GraphicsManager.GetGraphicsDevice(),
                GraphicsHelper.ImageToTexture2D(GraphicsManager.GetGraphicsDevice(), TextureResources.DefaultTexture),
                GraphicsHelper.ImageToTexture2D(GraphicsManager.GetGraphicsDevice(), TextureResources.WhiteTexture)
                );
        }

        private void SetUpFontStorage()
        {
            MonoKleGame.FontStorage = new FontStorage(GraphicsManager.GetGraphicsDevice());
            using (MemoryStream ms = new MemoryStream(Resources.FontResources.DefaultFont))
            {
                MonoKleGame.FontStorage.LoadStream(ms, "default");
                MonoKleGame.FontStorage.DefaultValue = MonoKleGame.FontStorage.GetAsset("default");
            }
        }

        protected override void Update(GameTime gameTime)
        {
            double seconds = gameTime.ElapsedGameTime.TotalSeconds;
            MonoKleGame.IsRunningSlowly = gameTime.IsRunningSlowly;
            MonoKleGame.TotalGameTime = gameTime.TotalGameTime;
            MonoKleGame.Console.Update(seconds);
            MonoKleGame.GamePad.Update(seconds);
            MonoKleGame.Keyboard.Update(seconds);
            (MonoKleGame.Mouse as MouseInput).Update(seconds);
            MonoKleGame.StateManager.Update(seconds);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MonoKleGame.Logger.Log(e.ExceptionObject.ToString(), LogLevel.Error);
            FileStream fs = new FileStream("./crashdump.log", FileMode.OpenOrCreate | FileMode.Truncate);
            MonoKleGame.Logger.WriteLog(fs); // TODO: Remove magic constant. Not into a constants class, but into settings! E.g. Settings.GetValue("crashdump").
        }

        private void SetUpConsole()
        {
            // Set up font
            using (MemoryStream ms = new MemoryStream(Resources.FontResources.ConsoleFont))
            {
                MonoKleGame.FontStorage.LoadStream(ms, "console");
            }
            MonoKleGame.Console = new GameConsole(new Rectangle(0, 0, GraphicsManager.ScreenSize.X, GraphicsManager.ScreenSize.Y / 3), GraphicsManager.GetGraphicsDevice());    // TODO: Break out magic numbers into config file.
            MonoKleGame.Console.ToggleKey = Microsoft.Xna.Framework.Input.Keys.F1;
            MonoKleGame.Console.CommandBroker.Register("exit", CommandExit);
            MonoKleGame.Console.TextFont = MonoKleGame.FontStorage.GetAsset("console");
        }

        private void CommandExit(string[] arguments)
        {
            this.Exit();
        }
    }
}