namespace Pixeek.SoundVibration
{
    public class SoundAndVibrationWindows : SoundAndVibration
    {



        public void playSound()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(GameManager.Instance.Content.RootDirectory + "/buttonsound.wav");

            player.Play();
        }

        public void playSoundBad()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(GameManager.Instance.Content.RootDirectory + "/buttonsoundbad.wav");
            player.Play();
        }

    

        public void vibrate()
        {
            // do nothing under windows
        }

        public void vibrateBad()
        {
        }
    }
}