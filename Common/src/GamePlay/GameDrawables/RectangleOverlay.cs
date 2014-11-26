using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Pixeek.GameDrawables
{
    public class RectangleOverlay : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D background;
        Rectangle rectangle;
        Color Colori;

        public RectangleOverlay(Rectangle rect, Color colori, int drawOrder, Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            DrawOrder = drawOrder;
            rectangle = rect;
            Colori = colori;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = new Texture2D(GraphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.LightGray });
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, rectangle, Colori);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}