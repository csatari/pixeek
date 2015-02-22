using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;

namespace Pixeek.SoundVibration
{
    public class SoundAndVibrationAndroid : SoundAndVibration
    {
        protected MediaPlayer player;
        public void playSound()
        {
            try
            {
                if (player == null)
                {
                    player = new MediaPlayer();
                }
                player.Reset();
                var descriptor = GameManager.Activity.Assets.OpenFd("Content/buttonsound.wav");
                player.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
                player.Prepare();
                player.Start();
            }
            catch (Exception e)
            {
            }
        }

        public void playSoundBad()
        {
            try
            {
                if (player == null)
                {
                    player = new MediaPlayer();
                }
                player.Reset();
                var descriptor = GameManager.Activity.Assets.OpenFd("Content/buttonsoundbad.wav");
                player.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
                player.Prepare();
                player.Start();
            }
            catch (Exception e)
            {
            }
        }

        public void vibrate()
        {
            try
            {
                Vibrator vibrator = (Vibrator)Pixeek.GameManager.Activity.GetSystemService(Context.VibratorService);
                vibrator.Vibrate(20);
            }
            catch (Exception e)
            {
            }
        }

        public void vibrateBad()
        {
            try
            {
                Vibrator vibrator = (Vibrator)Pixeek.GameManager.Activity.GetSystemService(Context.VibratorService);
                vibrator.Vibrate(100);
            }
            catch (Exception e)
            {
            }
        }
    }
}