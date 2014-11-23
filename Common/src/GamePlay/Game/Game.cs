using Pixeek.ImageLoader;
using Pixeek.Saving;
using Pixeek.SoundVibration;
using System.Timers;

namespace Pixeek.Game
{
    public class Game
    {
        private SoundAndVibration soundAndVibration;
        private Timer timeManager;
        private static Game jatekModell = null;
        private ImageDatabase imageDatabase;
        private Save savingManager;
        private LevelManager levelManager;
        private Scoring scoring;
        private Board board;

        public Game()
        {

        }

        public Game getInstance()
        {
            if (jatekModell == null)
            {
                jatekModell = new Game();
            }
            return jatekModell;
        }
    }
}