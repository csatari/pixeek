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
        /// Hozz�ad megadott pontot az oszt�lyhoz komb� szorz�j�val egy�tt
        /// </summary>
        /// <param name="score"></param>
        public void addPoint(int score)
        {
            Score += Combo * score;
            setCombo();
        }

        /// <summary>
        /// Hozz�ad a komb�hoz egyet, ha m�g nem telt le az el�z� komb� ideje.
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
        /// Az aktu�lisan fut� timer letelt
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
        /// T�rli az aktu�lisan fut� timert.
        /// </summary>
        private void stopTimer()
        {
            timer.Stop();
        }
    }
}