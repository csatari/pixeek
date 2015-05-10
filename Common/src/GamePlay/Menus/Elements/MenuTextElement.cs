using Android;
using Android.InputMethodServices;
using Android.Views.InputMethods;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
#if WINDOWS
            oldState = Keyboard.GetState();
#endif
#if ANDROID
            Activity1.Pressed = new Activity1.KeyboardPressed(Pressed);
#endif
        }

        KeyboardState oldState;

        override public void Update(GameTime gameTime)
        {
#if WINDOWS
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
#endif
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
            
            if (key == Keys.Back)
            {
                DeleteCharacter();
            }
            else if (key == Keys.Space)
            {
                if (Text.Length > 20) return;
                AddSpace();
            }
            else
            {
                if (Text.Length > 20) return;
                if (IsKeyADigit(key))
                {
                    AddCharacter(KeyToDigit(key));
                }
                else
                {
                    AddCharacter(key.ToString());
                }
            }
        }

        private void DeleteCharacter()
        {
            if (Text.Length >= 1)
            {
                Text = Text.Substring(0, Text.Length - 1);
            }
        }
        private void AddSpace()
        {
            Text += " ";
        }
        private void AddCharacter(string s)
        {
            Text += s;
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
            if (textboxArea.Contains(pos))
            {
                InputMethodManager imm = (InputMethodManager)GameManager.Activity.GetSystemService(InputMethodService.InputMethodService);
                imm.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.None);
                keyboardOpened = true;
                buttonDown = false;
                return true;
            }
            else
            {
                keyboardOpened = false;
                buttonDown = false;
            }
            return false;
#endif
        }

#if ANDROID

        private void Pressed(Android.Views.Keycode keycode)
        {
            if (keyboardOpened)
            {
                if (keycode == Android.Views.Keycode.Del)
                {
                    DeleteCharacter();
                }
                else
                {
                    if (Text.Length > 20) return;

                    char c = GetKey(keycode);
                    AddCharacter(c.ToString());
                }
            }
        }

        private char GetKey(Android.Views.Keycode keycode ) 
        {
            switch(keycode) {
                case Android.Views.Keycode.A : return 'A';
                case Android.Views.Keycode.B : return 'B';
                case Android.Views.Keycode.C : return 'C';
                case Android.Views.Keycode.D : return 'D';
                case Android.Views.Keycode.E : return 'E';
                case Android.Views.Keycode.F : return 'F';
                case Android.Views.Keycode.G : return 'G';
                case Android.Views.Keycode.H : return 'H';
                case Android.Views.Keycode.I : return 'I';
                case Android.Views.Keycode.J : return 'J';
                case Android.Views.Keycode.K : return 'K';
                case Android.Views.Keycode.L : return 'L';
                case Android.Views.Keycode.M : return 'M';
                case Android.Views.Keycode.N : return 'N';
                case Android.Views.Keycode.O : return 'O';
                case Android.Views.Keycode.P : return 'P';
                case Android.Views.Keycode.Q : return 'Q';
                case Android.Views.Keycode.R : return 'R';
                case Android.Views.Keycode.S : return 'S';
                case Android.Views.Keycode.T : return 'T';
                case Android.Views.Keycode.U : return 'U';
                case Android.Views.Keycode.V : return 'V';
                case Android.Views.Keycode.W : return 'W';
                case Android.Views.Keycode.X : return 'X';
                case Android.Views.Keycode.Y : return 'Y';
                case Android.Views.Keycode.Z : return 'Z';
                case Android.Views.Keycode.Space : return ' ';
                default : return ' ';
            }
        }

#endif

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
