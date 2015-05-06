using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Pixeek.Menus.Elements
{
    //TODO androidban működik-e?
    public class MenuTextElement : MenuElement
    {
        public MenuTextElement(Rectangle textboxArea)
            : base()
        {
            this.textboxArea = textboxArea;
            this.texture = GameManager.Instance.Content.Load<Texture2D>("GUI/text_bg.png");
            oldState = Keyboard.GetState();
        }

        KeyboardState oldState;

        override public void Update(GameTime gameTime)
        {
            if (keyboardOpened)
            {
                KeyboardState newState = Keyboard.GetState();
                if (newState.GetPressedKeys().Length > 0)
                {
                    if (oldState.GetPressedKeys().Length == 0)
                    {
                        KeyDown(newState.GetPressedKeys()[0]);
                    }
                    else
                    {
                        if( !oldState.GetPressedKeys().First().Equals(newState.GetPressedKeys().First()) )
                        {
                            KeyDown(newState.GetPressedKeys()[0]);
                        }
                    }
                }
                oldState = newState;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Beírja a Text-be az adott billentyű tartalmát
        /// </summary>
        /// <param name="key"></param>
        private void KeyDown(Keys key)
        {
            if (!IsKeyAChar(key) && !IsKeyADigit(key) && key != Keys.Back && key != Keys.Space)
            {
                return;
            }
            if (Text.Length > 20) return;
            if (key == Keys.Back)
            {
                if (Text.Length >= 1)
                {
                    Text = Text.Substring(0, Text.Length - 1);
                }
            }
            else if (key == Keys.Space)
            {
                Text += " ";
            }
            else
            {
                if (IsKeyADigit(key))
                {
                    Text += KeyToDigit(key);
                }
                else
                {
                    Text += key.ToString();
                }
            }
        }

        /// <summary>
        /// Megadja, hogy az adott billentyű karakter-e
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyAChar(Keys key)
        {
            return key >= Keys.A && key <= Keys.Z;
        }

        /// <summary>
        /// Megadja,hogy az adott billentyű szám-e
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyADigit(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9);
        }

        /// <summary>
        /// Visszaadja a szám billentyű szám értékét
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string KeyToDigit(Keys key)
        {
            switch (key)
            {
                case Keys.D0: return "0";
                case Keys.D1: return "1";
                case Keys.D2: return "2";
                case Keys.D3: return "3";
                case Keys.D4: return "4";
                case Keys.D5: return "5";
                case Keys.D6: return "6";
                case Keys.D7: return "7";
                case Keys.D8: return "8";
                case Keys.D9: return "9";
                default: return "";
            }
        }

        override public bool OnPress(Point pos, bool down)
        {
#if WINDOWS
            if (base.OnPress(pos, down))
            {
                buttonDown = true;
                return true;
            }

            if (textboxArea.Contains(pos))
            {
                if (buttonDown && !down)
                {
                    keyboardOpened = true;
                    buttonDown = false;
                    return true;
                }
                else if (down)
                {
                    buttonDown = true;
                }
            }
            else
            {
                keyboardOpened = false;
                buttonDown = false;
            }

            return false;
#endif
#if ANDROID
                if (area.Contains(pos))
                {
                    if (clickHandler != null)
                    {
                        clickHandler();
                    }
                    Debug.WriteLine("megnyomva");
                    return true;
                }
                return false;
#endif
        }

        override public bool OnHover(Point pos, bool hover)
        {
            if (base.OnHover(pos, hover))
            {
                buttonHovered = true;
                return true;
            }

            if (textboxArea.Contains(pos))
            {
                if (buttonHovered && !hover)
                {
                    buttonHovered = false;
                    return true;
                }
                else if (hover)
                {
                    buttonHovered = true;
                }
            }
            else
            {
                buttonHovered = false;
            }

            return false;
        }

        protected override Color GetColor()
        {
            Color bc = base.GetColor();
            if (buttonHovered)
            {
                return Color.Lerp(Color.LightGray, bc, 0.5f);
            }
            else return bc;
        }

        override public void Draw(GameTime gameTime, Color baseColor)
        {
            if (texture != null)
            {
                GameManager.Instance.spriteBatch.Draw(texture, textboxArea, Color.Lerp(GetColor(), baseColor, 0.5f));
            }

            if (Text != null)
            {
                Vector2 size = GameManager.Instance.font.MeasureString(Text);
                size.X *= scale;
                size.Y *= scale;
                Vector2 pos = new Vector2(textboxArea.Center.X - size.X / 2, textboxArea.Center.Y - size.Y / 2);
                GameManager.Instance.spriteBatch.DrawString(GameManager.Instance.font, Text, pos, Color.White, 0,
                    new Vector2(0, 0), scale, SpriteEffects.None, 0);
            }
            base.Draw(gameTime, baseColor);
        }

        bool buttonDown = false;
        bool buttonHovered = false;
        bool keyboardOpened = false;
        private Rectangle textboxArea;
        private Texture2D texture;
        public string Text = "";
        private float scale = 1.0f;
    }
}
