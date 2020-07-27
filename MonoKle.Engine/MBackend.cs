using Microsoft.Xna.Framework;
using MonoKle.Asset.Effect;
using MonoKle.Asset.Font;
using MonoKle.Asset.Texture;
using MonoKle.Console;
using MonoKle.Graphics;
using MonoKle.Input.Gamepad;
using MonoKle.Input.Keyboard;
using MonoKle.Input.Mouse;
using MonoKle.Logging;
using MonoKle.State;
using System;
using System.IO;
using System.Reflection;

namespace MonoKle.Engine
{
    /// <summary>
    /// Backend for the MonoKle engine. Provides global access to all MonoKle systems.
    /// </summary>
    public static class MBackend
    {
        private static GameConsole console;
        private static GamePadHub gamepad;
        private static bool initializing;
        private static Keyboard keyboard;
        private static Mouse mouse;
        private static readonly MonoKleSettings settings = new MonoKleSettings();
        private static StateSystem stateSystem;

        /// <summary>
        /// Gets the game console, printing logs and accepting input.
        /// </summary>
        public static IGameConsole Console => MBackend.console;

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
        /// Gets the hub for gamepads.
        /// </summary>
        public static IGamePadHub GamePad => MBackend.gamepad;

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
        public static IKeyboard Keyboard => MBackend.keyboard;

        /// <summary>
        /// Gets the logging utility, same as <see cref="Logger.Global"/>.
        /// </summary>
        public static Logger Logger
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current mouse.
        /// </summary>
        public static IMouse Mouse => MBackend.mouse;

        /// <summary>
        /// Gets the state system, which keeps track of the states and switches between them.
        /// </summary>
        public static IStateSystem StateSystem => MBackend.stateSystem;

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
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public static MonoKleSettings Settings => MBackend.settings;

        /// <summary>
        /// Initializes the MonoKle backend, returning a runnable game instance.
        /// </summary>
        /// <returns></returns>
        public static MGame Initialize() => MBackend.Initialize(new MPoint2(640, 400));

        /// <summary>
        /// Initializes the MonoKle backend, returning a runnable game instance.
        /// </summary>
        /// <param name="resolution">The resolution.</param>
        /// <returns></returns>
        public static MGame Initialize(MPoint2 resolution) => MBackend.Initialize(resolution, false);

        /// <summary>
        /// Initializes the MonoKle backend, returning a runnable game instance.
        /// </summary>
        /// <param name="resolution">The initial display resolution.</param>
        /// <param name="enableConsole">Enables console.</param>
        /// <returns>Runnable <see cref="MGame"/>.</returns>
        public static MGame Initialize(MPoint2 resolution, bool enableConsole)
        {
            MBackend.initializing = true;

            MBackend.settings.GamePadEnabled = true;
            MBackend.settings.KeyboardEnabled = true;
            MBackend.settings.MouseEnabled = true;
            MBackend.settings.ConsoleEnabled = enableConsole;

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            MBackend.Logger = Logger.Global;
            MBackend.InitializeVariables();

            MBackend.GameInstance = new MGame();
            MBackend.GraphicsManager = new GraphicsManager(new GraphicsDeviceManager(MBackend.GameInstance));
            MBackend.GraphicsManager.ResolutionChanged += ResolutionChanged;
            MBackend.gamepad = new GamePadHub();
            MBackend.keyboard = new Keyboard();
            MBackend.mouse = new Mouse();
            MBackend.stateSystem = new StateSystem();

            MBackend.GameInstance.RunOneFrame();
            MBackend.GraphicsManager.Resolution = resolution;

            MBackend.InitializeTextureStorage();
            MBackend.InitializeFontStorage();
            MBackend.EffectStorage = new EffectStorage(GraphicsManager.GraphicsDevice);
            MBackend.InitializeConsole();

            Console.CommandBroker.RegisterCallingAssembly();
            MBackend.BindSettings();

            mouse.VirtualRegion = new MRectangleInt(GraphicsManager.Resolution);

            console.WriteLine("MonoKle Engine initialized!", MBackend.Console.CommandTextColour);
            console.WriteLine("Running version: " + Assembly.GetAssembly(typeof(MBackend)).GetName().Version, MBackend.Console.CommandTextColour);
            MBackend.initializing = false;

            return MBackend.GameInstance;
        }

