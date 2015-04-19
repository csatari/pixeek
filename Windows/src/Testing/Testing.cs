using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Diagnostics;

namespace Pixeek
{
    class Testing
    {
        static void assert(bool expression)
        {
            if (!expression)
            {
                throw new System.Exception("Testing assertion failure");
            }
        }

        public static void RunAllTests()
        {
            // testing assertion system :)
            assert(true);
            RunMenuTests();
        }

        static void RunMenuTests()
        {
            // build a 1x1 menu, see if button works
            System.TimeSpan simple = new System.TimeSpan(1000);
            bool ch = false;
            Menus.Elements.MenuElement root = new Menus.Elements.MenuElement();
            root.AddChild(new Menus.Elements.MenuButtonElement(new Rectangle(0, 0, 1, 1), delegate()
            {
                ch = true;
            }));

            assert(ch == false);
            root.Update(new GameTime(simple, simple));
            assert(ch == false);
            root.OnHover(new Point(0, 0), true);
            assert(ch == false);
            root.Update(new GameTime(simple, simple));
            assert(ch == false);
            root.OnHover(new Point(0, 0), false);
            assert(ch == false);
            root.Update(new GameTime(simple, simple));
            assert(ch == false);
            root.OnPress(new Point(0, 0), true);
            assert(ch == false);
            root.Update(new GameTime(simple, simple));
            assert(ch == false);
            root.OnPress(new Point(0, 0), false);
            assert(ch == true);
        }
    }
}