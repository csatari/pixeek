using Pixeek.Game;
using Microsoft.Xna.Framework.Graphics;

namespace Pixeek.Transformation
{
    public class Transformator
    {
        protected Difficulty difficulty
        {
            get;
            set;
        }

        protected int random
        {
            get;
            set;
        }

        public Transformator(Difficulty difficulty, int random)
        {
            this.difficulty = difficulty;
            this.random = random;
        }

        public virtual Texture2D transform(Texture2D texture)
        {
            return texture;
        }
    }
}