        internal static void Draw(GameTime time)
        {
            if (MBackend.initializing == false)
            {
                var deltaTime = time.ElapsedGameTime;

                MBackend.GraphicsManager.GraphicsDevice.Clear(Color.CornflowerBlue);
                MBackend.stateSystem.Draw(deltaTime);

                if (MBackend.settings.ConsoleEnabled)
                {
                    MBackend.console.Draw(deltaTime);
                }
            }
        }

        internal static void Update(GameTime time)
        {
            if (MBackend.initializing == false)
            {
                var deltaTime = time.ElapsedGameTime;
                MBackend.IsRunningSlowly = time.IsRunningSlowly;
                MBackend.TotalGameTime = time.TotalGameTime;

                if (MBackend.settings.GamePadEnabled)
                {
                    MBackend.gamepad.Update(deltaTime);
                }

                if (MBackend.settings.KeyboardEnabled)
                {
                    MBackend.keyboard.Update(deltaTime);
                }

                if (MBackend.settings.MouseEnabled)
                {
                    MBackend.mouse.Update(deltaTime);
                }

                if (MBackend.settings.ConsoleEnabled == false || MBackend.Console.IsOpen == false)
                {
                    MBackend.stateSystem.Update(deltaTime);
                }

                if (MBackend.settings.ConsoleEnabled)
                {
                    MBackend.console.Update(deltaTime);
                }
            }
        }

        private static void BindSettings()
        {
            MBackend.Variables.Variables.BindProperties(MBackend.Logger);
            MBackend.Variables.Variables.BindProperties(MBackend.GraphicsManager);
            MBackend.Variables.Variables.BindProperties(MBackend.Console);
            MBackend.Variables.Variables.BindProperties(MBackend.Settings);
            MBackend.Variables.Variables.BindProperties(MBackend.Mouse);
        }

        private static void InitializeConsole()
        {
            MBackend.console = new GameConsole(new Rectangle(0, 0, GraphicsManager.Resolution.X, GraphicsManager.Resolution.Y / 3),
                MBackend.GraphicsManager.GraphicsDevice,
                MBackend.keyboard,
                MBackend.TextureStorage.White,
                MBackend.FontStorage.DefaultValue,
                MBackend.Logger);
            MBackend.Console.ToggleKey = Microsoft.Xna.Framework.Input.Keys.F1;
            MBackend.Console.TextFont = MBackend.FontStorage.DefaultValue;
        }

        private static void InitializeFontStorage()
        {
            MBackend.FontStorage = new FontStorage(GraphicsManager.GraphicsDevice);
            using var ms = new MemoryStream(Resources.FontResources.DefaultFont);
            MBackend.FontStorage.LoadStream(ms, "default");
            MBackend.FontStorage.DefaultValue = MBackend.FontStorage.GetAsset("default");
        }

        private static void InitializeTextureStorage() => MBackend.TextureStorage = new TextureStorage(GraphicsManager.GraphicsDevice);

        private static void InitializeVariables()
        {
            MBackend.Variables = new VariableStorage(MBackend.Logger);
            MBackend.Variables.LoadDefaultVariables();
        }

        private static void ResolutionChanged(object sender, ResolutionChangedEventArgs e)
        {
            if (MBackend.Console != null)
            {
                MBackend.Console.Area = new Rectangle(0, 0, MBackend.GraphicsManager.ResolutionWidth, MBackend.GraphicsManager.ResolutionHeight / 3);
            }
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MBackend.Logger.Log(e.ExceptionObject.ToString(), LogLevel.Error);
            var fs = new FileStream("./crashdump.log", FileMode.OpenOrCreate | FileMode.Truncate);
            MBackend.Logger.WriteLog(fs); // TODO: Remove magic constant. Not into a constants class, but into settings! E.g. Settings.GetValue("crashdump").
        }
    }
}
