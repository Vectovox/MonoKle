using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
using MonoKle.State;
using System;
using System.IO;
using System.Linq;

namespace MonoKle.Engine
{
    /// <summary>
    /// Backend for the MonoKle engine. Provides global access to all MonoKle systems.
    /// </summary>
    public class MGame : Game
    {
        private static string _title = string.Empty;
        private static readonly Callbacker _uiThreadCallbacker = new();
        private static PerformanceWidget _performanceWidget;
        private static bool _initializing = true;
        private static readonly GameConsoleLogData _logData = new();

        private static GameConsole _console;
        private static readonly GamePadHub _gamepad = new();
        private static readonly Keyboard _keyboard = new();
        private static readonly Mouse _mouse = new();
        private static readonly TouchScreen _touchScreen = new(_mouse);
        private static StateSystem _stateSystem;

        public MGame() : this(new ServiceCollection())
        {
        }

        public MGame(ServiceCollection serviceCollection) : base()
        {
            Services = serviceCollection
                .AddSingleton<StateSystem>()
                .AddSingleton<SongStorage>()
                .AddSingleton<SoundEffectStorage>()
                .AddLogging((loggingBuilder) => loggingBuilder
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddMonoKleConsoleLogger(_logData))
                .BuildServiceProvider();
        }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        public new ServiceProvider Services { get; }

