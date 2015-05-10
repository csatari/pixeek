using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus.Elements
{
    public class TutorialButton : MenuElement
    {
        public TutorialButton(Rectangle area, ClickHandler clickHandler)
            : base()
        {
            this.area = area;
            this.clickHandler = clickHandler;
        }

        public delegate void ClickHandler();

        override public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        override public bool OnPress(Point pos, bool down)
        {
#if WINDOWS
            if (base.OnPress(pos, down))
            {
                buttonDown = true;
                return true;
            }

            if (area.Contains(pos))
            {
                if (buttonDown && !down)
                {
                    if (clickHandler != null)
                    {
                        clickHandler();
                    }
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
                    //Debug.WriteLine("megnyomva");
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

            if (area.Contains(pos))
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
            return base.GetColor();
        }

        bool buttonDown = false;
        bool buttonHovered = false;
        private ClickHandler clickHandler;
        private Rectangle area;

    }
}
