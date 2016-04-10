namespace MonoKle.Engine
{
    using Asset.Effect;
    using Asset.Font;
    using Asset.Texture;
    using Console;
    using Graphics;
    using Input.Mouse;
    using Logging;
    using Microsoft.Xna.Framework;
    using Input.Gamepad;
    using Resources;
    using State;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Input.Keyboard;
    using Scripting;
    using System.Text;

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
        private static MonoKleSettings settings = new MonoKleSettings();
        private static StateSystem stateSystem;

        /// <summary>
        /// Gets the game console, printing logs and accepting input.
        /// </summary>
        public static IGameConsole Console { get { return MBackend.console; } }

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
        public static IGamePadHub GamePad { get { return MBackend.gamepad; } }

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
        public static IKeyboard Keyboard { get { return MBackend.keyboard; } }

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
        public static IMouse Mouse
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

            MBackend.settings.GamePadEnabled = true;
            MBackend.settings.KeyboardEnabled = true;
            MBackend.settings.MouseEnabled = true;
            MBackend.settings.ConsoleEnabled = enableConsole;

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            MBackend.Logger = Logger.Global;
            MBackend.InitializeVariables();

            MBackend.ScriptEnvironment = new ScriptEnvironment();
            MBackend.ScriptEnvironment.ReferencedAssemblies.Add(typeof(MBackend).Assembly);
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

            MBackend.RegisterConsoleCommands();
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
                double seconds = time.ElapsedGameTime.TotalSeconds;

                MBackend.GraphicsManager.GraphicsDevice.Clear(Color.CornflowerBlue);
                MBackend.stateSystem.Draw(seconds);

                if (MBackend.settings.ConsoleEnabled)
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

                if (MBackend.settings.GamePadEnabled)
                {
                    MBackend.gamepad.Update(seconds);
                }

                if (MBackend.settings.KeyboardEnabled)
                {
                    MBackend.keyboard.Update(seconds);
                }

                if (MBackend.settings.MouseEnabled)
                {
                    MBackend.mouse.Update(seconds);
                }

                if (MBackend.settings.ConsoleEnabled == false || MBackend.Console.IsOpen == false)
                {
                    MBackend.stateSystem.Update(seconds);
                }

                if (MBackend.settings.ConsoleEnabled)
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
            MBackend.Variables.Variables.BindProperties(MBackend.Settings);
            MBackend.Variables.Variables.BindProperties(MBackend.Mouse);
        }

        private static void CommandExit()
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
        
        private static ICollection<string> CommandGetRemSuggestion(int index)
        {
            return MBackend.Variables.Variables.Identifiers;
        }

        private static ICollection<string> CommandScriptSuggestion(int index)
        {
            return MBackend.ScriptEnvironment.ScriptNames;
        }

        private static void CommandListVariables()
        {
            List<string> identifiers = MBackend.Variables.Variables.Identifiers.ToList();
            identifiers.Sort();
            foreach (string s in identifiers)
            {
                MBackend.Console.WriteLine("\t" + s, MBackend.Variables.Variables.CanSet(s) ? MBackend.Console.DefaultTextColour : MBackend.Console.DisabledTextColour);
            }
        }

        private static void CommandRemove(string[] args)
        {
            if (MBackend.Variables.Variables.Remove(args[0]) == false)
            {
                MBackend.Console.WriteLine("Could not remove variable since it does not exist.", MBackend.Console.ErrorTextColour);
            }
        }

        private static void CommandCompile(string[] args)
        {
            if (MBackend.ScriptEnvironment.Compile(args[0]))
            {
                IScript script = MBackend.ScriptEnvironment[args[0]];
                if(script.Errors.Count == 0)
                {
                    Console.WriteLine($"Script '{script.Name}' compiled.", Console.CommandTextColour);
                }
                else
                {
                    Console.WriteLine($"Script '{script.Name}' compiled with {script.Errors.Count} errors:\n  {string.Join("\n  ", script.Errors)}", Console.ErrorTextColour);
                }
            }
            else
            {
                Console.WriteLine($"Provided script '{args[0]}' does not exist.", Console.ErrorTextColour);
            }
        }

        private static void CommandCompileOutdated()
        {
            int amount = MBackend.ScriptEnvironment.CompileOutdated();
            Console.WriteLine($"Compiled {amount} scripts.", Console.CommandTextColour);
        }

        private static void CommandCompileAll()
        {
            int amount = MBackend.ScriptEnvironment.CompileAll();
            Console.WriteLine($"Compiled {amount} scripts.", Console.CommandTextColour);
        }

        private static void CommandRun(string[] args)
        {
            if(MBackend.ScriptEnvironment.Contains(args[0]))
            {
                IScript script = MBackend.ScriptEnvironment[args[0]];
                if(script.CanExecute)
                {
                    StringConverter sc = new StringConverter();
                    object[] arguments = args.Skip(1).Select(a => sc.ToAny(a)).ToArray();
                    ScriptExecution result = script.Execute(arguments);

                    if(result.Success)
                    {
                        if(script.ReturnsValue)
                        {
                            Console.WriteLine($"Execution result: {result.Result}", Console.CommandTextColour);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error executing script '{script.Name}': {result.Message}", Console.ErrorTextColour);
                    }
                }
                else
                {
                    Console.WriteLine($"Can not execute script '{script.Name}'.", Console.ErrorTextColour);
                }
            }
            else
            {
                Console.WriteLine($"Provided script '{args[0]}' does not exist.", Console.ErrorTextColour);
            }
        }

        private static void CommandListScripts()
        {
            var scripts = MBackend.ScriptEnvironment.ScriptNames.Select(s => MBackend.ScriptEnvironment[s]).OrderBy(s => s.Name);
            foreach (var s in scripts)
            {
                StringBuilder sb = new StringBuilder("\t");
                if (s.ReturnsValue)
                {
                    sb.Append(s.ReturnType.Name);
                    sb.Append(" ");
                }
                sb.Append(s.Name);
                
                if (s.IsOutdated)
                {
                    sb.Append(" -outdated-");
                }
                if(s.Errors.Count != 0)
                {
                    sb.Append($" -{s.Errors.Count} errors-");
                }

                Color c = MBackend.Console.DefaultTextColour;
                if (s.IsOutdated) c = MBackend.Console.WarningTextColour;
                if (s.Errors.Count != 0) c = MBackend.Console.ErrorTextColour;
                
                MBackend.Console.WriteLine(sb.ToString(), c);
            }
        }

        private static void CommandSource(string[] args)
        {
            if (MBackend.ScriptEnvironment.Contains(args[0]))
            {
                IScript script = MBackend.ScriptEnvironment[args[0]];
                Console.WriteLine($"Printing source for '{script.Name}' from '{script.Source.Date}': ", Console.CommandTextColour);
                Console.WriteLine("> " + script.Source.Code.Replace("\n", "\n> "));
            }
            else
            {
                Console.WriteLine($"Provided script '{args[0]}' does not exist.", Console.ErrorTextColour);
            }
        }

        private static void CommandSet(string[] arguments)
        {
            if (MBackend.Variables.Variables.Contains(arguments[0]) && MBackend.Variables.Variables.CanSet(arguments[0]) == false)
            {
                Console.WriteLine("Can not set variable since it is read-only", Console.ErrorTextColour);
            }
            else if (MBackend.Variables.VariablePopulator.LoadItem(arguments[0], arguments[1]) == false)
            {
                Console.WriteLine("Variable assignment failed", Console.ErrorTextColour);
            }
        }

        private static ICollection<string> CommandSetSuggestion(int index)
        {
            if (index == 0)
            {
                return MBackend.Variables.Variables.Identifiers;
            }
            return new string[0];
        }

        private static void CommandVersion()
        {
            MBackend.console.WriteLine("       MonoKle Version:\t" + Assembly.GetAssembly(typeof(Timer)).GetName().Version);
            MBackend.console.WriteLine("MonoKle Engine Version:\t" + Assembly.GetAssembly(typeof(MBackend)).GetName().Version);
        }

        private static void InitializeConsole()
        {
            MBackend.console = new GameConsole(new Rectangle(0, 0, GraphicsManager.Resolution.X, GraphicsManager.Resolution.Y / 3),
                MBackend.GraphicsManager.GraphicsDevice,
                MBackend.keyboard,
                MBackend.TextureStorage.White,
                MBackend.Logger);
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
            MBackend.Console.CommandBroker.Register(new ArgumentlessConsoleCommand("exit", "Terminates the application.", MBackend.CommandExit));
            MBackend.Console.CommandBroker.Register(new ArgumentlessConsoleCommand("version", "Prints the current MonoKle version.", MBackend.CommandVersion));
            MBackend.Console.CommandBroker.Register(new ArgumentlessConsoleCommand("vars", "Lists the currently active variables.", MBackend.CommandListVariables));
            MBackend.Console.CommandBroker.Register(new ArgumentlessConsoleCommand("scripts", "Lists the known scripts.", MBackend.CommandListScripts));
            MBackend.Console.CommandBroker.Register(
                new ConsoleCommand("get", "Prints the value of the provided variable.",
                    new CommandArguments(new string[] { "variable" }, new string[] { "The variable to print" }),
                    MBackend.CommandGet, null, MBackend.CommandGetRemSuggestion)
                );
            MBackend.Console.CommandBroker.Register(
                new ConsoleCommand("set", "Assigns the provided variable with the given value.",
                    new CommandArguments(new string[] { "variable", "value" }, new string[] { "The variable to print", "The value to assign" }),
                    MBackend.CommandSet, null, MBackend.CommandSetSuggestion)
                );
            MBackend.Console.CommandBroker.Register(
                new ConsoleCommand("rem", "Removes the provided variable.",
                    new CommandArguments(new string[] { "variable" }, new string[] { "The variable to remove" }),
                    MBackend.CommandRemove, null, MBackend.CommandGetRemSuggestion)
                );
            MBackend.Console.CommandBroker.Register(
                new ConsoleCommand("run", "Runs the specified script.",
                new CommandArguments(new Dictionary<string, string> { {"name", "Name of the script to run." } }),
                MBackend.CommandRun, null, MBackend.CommandScriptSuggestion));

            MBackend.Console.CommandBroker.Register(
                new ConsoleCommand("compile", "Compiles the specified script. If no script is provided, all outdated will be compiled.",
                new CommandArguments(new Dictionary<string, string> { { "name", "Name of the script to compile." } }),
                MBackend.CommandCompile, MBackend.CommandCompileOutdated, MBackend.CommandScriptSuggestion));

            MBackend.Console.CommandBroker.Register(
                new ConsoleCommand("source", "Prints the source for the given script.",
                new CommandArguments(new Dictionary<string, string> { { "name", "Name of the script to print source for." } }),
                MBackend.CommandSource, null, MBackend.CommandScriptSuggestion));
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
            FileStream fs = new FileStream("./crashdump.log", FileMode.OpenOrCreate | FileMode.Truncate);
            MBackend.Logger.WriteLog(fs); // TODO: Remove magic constant. Not into a constants class, but into settings! E.g. Settings.GetValue("crashdump").
        }
    }
}