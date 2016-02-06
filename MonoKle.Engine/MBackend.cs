namespace MonoKle.Engine
{
    using Asset.Effect;
    using Asset.Font;
    using Asset.Texture;
    using Console;
    using Graphics;
    using Input;
    using Logging;
    using Microsoft.Xna.Framework;
    using Resources;
    using Script;
    using State;
    using System;
    using System.IO;
    using System.Reflection;

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
        /// Gets the game variables, including settings.
        /// </summary>
        /// <value>
        /// The variables.
        /// </value>
        public static VariableStorage Variables
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
            MBackend.InitializeVariables();

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
            MBackend.InitializeConsole();

            MBackend.RegisterConsoleCommands();
            MBackend.BindSettings();

            mouse.ScreenSize = GraphicsManager.ScreenSize;

            console.WriteLine("MonoKle Engine initialized!", Color.LightGreen);
            console.WriteLine("Running version: " + Assembly.GetAssembly(typeof(MBackend)).GetName().Version, Color.LightGreen);
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

        private static void BindSettings()
        {
            MBackend.Variables.Variables.BindProperties(MBackend.Logger);
            MBackend.Variables.Variables.BindProperties(MBackend.Console);
        }

        private static void CommandExit(string[] arguments)
        {
            MBackend.GameInstance.Exit();
        }

        private static void CommandGet(string[] arguments)
        {
            object value = MBackend.Variables.Variables.GetValue(arguments[0]);
            if (value != null)
            {
                Console.WriteLine(value.ToString());
            }
            else
            {
                Console.WriteLine("No such variable exist", Console.ErrorTextColour);
            }
        }

        private static void CommandListVariables(string[] arguments)
        {
            MBackend.Console.WriteLine("## variables ##");
            foreach (string s in MBackend.Variables.Variables.Identifiers)
            {
                MBackend.Console.WriteLine(s);
            }
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

        private static void CommandSet(string[] arguments)
        {
            if (MBackend.Variables.VariablePopulator.LoadItem(arguments[0], arguments[1]) == false)
            {
                Console.WriteLine("Variable assignment failed", Console.ErrorTextColour);
            }
        }

        private static void CommandVersion(string[] arguments)
        {
            MBackend.console.WriteLine("       MonoKle Version:\t" + Assembly.GetAssembly(typeof(MonoKle.Core.Timer)).GetName().Version);
            MBackend.console.WriteLine("MonoKle Engine Version:\t" + Assembly.GetAssembly(typeof(MBackend)).GetName().Version);
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

        private static void InitializeVariables()
        {
            MBackend.Variables = new VariableStorage(MBackend.Logger);
            MBackend.Variables.LoadDefaultVariables();
        }

        private static void RegisterConsoleCommands()
        {
            MBackend.Console.CommandBroker.Register("exit", CommandExit);
            MBackend.Console.CommandBroker.Register("loglevel", 1, MBackend.CommandLogLevel);
            MBackend.Console.CommandBroker.Register("version", 0, MBackend.CommandVersion);
            MBackend.Console.CommandBroker.Register("vars", 0, MBackend.CommandListVariables);
            MBackend.Console.CommandBroker.Register("get", 1, MBackend.CommandGet);
            MBackend.Console.CommandBroker.Register("set", 2, MBackend.CommandSet);
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MBackend.Logger.Log(e.ExceptionObject.ToString(), LogLevel.Error);
            FileStream fs = new FileStream("./crashdump.log", FileMode.OpenOrCreate | FileMode.Truncate);
            MBackend.Logger.WriteLog(fs); // TODO: Remove magic constant. Not into a constants class, but into settings! E.g. Settings.GetValue("crashdump").
        }
    }
}