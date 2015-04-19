using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus.Elements
{
    public class MenuSelector<TEnum> : MenuElement
    {
        public TEnum Selected
        {
            get;
            set;
        }

        public MenuSelector(TEnum selector) 
        {
            Selected = selector;
        }

        override public void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (KeyValuePair<TEnum, MenuElement> kvp in elements)
            {
                if (Selected.Equals(kvp.Key))
                {
                    kvp.Value.SetBaseColor(Color.Red);
                }
                else
                {
                    kvp.Value.SetBaseColor(Color.White);
                }
            }
        }

        Dictionary<TEnum, MenuElement> elements = new Dictionary<TEnum, MenuElement>();

        public void AddElementToDictionary(TEnum index, MenuElement elem)
        {
            elements[index] = elem;
            AddChild(elem);
        }
    }
}
