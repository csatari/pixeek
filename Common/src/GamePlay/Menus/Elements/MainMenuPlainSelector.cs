using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus.Elements
{
    public class MainMenuPlainSelector<TEnum> : MenuSelector<TEnum>
    {
        /// <summary>
        /// The most left position of the element
        /// </summary>
        public int BaseX { get; set; }
        /// <summary>
        /// The most top position of the element
        /// </summary>
        public int BaseY { get; set; }
        /// <summary>
        /// The space's size between elements
        /// </summary>
        public int YDiff { get; set; }
        /// <summary>
        /// The width of one element
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// The height of one element
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// How many elements the object has
        /// </summary>
        private int Count { get; set; }

        public event EventHandler SelectedChange;

        public MainMenuPlainSelector(TEnum selector) : base(selector)
        {
            Count = 0;
        }

        public void AddElement(string text, TEnum value)
        {
            Rectangle easyRect = new Rectangle(BaseX,
                                               BaseY + YDiff * Count,
                                               Width,
                                               Height);
            MenuButtonElement button = new MenuButtonElement(easyRect, delegate()
            {
                Selected = value;
                if (SelectedChange != null)
                {
                    SelectedChange(this, EventArgs.Empty);
                }
            });
            button.AddChild(new MenuSpriteElement(null, easyRect, text, 1.5f));
            AddElementToDictionary(value, button);
            Count++;
        }

        public int GetHeight()
        {
            return (YDiff * Count) + Height;
        }

        /// <summary>
        /// Get the relative height to the top of the element set in the index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetHeightOfElement(int index)
        {
            return (YDiff * (index-1)) + Height;
        }

    }
}
