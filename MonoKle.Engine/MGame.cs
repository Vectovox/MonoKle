using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Asset;
using MonoKle.Console;
using MonoKle.Graphics;
using MonoKle.Input;
using MonoKle.Input.Gamepad;
using MonoKle.Input.Keyboard;
using MonoKle.Input.Mouse;
using MonoKle.Input.Touch;
using MonoKle.Logging;
using MonoKle.State;
using System;
using System.IO;

namespace MonoKle.Engine
{
    /// <summary>
    /// Backend for the MonoKle engine. Provides global access to all MonoKle systems.
    /// </summary>
    public class MGame : Game
    {
        private static string _title = string.Empty;
        private static readonly Callbacker _uiThreadCallbacker = new Callbacker();
        private static PerformanceWidget _performanceWidget;
        private static bool _initializing = true;

        /// <summary>
        /// Gets the game console, printing logs and accepting input.
        /// </summary>
        public static IGameConsole Console => _console;
        private static GameConsole _console;

        /// <summary>
        /// Gets the container of asset storages.
        /// </summary>
        public static AssetStorageContainer Asset { get; } = new AssetStorageContainer();

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
        public static IGamePadHub GamePad => _gamepad;
        private static readonly GamePadHub _gamepad = new GamePadHub();

        /// <summary>
        /// Gets the keyboard input.
        /// </summary>
        public static IKeyboard Keyboard => _keyboard;
        private static readonly Keyboard _keyboard = new Keyboard();

        /// <summary>
        /// Gets the current mouse input.
        /// </summary>
        public static IMouse Mouse => _mouse;
        private static readonly Mouse _mouse = new Mouse();

        /// <summary>
        /// Gets the touch screen input.
        /// </summary>
        public static ITouchScreen TouchScreen => _touchScreen;
        private static readonly TouchScreen _touchScreen = new TouchScreen(_mouse);

        /// <summary>
        /// Gets the most recently activated input mode.
        /// </summary>
        public static InputMode InputMode { get; private set; }

        /// <summary>
        /// Gets the logging utility, same as <see cref="Logger.Global"/>.
        /// </summary>
        public static Logger Logger { get; private set; } = Logger.Global;

        /// <summary>
        /// Gets the state system, which keeps track of the states and switches between them.
        /// </summary>
        public static IStateSystem StateSystem => _stateSystem;
        private static readonly StateSystem _stateSystem = new StateSystem(Logger);

        /// <summary>
        /// Gets the running game instance.
        /// </summary>
        public static MGame GameInstance { get; private set; }

        /// <summary>
        /// Gets or sets the global game settings.
        /// </summary>
        public static MonoKleSettings Settings { get; } = new MonoKleSettings();

        /// <summary>
        /// Gets the total time spent in the game.
        /// </summary>
        public static TimeSpan TotalGameTime { get; private set; }

        /// <summary>
        /// Gets the variable storage, handling all game variables, such as settings.
        /// </summary>
        public static VariableStorage Variables { get; private set; }

        /// <summary>
        /// Gets the sound mixer.
        /// </summary>
        public static Mixer Mixer { get; } = new Mixer();

        /// <summary>
        /// Callback for when the input mode changed. Supplies the previous mode and the new mode as input parameters.
        /// </summary>
        public static event Action<InputMode, InputMode> InputModeChanged;

        /// <summary>
        /// Event invoked when a gamepad has been disconnected.
        /// </summary>
        public static event Action GamepadDisconnected;

        /// <summary>
        /// Initializes the MonoKle backend, returning a runnable game instance.
        /// </summary>
        /// <param name="title">Title of the game window.</param>
        /// <param name="graphicsMode">The initial graphics mode setting.</param>
        /// <param name="arguments">Variable assignment strings. E.g. 'mySettingEnabled = false'.</param>
        public static MGame Create(string title, GraphicsMode graphicsMode, string[] arguments) => Create(new MGame(), title, graphicsMode, arguments);

        /// <summary>
        /// Initializes the MonoKle backend with the given game instance.
        /// </summary>
        /// <param name="gameInstance">The instance to use for the MonoKle backend.</param>
        /// <param name="title">Title of the game window.</param>
        /// <param name="graphicsMode">The initial graphics mode setting.</param>
        /// <param name="arguments">Variable assignment strings. E.g. 'mySettingEnabled = false'.</param>
        public static MGame Create(MGame gameInstance, string title, GraphicsMode graphicsMode, string[] arguments)
        {
            GameInstance = gameInstance;

            _title = title;

            // Graphics device has to be created immediately but cannot be used before LoadContent
            GraphicsManager = new GraphicsManager(GameInstance);
            GraphicsManager.BackBufferChanged += ResolutionChanged;
            GraphicsManager.GraphicsMode = graphicsMode;

            Settings.GamePadEnabled = true;
            Settings.KeyboardEnabled = true;
            Settings.MouseEnabled = true;
            Settings.TouchEnabled = true;
            Settings.CrashDumpPath = "./crashdump.log";

            // Enable crashdumps
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            InitializeVariables(arguments);

            return GameInstance;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Window.Title = _title;
        }

