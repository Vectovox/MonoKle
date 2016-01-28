namespace MonoKle.Engine
{
    using Asset.Effect;
    using Asset.Font;
    using Asset.Texture;
    using Console;
    using Graphics;
    using Input;
    using Logging;
    using Messaging;
    using Microsoft.Xna.Framework;
    using Resources;
    using Script;
    using State;
    using System;
    using System.IO;

    /// <summary>
    /// Backend for the MonoKle engine. Provides global access to all MonoKle systems.
    /// </summary>
    public static class MBackend
    {
        private static GameConsole console;
        private static GamePadInput gamepad;
        private static bool initializing;
        private static KeyboardInput keyboard;
        private static MouseInput mouse;
        private static StateSystem stateSystem;

        /// <summary>
        /// Gets the game console, printing logs and accepting input.
        /// </summary>
        public static IGameConsole Console { get { return MBackend.console; } }

        /// <summary>
        /// Gets a value indicating whether the console is enabled.
        /// </summary>
        public static bool ConsoleEnabled { get; }

        /// <summary>
        /// Gets the effect storage, loading and providing effects.
        /// </summary>
        public static EffectStorage EffectStorage
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
        /// Gets the game instance.
        /// </summary>
        /// <value>
        /// The game instance.
        /// </value>
        public static MGame GameInstance { get; private set; }

        /// <summary>
        /// Gets the gamepad input.
        /// </summary>
        public static IGamePadInput GamePad { get { return MBackend.gamepad; } }

        /// <summary>
        /// Gets the graphics manager. This is in charge of screen settings (resolution, full-screen, etc.) and provides the <see cref="GraphicsDevice"/>.
        /// </summary>
        public static GraphicsManager GraphicsManager
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets wether the game is running slowly or not.
        /// </summary>
        public static bool IsRunningSlowly
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the keyboard input.
        /// </summary>
        public static IKeyboardInput Keyboard { get { return MBackend.keyboard; } }

        /// <summary>
        /// Gets the logging utility, same as <see cref="Logger.Global"/>.
        /// </summary>
        public static Logger Logger
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the message passing utility.
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
            get { return MBackend.mouse; }
        }

        /// <summary>
        /// Gets the script environment. Used for scripting.
        /// </summary>
        public static ScriptEnvironment ScriptEnvironment
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the state system, which keeps track of the states and switches between them.
        /// </summary>
        public static IStateSystem StateSystem { get { return MBackend.stateSystem; } }

        /// <summary>
        /// Gets the texture storage, loading and providing textures.
        /// </summary>
        public static TextureStorage TextureStorage
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
        /// Initializes the MonoKle backend, returning a runnable game instance.
        /// </summary>
        /// <param name="enableConsole">Enables console.</param>
        /// <returns>Runnable <see cref="MGame"/>.</returns>
        public static MGame Initialize(bool enableConsole)
        {
            MBackend.initializing = true;

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            MBackend.Logger = Logger.Global;

            MBackend.GameInstance = new MGame();
            MBackend.GraphicsManager = new GraphicsManager(new GraphicsDeviceManager(MBackend.GameInstance));
            MBackend.gamepad = new GamePadInput();
            MBackend.keyboard = new KeyboardInput();
            MBackend.mouse = new MouseInput();
            MBackend.stateSystem = new StateSystem();

            MBackend.GameInstance.RunOneFrame();

            MBackend.InitializeTextureStorage();
            MBackend.InitializeFontStorage();
            MBackend.EffectStorage = new EffectStorage(GraphicsManager.GetGraphicsDevice());
            MBackend.MessagePasser = new MessagePasser();
            MBackend.InitializeConsole();

            mouse.ScreenSize = GraphicsManager.ScreenSize;

            MBackend.initializing = false;

            return MBackend.GameInstance;
        }

        internal static void Draw(GameTime time)
        {
            if (MBackend.initializing == false)
            {
                double seconds = time.ElapsedGameTime.TotalSeconds;
                MBackend.GraphicsManager.GetGraphicsDevice().Clear(Color.CornflowerBlue);
                MBackend.stateSystem.Draw(seconds);
                MBackend.console.Draw(seconds);
            }
        }

        internal static void Update(GameTime time)
        {
            if (MBackend.initializing == false)
            {
                double seconds = time.ElapsedGameTime.TotalSeconds;
                MBackend.IsRunningSlowly = time.IsRunningSlowly;
                MBackend.TotalGameTime = time.TotalGameTime;
                MBackend.console.Update(seconds);
                MBackend.gamepad.Update(seconds);
                MBackend.keyboard.Update(seconds);
                MBackend.mouse.Update(seconds);
                MBackend.stateSystem.Update(seconds);
            }
        }

        private static void CommandExit(string[] arguments)
        {
            MBackend.GameInstance.Exit();
        }

        private static void CommandLogLevel(string[] arguments)
        {
            LogLevel level;
            if (Enum.TryParse(arguments[0], out level))
            {
                MBackend.Logger.LoggingLevel = level;
                MBackend.Console.WriteLine("Logging level set to: " + level);
            }
            else
            {
                MBackend.Console.WriteLine("Incorrect logging level specified.");
            }
        }

        private static void InitializeConsole()
        {
            // Set up font
            using (MemoryStream ms = new MemoryStream(FontResources.ConsoleFont))
            {
                MBackend.FontStorage.LoadStream(ms, "console");
            }
            MBackend.console = new GameConsole(new Rectangle(0, 0, GraphicsManager.ScreenSize.X, GraphicsManager.ScreenSize.Y / 3),
                MBackend.GraphicsManager.GetGraphicsDevice(),
                MBackend.keyboard,
                MBackend.TextureStorage.White);    // TODO: Break out magic numbers into config file.
            MBackend.Console.ToggleKey = Microsoft.Xna.Framework.Input.Keys.F1;
            MBackend.Console.CommandBroker.Register("exit", CommandExit);
            MBackend.Console.CommandBroker.Register("loglevel", 1, MBackend.CommandLogLevel);
            MBackend.Console.TextFont = MBackend.FontStorage.GetAsset("console");
        }

        private static void InitializeFontStorage()
        {
            MBackend.FontStorage = new FontStorage(GraphicsManager.GetGraphicsDevice());
            using (MemoryStream ms = new MemoryStream(Resources.FontResources.DefaultFont))
            {
                MBackend.FontStorage.LoadStream(ms, "default");
                MBackend.FontStorage.DefaultValue = MBackend.FontStorage.GetAsset("default");
            }
        }

        private static void InitializeTextureStorage()
        {
            MBackend.TextureStorage = new TextureStorage(GraphicsManager.GetGraphicsDevice(),
                GraphicsHelper.ImageToTexture2D(GraphicsManager.GetGraphicsDevice(), TextureResources.DefaultTexture),
                GraphicsHelper.ImageToTexture2D(GraphicsManager.GetGraphicsDevice(), TextureResources.WhiteTexture)
                );
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MBackend.Logger.Log(e.ExceptionObject.ToString(), LogLevel.Error);
            FileStream fs = new FileStream("./crashdump.log", FileMode.OpenOrCreate | FileMode.Truncate);
            MBackend.Logger.WriteLog(fs); // TODO: Remove magic constant. Not into a constants class, but into settings! E.g. Settings.GetValue("crashdump").
        }
    }
}