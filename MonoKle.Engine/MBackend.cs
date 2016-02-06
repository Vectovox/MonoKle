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
    using Attributes;
    using Resources;
    using Script;
    using State;
    using System;
    using System.IO;
    using System.Reflection;
    using Variable;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Core.Geometry;

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
        /// Gets or sets wether the console is enabled.
        /// </summary>
        [PropertyVariableAttribute("console_enabled")]
        public static bool ConsoleEnabled { get; set; }

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
        /// <returns></returns>
        public static MGame Initialize()
        {
            return MBackend.Initialize(new MPoint2(640, 400));
        }

        /// <summary>
        /// Initializes the MonoKle backend, returning a runnable game instance.
        /// </summary>
        /// <param name="resolution">The resolution.</param>
        /// <returns></returns>
        public static MGame Initialize(MPoint2 resolution)
        {
            return MBackend.Initialize(resolution, false);
        }

        /// <summary>
        /// Initializes the MonoKle backend, returning a runnable game instance.
        /// </summary>
        /// <param name="resolution">The initial display resolution.</param>
        /// <param name="enableConsole">Enables console.</param>
        /// <returns>Runnable <see cref="MGame"/>.</returns>
        public static MGame Initialize(MPoint2 resolution, bool enableConsole)
        {
            MBackend.initializing = true;
            MBackend.ConsoleEnabled = enableConsole;

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            MBackend.Logger = Logger.Global;
            MBackend.InitializeVariables();

            MBackend.GameInstance = new MGame();
            MBackend.GraphicsManager = new GraphicsManager(new GraphicsDeviceManager(MBackend.GameInstance));
            MBackend.GraphicsManager.ResolutionChanged += ResolutionChanged;
            MBackend.gamepad = new GamePadInput();
            MBackend.keyboard = new KeyboardInput();
            MBackend.mouse = new MouseInput();
            MBackend.stateSystem = new StateSystem();

            MBackend.GameInstance.RunOneFrame();
            MBackend.GraphicsManager.Resolution = resolution;

            MBackend.InitializeTextureStorage();
            MBackend.InitializeFontStorage();
            MBackend.EffectStorage = new EffectStorage(GraphicsManager.GraphicsDevice);
            MBackend.InitializeConsole();

            MBackend.RegisterConsoleCommands();
            MBackend.BindSettings();

            mouse.ScreenSize = GraphicsManager.Resolution;

            console.WriteLine("MonoKle Engine initialized!", Color.LightGreen);
            console.WriteLine("Running version: " + Assembly.GetAssembly(typeof(MBackend)).GetName().Version, Color.LightGreen);
            MBackend.initializing = false;

            return MBackend.GameInstance;
        }

        private static void ResolutionChanged(object sender, Graphics.Event.ResolutionChangedEventArgs e)
        {
            if(MBackend.Console != null)
            {
                MBackend.Console.Area = new Rectangle(0, 0, MBackend.GraphicsManager.ResolutionWidth, MBackend.GraphicsManager.ResolutionHeight / 3);
            }
        }

        internal static void Draw(GameTime time)
        {
            if (MBackend.initializing == false)
            {
                double seconds = time.ElapsedGameTime.TotalSeconds;

                MBackend.GraphicsManager.GraphicsDevice.Clear(Color.CornflowerBlue);
                MBackend.stateSystem.Draw(seconds);
                
                if (MBackend.ConsoleEnabled)
                {
                    MBackend.console.Draw(seconds);
                }
            }
        }

        internal static void Update(GameTime time)
        {
            if (MBackend.initializing == false)
            {
                double seconds = time.ElapsedGameTime.TotalSeconds;
                MBackend.IsRunningSlowly = time.IsRunningSlowly;
                MBackend.TotalGameTime = time.TotalGameTime;
                MBackend.gamepad.Update(seconds);
                MBackend.keyboard.Update(seconds);
                MBackend.mouse.Update(seconds);

                if (MBackend.ConsoleEnabled == false || MBackend.Console.IsOpen == false)
                {
                    MBackend.stateSystem.Update(seconds);
                }

                if (MBackend.ConsoleEnabled)
                {
                    MBackend.console.Update(seconds);
                }
            }
        }

        private static void BindSettings()
        {
            MBackend.Variables.Variables.BindProperties(MBackend.Logger);
            MBackend.Variables.Variables.BindProperties(MBackend.GraphicsManager);
            MBackend.Variables.Variables.BindProperties(MBackend.Console);
            MBackend.Variables.Variables.Bind(new PropertyVariable(nameof(MBackend.ConsoleEnabled), typeof(MBackend)), "c_enabled");
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
            MBackend.Console.WriteLine("Listing variables:");
            List<string> identifiers = MBackend.Variables.Variables.Identifiers.ToList();
            identifiers.Sort();
            foreach (string s in identifiers)
            {
                MBackend.Console.WriteLine("  " + s);
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
            MBackend.console = new GameConsole(new Rectangle(0, 0, GraphicsManager.Resolution.X, GraphicsManager.Resolution.Y / 3),
                MBackend.GraphicsManager.GraphicsDevice,
                MBackend.keyboard,
                MBackend.TextureStorage.White);
            MBackend.Console.ToggleKey = Microsoft.Xna.Framework.Input.Keys.F1;
            MBackend.Console.TextFont = MBackend.FontStorage.DefaultValue;
        }

        private static void InitializeFontStorage()
        {
            MBackend.FontStorage = new FontStorage(GraphicsManager.GraphicsDevice);
            using (MemoryStream ms = new MemoryStream(Resources.FontResources.DefaultFont))
            {
                MBackend.FontStorage.LoadStream(ms, "default");
                MBackend.FontStorage.DefaultValue = MBackend.FontStorage.GetAsset("default");
            }
        }

        private static void InitializeTextureStorage()
        {
            MBackend.TextureStorage = new TextureStorage(GraphicsManager.GraphicsDevice,
                GraphicsHelper.ImageToTexture2D(GraphicsManager.GraphicsDevice, TextureResources.DefaultTexture),
                GraphicsHelper.ImageToTexture2D(GraphicsManager.GraphicsDevice, TextureResources.WhiteTexture)
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