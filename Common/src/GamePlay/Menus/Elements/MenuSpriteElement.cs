using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus.Elements
{
    public class MenuSpriteElement : MenuElement
    {
        public MenuSpriteElement(string textureName, Rectangle area, String text = null, float scale = 1.0f)
        {
            this.area = area;
            if (!string.IsNullOrEmpty(textureName))
            {
                this.texture = GameManager.Instance.Content.Load<Texture2D>(textureName);
            }
            this.Text = text;
            this.scale = scale;
        }

        override public void Draw(GameTime gameTime, Color baseColor)
        {
            if (texture != null)
            {
                GameManager.Instance.spriteBatch.Draw(texture, area, Color.Lerp(GetColor(), baseColor, 0.5f));
            }

            if (Text != null)
            {
                Vector2 size = GameManager.Instance.font.MeasureString(Text);
                size.X *= scale;
                size.Y *= scale;
                Vector2 pos = new Vector2(area.Center.X - size.X / 2, area.Center.Y - size.Y / 2);
                /*GameManager.Instance.spriteBatch.DrawString(
                    GameManager.Instance.font, Text, pos, Color.Lerp(GetColor(), baseColor, 0.5f));*/
                GameManager.Instance.spriteBatch.DrawString(GameManager.Instance.font, Text, pos, Color.Lerp(GetColor(), baseColor, 0.5f), 0,
                    new Vector2(0, 0), scale, SpriteEffects.None, 0);
            }

            base.Draw(gameTime, baseColor);
        }

        protected Rectangle area;
        private Texture2D texture = null;
        private float scale;
        public String Text
        {
            get;
            set;
        }
    }
}
