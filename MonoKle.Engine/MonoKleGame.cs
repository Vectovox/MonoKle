using Microsoft.Xna.Framework;
using MonoKle.Asset;
using MonoKle.Console;
using MonoKle.Graphics;
using MonoKle.Input.Gamepad;
using MonoKle.Input.Keyboard;
using MonoKle.Input.Mouse;
using MonoKle.Input.Touch;
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
    public class MonoKleGame : Game
    {
        private static GameConsole console;
        private static GamePadHub gamepad;
        private static bool initializing;
        private static Keyboard keyboard;
        private static Mouse mouse;
        private static readonly MonoKleSettings settings = new MonoKleSettings();
        private static StateSystem stateSystem;
        private static TouchScreen touchScreen = new TouchScreen();

        /// <summary>
        /// Gets the game console, printing logs and accepting input.
        /// </summary>
        public static IGameConsole Console => MonoKleGame.console;

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
        /// Gets the hub for gamepads.
        /// </summary>
        public static IGamePadHub GamePad => MonoKleGame.gamepad;

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
        public static IKeyboard Keyboard => MonoKleGame.keyboard;

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
        public static IMouse Mouse => MonoKleGame.mouse;

        /// <summary>
        /// Gets the touch screen.
        /// </summary>
        public static ITouchScreen TouchScreen => touchScreen;

        /// <summary>
        /// Gets the state system, which keeps track of the states and switches between them.
        /// </summary>
        public static IStateSystem StateSystem => MonoKleGame.stateSystem;

        /// <summary>
        /// Gets the running game instance.
        /// </summary>
        public static MonoKleGame GameInstance { get; private set; }

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
        public static MonoKleSettings Settings => MonoKleGame.settings;

        /// <summary>
        /// Initializes the MonoKle backend, returning a runnable game instance.
        /// </summary>
        /// <param name="resolution">The initial display resolution.</param>
        /// <param name="fullscreen">The initial fullscreen setting.</param>
        public static MonoKleGame Create(bool fullscreen)
        {
            MonoKleGame.initializing = true;
            
            // Create the singleton instance
            MonoKleGame.GameInstance = new MonoKleGame();

            // Graphics device has to be created immediately but cannot be used before LoadContent
            MonoKleGame.GraphicsManager = new GraphicsManager(new GraphicsDeviceManager(GameInstance));
            MonoKleGame.GraphicsManager.ResolutionChanged += ResolutionChanged;
            MonoKleGame.GraphicsManager.IsFullscreen = fullscreen;

            MonoKleGame.settings.GamePadEnabled = true;
            MonoKleGame.settings.KeyboardEnabled = true;
            MonoKleGame.settings.MouseEnabled = true;

            // Set logger and enabled crashdumps
            MonoKleGame.Logger = Logger.Global;
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            MonoKleGame.InitializeVariables();
            MonoKleGame.gamepad = new GamePadHub();
            MonoKleGame.keyboard = new Keyboard();
            MonoKleGame.mouse = new Mouse();
            MonoKleGame.stateSystem = new StateSystem();

            return GameInstance;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            MonoKleGame.InitializeTextureStorage();
            MonoKleGame.InitializeFontStorage();
            MonoKleGame.EffectStorage = new EffectStorage(GraphicsManager.GraphicsDevice);
            MonoKleGame.InitializeConsole();
            Console.CommandBroker.RegisterCallingAssembly();
            MonoKleGame.BindSettings();

            mouse.VirtualRegion = new MRectangleInt(GraphicsManager.Resolution);

            // Done initializing
            console.WriteLine("MonoKle Engine initialized!", MonoKleGame.Console.CommandTextColour);
            console.WriteLine("Running version: " + Assembly.GetAssembly(typeof(MonoKleGame)).GetName().Version, MonoKleGame.Console.CommandTextColour);
            MonoKleGame.initializing = false;
        }

        protected override void Draw(GameTime time)
        {
            if (MonoKleGame.initializing == false)
            {
                var deltaTime = time.ElapsedGameTime;

                MonoKleGame.GraphicsManager.GraphicsDevice.Clear(Color.CornflowerBlue);
                MonoKleGame.stateSystem.Draw(deltaTime);

                if (MonoKleGame.settings.ConsoleEnabled)
                {
                    MonoKleGame.console.Draw(deltaTime);
                }
            }
        }

        protected override void Update(GameTime time)
        {
            if (MonoKleGame.initializing == false)
            {
                var deltaTime = time.ElapsedGameTime;
                MonoKleGame.IsRunningSlowly = time.IsRunningSlowly;
                MonoKleGame.TotalGameTime = time.TotalGameTime;

                if (MonoKleGame.settings.GamePadEnabled)
                {
                    MonoKleGame.gamepad.Update(deltaTime);
                }

                if (MonoKleGame.settings.KeyboardEnabled)
                {
                    MonoKleGame.keyboard.Update(deltaTime);
                }

                if (MonoKleGame.settings.MouseEnabled)
                {
                    MonoKleGame.mouse.Update(deltaTime);
                }

                if (MonoKleGame.settings.TouchEnabled)
                {
                    touchScreen.Update(deltaTime);
                }

                GraphicsManager.Update(Window.ClientBounds.Size);

                if (MonoKleGame.Console.IsOpen == false)
                {
                    MonoKleGame.stateSystem.Update(deltaTime);
                }

                if (MonoKleGame.settings.ConsoleEnabled)
                {
                    MonoKleGame.console.Update(deltaTime);
                }
            }
        }

        private static void BindSettings()
        {
            MonoKleGame.Variables.Variables.BindProperties(MonoKleGame.Logger);
            MonoKleGame.Variables.Variables.BindProperties(MonoKleGame.GraphicsManager);
            MonoKleGame.Variables.Variables.BindProperties(MonoKleGame.Console);
            MonoKleGame.Variables.Variables.BindProperties(MonoKleGame.Settings);
            MonoKleGame.Variables.Variables.BindProperties(MonoKleGame.Mouse);
        }

        private static void InitializeConsole()
        {
            MonoKleGame.console = new GameConsole(new Rectangle(0, 0, GraphicsManager.Resolution.X, GraphicsManager.Resolution.Y / 3),
                MonoKleGame.GraphicsManager.GraphicsDevice,
                MonoKleGame.keyboard,
                MonoKleGame.TextureStorage.White,
                MonoKleGame.FontStorage.DefaultValue,
                MonoKleGame.Logger);
            MonoKleGame.Console.ToggleKey = Microsoft.Xna.Framework.Input.Keys.F1;
            MonoKleGame.Console.TextFont = MonoKleGame.FontStorage.DefaultValue;
        }

        private static void InitializeFontStorage()
        {
            MonoKleGame.FontStorage = new FontStorage(GraphicsManager.GraphicsDevice);
            using var ms = new MemoryStream(Resources.FontResources.DefaultFont);
            MonoKleGame.FontStorage.Load(ms, "default");
            MonoKleGame.FontStorage.DefaultValue = MonoKleGame.FontStorage.GetAsset("default");
        }

        private static void InitializeTextureStorage() => MonoKleGame.TextureStorage = new TextureStorage(GraphicsManager.GraphicsDevice);

        private static void InitializeVariables()
        {
            MonoKleGame.Variables = new VariableStorage(MonoKleGame.Logger);
            MonoKleGame.Variables.LoadDefaultVariables();
        }

        private static void ResolutionChanged(object sender, ResolutionChangedEventArgs e)
        {
            if (MonoKleGame.Console != null)
            {
                MonoKleGame.Console.Area = new Rectangle(0, 0, MonoKleGame.GraphicsManager.ResolutionWidth, MonoKleGame.GraphicsManager.ResolutionHeight / 3);
            }
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MonoKleGame.Logger.Log(e.ExceptionObject.ToString(), LogLevel.Error);
            var fs = new FileStream("./crashdump.log", FileMode.OpenOrCreate | FileMode.Truncate);
            MonoKleGame.Logger.WriteLog(fs); // TODO: Remove magic constant. Not into a constants class, but into settings! E.g. Settings.GetValue("crashdump").
        }
    }
}
