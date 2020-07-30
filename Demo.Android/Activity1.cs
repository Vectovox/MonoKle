using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Demo.Domain;
using Microsoft.Xna.Framework;
using MonoKle.Engine;

namespace Demo.Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.FullUser,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private View _view;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var _game = MonoKleGame.Create(true);
            Boilerplate.ConfigureStates();
            _view = _game.Services.GetService(typeof(View)) as View;
            SetContentView(_view);
            _game.Run();
        }
    }
}
