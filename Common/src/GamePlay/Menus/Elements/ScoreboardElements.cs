using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.Menus.Elements
{
    public class ScoreboardElements : MenuElement
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
        /// The most left position of the name
        /// </summary>
        public int NameX { get; set; }
        /// <summary>
        /// The most top pos of the name
        /// </summary>
        public int NameY { get; set; }
        /// <summary>
        /// The most left pos of the score
        /// </summary>
        public int ScoreX { get; set; }
        /// <summary>
        /// The most top pos of the score
        /// </summary>
        public int ScoreY { get; set; }
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

        public ScoreboardElements()
        {
            Count = 0;
        }

        public void AddElement(string name, int score)
        {
            if (Count >= 11) return;
            Rectangle easyRect = new Rectangle(BaseX,
                                               BaseY + YDiff * Count,
                                               Width,
                                               Height);

            MenuSpriteElement mse = new MenuSpriteElement(null, easyRect, (Count+1)+"", 1.2f);
            AddChild(mse);
            easyRect = new Rectangle(NameX,
                                     NameY + YDiff * Count,
                                     Width,
                                     Height);

            mse = new MenuSpriteElement(null, easyRect, name, 1.2f);
            AddChild(mse);
            easyRect = new Rectangle(ScoreX,
                                     ScoreY + YDiff * Count,
                                     Width,
                                     Height);

            mse = new MenuSpriteElement(null, easyRect, score.ToString(), 1.2f);
            AddChild(mse);
            Count++;
        }

        public void Clear()
        {
            Count = 0;
            if (children != null)
            {
                children.Clear();
            }
        }

    }
}
