namespace Pixeek.SoundVibration
{
    public class SoundAndVibrationWindows : SoundAndVibration
    {



        public void PlaySound()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(GameManager.Instance.Content.RootDirectory + "/buttonsound.wav");

            player.Play();
        }

        public void PlaySoundBad()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(GameManager.Instance.Content.RootDirectory + "/buttonsoundbad.wav");
            player.Play();
        }

    

        public void Vibrate()
        {
            // do nothing under windows
        }

        public void VibrateBad()
        {
        }
    }
}