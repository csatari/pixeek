using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Pixeek.GameDrawables
{
    public class TimeDrawable : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Rectangle backgroundArea;
        Color Colori;
        RectangleOverlay background;
        Timer timer;

        public TimeDrawable(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            TimerText = "";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundArea = new Rectangle(0, 0, GameManager.Width / 7, GameManager.Height / 8);
            background = new RectangleOverlay(backgroundArea, Color.LightGray, 100, GameManager.Instance);
            GameManager.Instance.Components.Add(background);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            background.Draw(gameTime);

            Vector2 size = GameManager.Instance.font.MeasureString(TimerText);
            Vector2 pos = new Vector2(backgroundArea.Center.X - size.X / 2, backgroundArea.Center.Y - size.Y / 2);
            spriteBatch.DrawString(GameManager.Instance.font, TimerText, pos, Color.White);
            //spriteBatch.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public string TimerText
        {
            get;
            set;
        }
    }
}