using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixeek.Game;
using System.Collections.Generic;
using System.Timers;

namespace Pixeek.GameDrawables
{
    public class ScoreDrawable : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Rectangle backgroundArea;
        RectangleOverlay background;
        public Scoring Scoring { private get; set; }

        public ScoreDrawable(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundArea = new Rectangle(GameManager.Width / 7, 0, 5 * GameManager.Width / 7, GameManager.Height / 8);
            background = new RectangleOverlay(backgroundArea, new Color(227, 227, 227), 100, GameManager.Instance);
            GameManager.Instance.Components.Add(background);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            background.Draw(gameTime);

            Vector2 size = GameManager.Instance.font.MeasureString("SCORE: "+Scoring.Score+"                  COMBO:"+Scoring.Combo+"x");
            Vector2 pos = new Vector2(backgroundArea.Center.X - size.X / 2, backgroundArea.Center.Y - size.Y / 2);
            spriteBatch.DrawString(GameManager.Instance.font, "SCORE: " + Scoring.Score + "                  COMBO:" + Scoring.Combo + "x", pos, Color.White);
            //spriteBatch.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}