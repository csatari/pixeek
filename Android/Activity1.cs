using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Android
{
    [Activity(Label = "Android"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Pixeek.GameManager.Activity = this;
            var g = new Pixeek.GameManager();
            SetContentView(g.Window);
            //SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }

        public delegate void KeyboardPressed(Keycode keycode);
        public static KeyboardPressed Pressed;

        public override bool DispatchKeyEvent(KeyEvent KEvent)
        {
            KeyEventActions keyaction = KEvent.Action;

            if(keyaction == KeyEventActions.Down)
            {
                Keycode keycode = KEvent.KeyCode;
                if (Pressed != null)
                {
                    Pressed(keycode);
                }
            }
            return base.DispatchKeyEvent(KEvent);
        }
    }
}

