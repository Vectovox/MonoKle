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
        private static bool initializing = true;

        /// <summary>
        /// Gets the game console, printing logs and accepting input.
        /// </summary>
        public static IGameConsole Console => console;
        private static GameConsole console;

        /// <summary>
        /// Gets the effect storage, loading and providing effects.
        /// </summary>
        public static EffectStorage EffectStorage { get; private set; }

        /// <summary>
        /// Gets the font storage, loading and providing fonts.
        /// </summary>
        public static FontStorage FontStorage { get; private set; }

        /// <summary>
        /// Gets the graphics manager. This is in charge of screen settings (resolution, full-screen, etc.)
        /// and provides the <see cref="GraphicsDevice"/>.
        /// </summary>
        public static GraphicsManager GraphicsManager { get; private set; }

        /// <summary>
        /// Gets wether the game is running slowly or not.
        /// </summary>
        public static bool IsRunningSlowly { get; private set; }

        /// <summary>
        /// Gets the hub for gamepad input.
        /// </summary>
        public static IGamePadHub GamePad => gamepad;
        private static readonly GamePadHub gamepad = new GamePadHub();

        /// <summary>
        /// Gets the keyboard input.
        /// </summary>
        public static IKeyboard Keyboard => keyboard;
        private static readonly Keyboard keyboard = new Keyboard();

        /// <summary>
        /// Gets the touch screen input.
        /// </summary>
        public static ITouchScreen TouchScreen => touchScreen;
        private static readonly TouchScreen touchScreen = new TouchScreen();

        /// <summary>
        /// Gets the current mouse input.
        /// </summary>
        public static IMouse Mouse => mouse;
        private static readonly Mouse mouse = new Mouse();

        /// <summary>
        /// Gets the state system, which keeps track of the states and switches between them.
        /// </summary>
        public static IStateSystem StateSystem => stateSystem;
        private static readonly StateSystem stateSystem = new StateSystem();

        /// <summary>
        /// Gets the running game instance.
        /// </summary>
        public static MonoKleGame GameInstance { get; } = new MonoKleGame();

        /// <summary>
        /// Gets or sets the global game settings.
        /// </summary>
        public static MonoKleSettings Settings { get; } = new MonoKleSettings();

        /// <summary>
        /// Gets the logging utility, same as <see cref="Logger.Global"/>.
        /// </summary>
        public static Logger Logger { get; private set; }

        /// <summary>
        /// Gets the texture storage, loading and providing textures.
        /// </summary>
        public static TextureStorage TextureStorage { get; private set; }

        /// <summary>
        /// Gets the total time spent in the game.
        /// </summary>
        public static TimeSpan TotalGameTime { get; private set; }

        /// <summary>
        /// Gets the variable storage, handling all game variables, such as settings.
        /// </summary>
        /// <value>
        public static VariableStorage Variables { get; private set; }

        /// <summary>
        /// Initializes the MonoKle backend, returning a runnable game instance.
        /// </summary>
        /// <param name="resolution">The initial display resolution.</param>
        /// <param name="fullscreen">The initial fullscreen setting.</param>
        public static MonoKleGame Create(bool fullscreen)
        {
            // Graphics device has to be created immediately but cannot be used before LoadContent
            GraphicsManager = new GraphicsManager(new GraphicsDeviceManager(GameInstance));
            GraphicsManager.ResolutionChanged += ResolutionChanged;
            GraphicsManager.IsFullscreen = fullscreen;

            Settings.GamePadEnabled = true;
            Settings.KeyboardEnabled = true;
            Settings.MouseEnabled = true;
            Settings.TouchEnabled = true;

            // Set logger and enabled crashdumps
            Logger = Logger.Global;
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            InitializeVariables();

            return GameInstance;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            InitializeTextureStorage();
            InitializeFontStorage();
            EffectStorage = new EffectStorage(GraphicsManager.GraphicsDevice);
            InitializeConsole();
            Console.CommandBroker.RegisterCallingAssembly();
            BindSettings();

            mouse.VirtualRegion = new MRectangleInt(GraphicsManager.Resolution);

            // Done initializing
            console.WriteLine("MonoKle Engine initialized!", Console.CommandTextColour);
            console.WriteLine("Running version: " + Assembly.GetAssembly(typeof(MonoKleGame)).GetName().Version, Console.CommandTextColour);
            initializing = false;
        }

        protected override void Draw(GameTime time)
        {
            if (initializing == false)
            {
                var deltaTime = time.ElapsedGameTime;

                GraphicsManager.GraphicsDevice.Clear(Color.CornflowerBlue);
                stateSystem.Draw(deltaTime);

                if (Settings.ConsoleEnabled)
                {
                    console.Draw(deltaTime);
                }
            }
        }

        protected override void Update(GameTime time)
        {
            if (initializing == false)
            {
                var deltaTime = time.ElapsedGameTime;
                IsRunningSlowly = time.IsRunningSlowly;
                TotalGameTime = time.TotalGameTime;

                if (Settings.GamePadEnabled)
                {
                    gamepad.Update(deltaTime);
                }

                if (Settings.KeyboardEnabled)
                {
                    keyboard.Update(deltaTime);
                }

                if (Settings.MouseEnabled)
                {
                    mouse.Update(deltaTime);
                }

                if (Settings.TouchEnabled)
                {
                    touchScreen.Update(deltaTime);
                }

                GraphicsManager.Update(Window.ClientBounds.Size);

                if (Console.IsOpen == false)
                {
                    stateSystem.Update(deltaTime);
                }

                if (Settings.ConsoleEnabled)
                {
                    console.Update(deltaTime);
                }
            }
        }

        private static void BindSettings()
        {
            Variables.Variables.BindProperties(Logger);
            Variables.Variables.BindProperties(GraphicsManager);
            Variables.Variables.BindProperties(Console);
            Variables.Variables.BindProperties(Settings);
            Variables.Variables.BindProperties(Mouse);
        }

        private static void InitializeConsole()
        {
            console = new GameConsole(new Rectangle(0, 0, GraphicsManager.Resolution.X, GraphicsManager.Resolution.Y / 3),
                GraphicsManager.GraphicsDevice,
                keyboard,
                TextureStorage.White,
                FontStorage.DefaultValue,
                Logger);
            Console.ToggleKey = Microsoft.Xna.Framework.Input.Keys.F1;
            Console.TextFont = FontStorage.DefaultValue;
        }

        private static void InitializeFontStorage()
        {
            FontStorage = new FontStorage(GraphicsManager.GraphicsDevice);
            using var ms = new MemoryStream(Resources.FontResources.DefaultFont);
            FontStorage.Load(ms, "default");
            FontStorage.DefaultValue = FontStorage.GetAsset("default");
        }

        private static void InitializeTextureStorage() => TextureStorage = new TextureStorage(GraphicsManager.GraphicsDevice);

        private static void InitializeVariables()
        {
            Variables = new VariableStorage(Logger);
            Variables.LoadDefaultVariables();
        }

        private static void ResolutionChanged(object sender, ResolutionChangedEventArgs e)
        {
            if (Console != null)
            {
                Console.Area = new Rectangle(0, 0, GraphicsManager.ResolutionWidth, GraphicsManager.ResolutionHeight / 3);
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
