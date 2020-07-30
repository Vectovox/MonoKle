using Android.App;
using Android.Content.PM;
using Demo.Domain;
using MonoKle.Engine;
using MonoKle.Engine.Android;

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
    public class GameActivity : MonoKleActivity
    {
        protected override void OnBeforeRun(MonoKleGame game)
        {
            Boilerplate.ConfigureStates();
        }
    }
}
