using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixeek.Game;
using System.Collections.Generic;
using System.Timers;

namespace Pixeek.GameDrawables
{
    public class TutorialBackgroundDrawable : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Rectangle topArea;
        RectangleOverlay topAreaOverlay;
        Rectangle bottomArea;
        RectangleOverlay bottomAreaOverlay;
        private Point TopLeft { get; set; }
        private Point BottomRight { get; set; }

        public TutorialBackgroundDrawable(Microsoft.Xna.Framework.Game game, Point topLeft, Point bottomRight)
            : base(game)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            topArea = new Rectangle(0, 0, GameManager.Width, TopLeft.Y);
            topAreaOverlay = new RectangleOverlay(topArea, new Color(163, 163, 163, 100), 120, GameManager.Instance);
            GameManager.Instance.Components.Add(topAreaOverlay);
            bottomArea = new Rectangle(0, BottomRight.Y, GameManager.Width, GameManager.Height-BottomRight.Y);
            bottomAreaOverlay = new RectangleOverlay(bottomArea, new Color(163, 163, 163, 100), 120, GameManager.Instance);
            GameManager.Instance.Components.Add(bottomAreaOverlay);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            //spriteBatch.Begin();
            topAreaOverlay.Draw(gameTime);
            bottomAreaOverlay.Draw(gameTime);

            //Vector2 size = GameManager.Instance.font.MeasureString("SCORE: "+Scoring.Score+"                  COMBO:"+Scoring.Combo+"x");
            //Vector2 pos = new Vector2(backgroundArea.Center.X - size.X / 2, backgroundArea.Center.Y - size.Y / 2);
            //spriteBatch.DrawString(GameManager.Instance.font, "SCORE: " + Scoring.Score + "                  COMBO:" + Scoring.Combo + "x", pos, Color.White);
            //spriteBatch.Draw();
            //spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}