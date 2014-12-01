using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixeek.Game;
using System.Collections.Generic;
using System.Timers;

namespace Pixeek.GameDrawables
{
    public class ToFindDrawable : DrawableGameComponent
    {
        private static ToFindDrawable _instance = null;

        SpriteBatch spriteBatch;
        Rectangle backgroundArea;
        Color Colori;
        RectangleOverlay background;
        Timer timer;
        public ImagesToFind ImagesToFind
        {
            get;
            set;
        }

        public ToFindDrawable(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundArea = new Rectangle(0, 7 * GameManager.Height / 8, GameManager.Width, GameManager.Height / 8);
            background = new RectangleOverlay(backgroundArea, Color.LightGray, 100, GameManager.Instance);
            GameManager.Instance.Components.Add(background);
            base.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            background.Draw(gameTime);

            Vector2 size = GameManager.Instance.font.MeasureString(ImagesToFind.ToString());
            Vector2 pos = new Vector2(backgroundArea.Center.X - size.X / 2, backgroundArea.Center.Y - size.Y / 2);
            spriteBatch.DrawString(GameManager.Instance.font, ImagesToFind.ToString(), pos, Color.White);
            //spriteBatch.Draw();
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}