        /// <summary>
        /// If true, crashes the engine for testing purposes.
        /// </summary>
        public bool ShouldCrash { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            Window.Title = _title;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Initialize storages
            Asset.Texture = new TextureStorage(GraphicsDevice, Services.GetService<ILogger<TextureStorage>>());
            Asset.SoundEffect = Services.GetService<SoundEffectStorage>();
            Asset.Effect = new EffectStorage(GraphicsDevice, Services.GetService<ILogger<EffectStorage>>());
            Asset.Song = Services.GetService<SongStorage>();
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
            _logData.AddLine($"{ConfigData.Product} © {ConfigData.ProductYear} {ConfigData.Company}", Console.CommandTextColour);
            _console.CommandBroker.Call(Commands.VersionCommand.Name);
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
                if (ShouldCrash)
                {
                    throw new Exception("We have crashed!");
                }

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

        /// <summary>
        /// Gets the game console, printing logs and accepting input.
        /// </summary>
        public static IGameConsole Console => _console;

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

        /// <summary>
        /// Gets the keyboard input.
        /// </summary>
        public static IKeyboard Keyboard => _keyboard;

        /// <summary>
        /// Gets the current mouse input.
        /// </summary>
        public static IMouse Mouse => _mouse;

        /// <summary>
        /// Gets the touch screen input.
        /// </summary>
        public static ITouchScreen TouchScreen => _touchScreen;

        /// <summary>
        /// Gets the most recently activated input mode.
        /// </summary>
        public static InputMode InputMode { get; private set; }

        /// <summary>
        /// Gets the generic logger.
        /// </summary>
        public static ILogger Logger { get; private set; }

        /// <summary>
        /// Gets the state system, which keeps track of the states and switches between them.
        /// </summary>
        public static IStateSystem StateSystem => _stateSystem;

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
        /// <param name="graphicsMode">The initial graphics mode setting.</param>
        /// <param name="arguments">Variable assignment strings. E.g. 'mySettingEnabled = false'.</param>
        public static MGame Create(GraphicsMode graphicsMode, string[] arguments) =>
            Create(new MGame(), graphicsMode, arguments);

        /// <summary>
        /// Initializes the MonoKle backend, returning a runnable game instance.
        /// </summary>
        /// <param name="graphicsMode">The initial graphics mode setting.</param>
        /// <param name="arguments">Variable assignment strings. E.g. 'mySettingEnabled = false'.</param>
        /// <param name="serviceCollection">Service collection to use for dependency injection.</param>
        public static MGame Create(GraphicsMode graphicsMode, string[] arguments, ServiceCollection serviceCollection) =>
            Create(new MGame(serviceCollection), graphicsMode, arguments);

        /// <summary>
        /// Initializes the MonoKle backend with the given game instance.
        /// </summary>
        /// <param name="gameInstance">The instance to use for the MonoKle backend.</param>
        /// <param name="graphicsMode">The initial graphics mode setting.</param>
        /// <param name="arguments">Variable assignment strings. E.g. 'mySettingEnabled = false'.</param>
        /// <param name="title">Title of the game window. Auto-generated if null.</param>
        public static MGame Create(MGame gameInstance, GraphicsMode graphicsMode, string[] arguments, string title = null)
        {
            GameInstance = gameInstance;
            _title = title ?? $"{ConfigData.Product} {ConfigData.ProductVersion}";

            // Enable error logging first
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            // Make sure we can create error logs (and other game data)
            GameDataStorage.CreateFolders();

            // Set up systems
            Logger = gameInstance.Services.GetService<ILogger<MGame>>();
            _stateSystem = gameInstance.Services.GetService<StateSystem>();

            // Graphics device has to be created immediately but cannot be used before LoadContent
            GraphicsManager = new GraphicsManager(GameInstance);
            GraphicsManager.BackBufferChanged += ResolutionChanged;
            GraphicsManager.GraphicsMode = graphicsMode;

            // Default settings
            Settings.GamePadEnabled = true;
            Settings.KeyboardEnabled = true;
            Settings.MouseEnabled = true;
            Settings.TouchEnabled = true;

            // Set game variables from program arguments
            InitializeVariables(arguments);

            return GameInstance;
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
            Variables.System.BindProperties(Logger);
            Variables.System.BindProperties(GraphicsManager);
            Variables.System.BindProperties(Console);
            Variables.System.BindProperties(Settings);
            Variables.System.BindProperties(Mouse);
            Variables.System.BindProperties(TouchScreen);
            Variables.System.BindProperties(_performanceWidget);
            Variables.System.BindProperties(Mixer);
            Variables.System.BindProperties(typeof(GameDataStorage));
            Variables.System.BindProperties(_logData, true);
        }

        private static void InitializeConsole()
        {
            _console = new GameConsole(GameInstance.Window,
                new Rectangle(0, 0, GraphicsManager.Resolution.X, GraphicsManager.Resolution.Y / 3),
                GraphicsManager.GraphicsDevice,
                _keyboard,
                _mouse,
                Asset.Texture.White,
                Asset.Font.Default,
                _logData);
            Console.ToggleKey = Microsoft.Xna.Framework.Input.Keys.F1;
            Console.TextFont = Asset.Font.Default;
        }

        private static void InitializeFontStorage()
        {
            Asset.Font = new FontStorage(GraphicsManager.GraphicsDevice, GameInstance.Services.GetService<ILogger<FontStorage>>());
            using var ms = new MemoryStream(Resources.FontResources.DefaultFont);
            Asset.Font.Load(ms, "default");
            Asset.Font.Default = Asset.Font["default"];
            Asset.Font.Default.ColorTagEnabled = false;
        }

        private static void InitializeVariables(string[] arguments)
        {
            Variables = new VariableStorage(Logger);
            Variables.LoadSettings();
            foreach (var line in arguments)
            {
                Variables.Populator.LoadText(line);
            }
        }

        private static void ResolutionChanged(object sender, ResolutionChangedEventArgs e)
        {
            if (Console != null)
            {
                Console.Area = new Rectangle(0, 0, e.NewScreenSize.X, e.NewScreenSize.Y / 3);
            }
        }

        /// <summary>
        /// Print logs before we crash. App will still write stack-trace to stderr if this code fails.
        /// </summary>
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs unhandledException)
        {
            // Add the exception to the logs
            try
            {
                Logger.LogError(unhandledException.ExceptionObject.ToString());   
            }
            catch { }

            var logs = _logData.Entries.Reverse().ToList();

            // Write logs to stderr
            try
            {
                foreach (var entry in logs)
                {
                    var line = entry.Text;
                    System.Console.Error.WriteLine(line);
                }
            }
            catch { }

            // Write logs to error file
            using var lineWriter = new StreamWriter(GameDataStorage.GetLogFile("crash.log").Open(FileMode.Append));
            lineWriter.WriteLine($"=========== {DateTime.Now} ===========");
            foreach (var entry in _logData.Entries.Reverse())
            {
                var line = entry.Text;
                lineWriter.WriteLine(line);
            }
            lineWriter.Flush();
            lineWriter.Close();
        }
    }
}
