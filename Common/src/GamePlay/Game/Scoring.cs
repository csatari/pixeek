using System.Timers;
namespace Pixeek.Game
{
    public class Scoring
    {
        public int Combo { get; set; }
        public int Score { get; set; }
        private Timer timer;

        public Scoring()
        {
            Combo = 1;
            Score = 0;
            timer = new Timer(2500);
            timer.Elapsed += timer_Elapsed;
        }
        /// <summary>
        /// Hozzáad megadott pontot az osztályhoz kombó szorzójával együtt
        /// </summary>
        /// <param name="score"></param>
        public void addPoint(int score)
        {
            Score += Combo * score;
            setCombo();
        }

        /// <summary>
        /// Hozzáad a kombóhoz egyet, ha még nem telt le az elõzõ kombó ideje.
        /// </summary>
        public void setCombo()
        {
            if (Combo < 32)
            {
                Combo *= 2;
            }
            stopTimer();
            timer.Start();
        }

        /// <summary>
        /// Az aktuálisan futó timer letelt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Combo > 1)
            {
                Combo /= 2;
            }
            else
            {
                Combo = 1;
            }
            stopTimer();
            timer.Start();
        }

        /// <summary>
        /// Törli az aktuálisan futó timert.
        /// </summary>
        private void stopTimer()
        {
            timer.Stop();
        }
    }
}