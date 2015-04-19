using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus.Elements
{
    public class MenuElement
    {
        public void AddChild(MenuElement child)
        {
            if (children == null)
            {
                children = new List<MenuElement>();
            }

            children.Add(child);
        }

        virtual public void Draw(GameTime gameTime, Color baseColor)
        {
            if (children != null)
            {
                foreach (MenuElement child in children)
                {
                    child.Draw(gameTime, Color.Lerp(baseColor, GetColor(), 0.5f));
                }
            }
        }

        virtual public void Update(GameTime gameTime)
        {
            if (children != null)
            {
                foreach (MenuElement child in children)
                {
                    child.Update(gameTime);
                }
            }
        }

        virtual public bool OnPress(Point pos, bool down)
        {
            if (children != null)
            {
                foreach (MenuElement child in children)
                {
                    if (child.OnPress(pos, down))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        virtual public bool OnHover(Point pos, bool hover)
        {
            if (children != null)
            {
                foreach (MenuElement child in children)
                {
                    if (child.OnHover(pos, hover))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        virtual protected Color GetColor()
        {
            return baseColor;
        }

        public void SetBaseColor(Color color)
        {
            baseColor = color;
        }

        protected List<MenuElement> children = null;

        protected Color baseColor = Color.White;
    }
}
