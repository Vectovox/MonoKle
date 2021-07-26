using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using MonoKle.Graphics;

namespace MonoKle.Engine.Android
{
    /// <summary>
    /// Activity that boostraps MonoKle on Android on top of <see cref="AndroidGameActivity"/>.
    /// </summary>
    public abstract class MonoKleActivity : AndroidGameActivity
    {
        /// <summary>
        /// Gets the underlying view. Populated on activity creation (<see cref="OnCreate(Bundle)"/>).
        /// </summary>
        protected View View { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create MonoKleGame
            var game = MGame.Create(GraphicsMode.Borderless);
            
            // Hook up MonoGame
            View = game.Services.GetService(typeof(View)) as View;
            SetContentView(View);

            // Android settings
            Window.Attributes.LayoutInDisplayCutoutMode = LayoutInDisplayCutoutMode.ShortEdges;
            View.SystemUiVisibility = (StatusBarVisibility)
                  (SystemUiFlags.LayoutStable |
                   SystemUiFlags.LayoutHideNavigation |
                   SystemUiFlags.LayoutFullscreen |
                   SystemUiFlags.HideNavigation |
                   SystemUiFlags.Fullscreen |
                   SystemUiFlags.ImmersiveSticky);
            
            // Launch game
            OnBeforeRun(game);
            game.Run();
        }

        /// <summary>
        /// Called just before game is run, allowing setting of states or overriding Android settings.
        /// </summary>
        /// <param name="game">The game instance that is going to run.</param>
        protected abstract void OnBeforeRun(MGame game);
    }
}