        /// <summary>
        /// Runs the given action on the UI thread. Execution will occur at some future frame. Multiple calls are executed in a FIFO order. 
        /// </summary>
        /// <param name="action">The action to run.</param>
        /// <param name="wait">If true, waits blockingly until the action has been executed.</param>
        public static void RunOnUIThread(Action action, bool wait = true)
        {
            if (wait)
            {
                _uiThreadCallbacker.AddWait(action);
            }
            else
            {
                _uiThreadCallbacker.Add(action);
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Initialize storages
            Asset.Texture = new TextureStorage(GraphicsDevice, Logger);
            Asset.SoundEffect = new SoundEffectStorage(Logger);
            Asset.Effect = new EffectStorage(GraphicsDevice, Logger);
            Asset.Song = new SongStorage(Logger);
            InitializeFontStorage();
            
            // Initialize other services
            InitializeConsole();
            _performanceWidget = new PerformanceWidget(GraphicsDevice, Asset.Font.Default, Asset.Texture.White);

            // Set up commands and settings
            Console.CommandBroker.RegisterCallingAssembly();
            BindSettings();

            // Set up virtual mouse
            _mouse.VirtualRegion = new MRectangleInt(GraphicsManager.Resolution);   // TODO: Virtual mouse seems to have low fps? And the region may not work properly?

            // Done initializing
            _console.WriteLine("MonoKle Engine initialized!", Console.CommandTextColour);
            _console.WriteLine($"Running version: {ThisAssembly.AssemblyInformationalVersion}", Console.CommandTextColour);
            _initializing = false;
        }

        protected override void Draw(GameTime time)
        {
            if (!_initializing)
            {
                _performanceWidget.BeginDraw();

                var deltaTime = time.ElapsedGameTime;

                _stateSystem.Draw(deltaTime);

                _performanceWidget.Draw();

                if (Settings.ConsoleEnabled)
                {
                    _console.Draw(deltaTime);
                }
                _performanceWidget.EndDraw();
            }
        }

        protected override void Update(GameTime time)
        {
            if (!_initializing)
            {
                _performanceWidget.BeginUpdate();

                _uiThreadCallbacker.CallOne();

                var deltaTime = time.ElapsedGameTime;
                IsRunningSlowly = time.IsRunningSlowly;
                TotalGameTime = time.TotalGameTime;

                if (Settings.GamePadEnabled && GameInstance.IsActive)
                {
                    _gamepad.Update(deltaTime);
                    if (_gamepad.WasActivated)
                    {
                        SetInputMode(InputMode.Gamepad);
                    }
                    if (_gamepad.AnyDisconnected)
                    {
                        GamepadDisconnected?.Invoke();
                    }
                }

                if (Settings.KeyboardEnabled && GameInstance.IsActive)
                {
                    _keyboard.Update(deltaTime);
                    if (_keyboard.WasActivated)
                    {
                        SetInputMode(InputMode.KeyboardMouse);
                    }
                }

                if (Settings.MouseEnabled && GameInstance.IsActive)
                {
                    _mouse.Update(deltaTime);
                    if (_mouse.WasActivated)
                    {
                        SetInputMode(InputMode.KeyboardMouse);
                    }
                }

                if (Settings.TouchEnabled && GameInstance.IsActive)
                {
                    _touchScreen.Update(deltaTime);
                }

                GraphicsManager.Update();

                if (Console.IsOpen == false)
                {
                    _stateSystem.Update(deltaTime);
                }

                if (Settings.ConsoleEnabled)
                {
                    _console.Update(deltaTime);
                }

                _performanceWidget.EndUpdate();
            }
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            GraphicsManager.Dispose();
        }

        private static void SetInputMode(InputMode mode)
        {
            var prevMode = InputMode;
            if (prevMode != mode)
            {
                InputMode = mode;
                InputModeChanged?.Invoke(prevMode, mode);
            }
        }

        private static void BindSettings()
        {
            Variables.Variables.BindProperties(Logger);
            Variables.Variables.BindProperties(GraphicsManager);
            Variables.Variables.BindProperties(Console);
            Variables.Variables.BindProperties(Settings);
            Variables.Variables.BindProperties(Mouse);
            Variables.Variables.BindProperties(TouchScreen);
            Variables.Variables.BindProperties(_performanceWidget);
            Variables.Variables.BindProperties(Mixer);
        }

        private static void InitializeConsole()
        {
            _console = new GameConsole(new Rectangle(0, 0, GraphicsManager.Resolution.X, GraphicsManager.Resolution.Y / 3),
                GraphicsManager.GraphicsDevice,
                _keyboard,
                _mouse,
                Asset.Texture.White,
                Asset.Font.Default,
                Logger);
            Console.ToggleKey = Microsoft.Xna.Framework.Input.Keys.F1;
            Console.TextFont = Asset.Font.Default;
        }

        private static void InitializeFontStorage()
        {
            Asset.Font = new FontStorage(GraphicsManager.GraphicsDevice, Logger);
            using var ms = new MemoryStream(Resources.FontResources.DefaultFont);
            Asset.Font.Load(ms, "default");
            Asset.Font.Default = Asset.Font["default"];
        }

        private static void InitializeVariables(string[] arguments)
        {
            Variables = new VariableStorage(Logger);
            Variables.LoadDefaultVariables();
            foreach (var line in arguments)
            {
                Variables.VariablePopulator.LoadText(line);
            }
        }

        private static void ResolutionChanged(object sender, ResolutionChangedEventArgs e)
        {
            if (Console != null)
            {
                Console.Area = new Rectangle(0, 0, e.NewScreenSize.X, e.NewScreenSize.Y / 3);
            }
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log(e.ExceptionObject.ToString(), LogLevel.Error);
            var fs = new FileStream(Settings.CrashDumpPath, FileMode.OpenOrCreate | FileMode.Truncate);
            Logger.WriteLog(fs);
        }
    }
}
