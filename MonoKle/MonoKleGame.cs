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
    using MonoKle.Script.Interface;
    using System.Text.RegularExpressions;
    using System.Text;
using System.Collections.Generic;


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
            MonoKleGame.MessagePasser.Subscribe(GameConsole.CHANNEL_ID, MonoKleGame_ConsoleCommand);
            MonoKleGame.Logger = Logger.GetGlobalInstance();
            MonoKleGame.ScriptInterface = new ScriptInterface();
            MonoKleGame.ScriptInterface.CompilationError += HandleScriptCompilationError;
            MonoKleGame.ScriptInterface.Print += HandleScriptPrint;
            MonoKleGame.ScriptInterface.RuntimeError += HandleScriptRuntimeError;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void HandleScriptRuntimeError(object sender, Script.VM.Event.RuntimeErrorEventArgs e)
        {
            MonoKleGame.Logger.AddLog(e.Message, LogLevel.Error);
        }

        private void HandleScriptPrint(object sender, Script.VM.Event.PrintEventArgs e)
        {
            MonoKleGame.Console.WriteLine(e.Message);
        }

        private void HandleScriptCompilationError(object sender, Script.Interface.Event.CompilationErrorEventArgs e)
        {
            StringBuilder sb = new StringBuilder("Script compilation error in [");
            sb.Append(e.Script);
            sb.AppendLine("]:");
            foreach(string s in e.Messages)
            {
                sb.AppendLine(s);
            }
            MonoKleGame.Logger.AddLog(sb.ToString(), LogLevel.Error);
        }


        /// <summary>
        /// Gets the script interface. Used for scripting.
        /// </summary>
        public static ScriptInterface ScriptInterface
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the console utility. This displays log messages recorded by the logger and has the ability to send written commands.
        /// </summary>
        public static GameConsole Console
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the font manager. This provides all fonts and is responsible for loading them in from paths.
        /// </summary>
        public static FontManager FontManager
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
        /// Gets the graphics manager utility. This is in charge of screen settings (resolution, full-screen, etc.) and provides the <see cref="GraphicsDevice"/>.
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
        /// Gets the logging utility, same as Logger.GetGlobalInstance().
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
        /// Gets the mouse input utility.
        /// </summary>
        public static IMouseInput Mouse
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the primitive drawer utility, used to draw primitives for screen (should mainly be used for debug purposes).
        /// </summary>
        public static PrimitiveDrawer PrimitiveDrawer
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
        /// Gets the texture manager. This provides all textures and is responsible for loading them in from paths.
        /// </summary>
        public static TextureManager TextureManager
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
            MonoKleGame.PrimitiveDrawer.Render();
            MonoKleGame.Console.Draw();
        }

        protected override void LoadContent()
        {
            MonoKleGame.TextureManager = new TextureManager(GraphicsManager.GetGraphicsDevice());
            MonoKleGame.FontManager = new FontManager(GraphicsManager.GetGraphicsDevice());
            MonoKleGame.PrimitiveDrawer = new PrimitiveDrawer(GraphicsManager.GetGraphicsDevice());
            MonoKleGame.Console = new GameConsole(new Rectangle(0, 0, GraphicsManager.ScreenSize.X, GraphicsManager.ScreenSize.Y / 3), GraphicsManager.GetGraphicsDevice());    // TODO: Break out magic numbers into config file.
            MonoKleGame.Mouse = new MouseInput(GraphicsManager.ScreenSize);
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
            MonoKleGame.Logger.AddLog(e.ExceptionObject.ToString(), LogLevel.Error);
            FileStream fs = new FileStream("./crashdump.log", FileMode.OpenOrCreate | FileMode.Truncate);
            MonoKleGame.Logger.WriteLog(fs); // TODO: Remove magic constant. Not into a constants class, but into settings! E.g. Settings.GetValue("crashdump").
        }

        private void MonoKleGame_ConsoleCommand(object sender, MessageEventArgs e)
        {
            // TODO: Create specialized parser for console commands.
            if (sender == MonoKleGame.Console)
            {
                string s = e.Data as string;
                if (s != null)
                {
                    if (s.Equals("exit"))
                    {
                        this.Exit();
                    } else if(s.StartsWith("run "))
                    {
                        string script = s.Substring(4, s.Length - 4);
                        string[] argumentStrings = Regex.Split(script, " ");

                        object[] arguments = new object[argumentStrings.Length - 1];
                        for (int i = 1; i < argumentStrings.Length; i++)
                        {
                            if(Regex.IsMatch(argumentStrings[i], "true|false"))
                            {
                                arguments[i] = bool.Parse(argumentStrings[i]);
                            }
                            else if(Regex.IsMatch(argumentStrings[i], "((\\d*)|(\\d+)\\.(\\d)+)"))
                            {
                                arguments[i] = float.Parse(argumentStrings[i], System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                arguments[i-1] = argumentStrings[i];
                            }
                        }

                        Script.VM.ExecutionResult result = MonoKleGame.ScriptInterface.ExecuteScript(argumentStrings[0], arguments);
                        MonoKleGame.Console.WriteLine("Return value: " + result.ToString());
                    }
                }
            }
        }
    }
}