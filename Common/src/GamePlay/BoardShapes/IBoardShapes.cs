using Pixeek.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixeek.BoardShapes
{
    public interface IBoardShapes
    {
        /// <summary>
        /// Elkészít egy 1-ből és 0-ból álló tömböt, ami egy alakzatot ír le. Ebből készíthető el a pálya.
        /// Az első dimenzió a függőleges tengelyt jelenti, a második pedig vízszintest.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        int[][] getField(Difficulty difficulty);
    }
}
