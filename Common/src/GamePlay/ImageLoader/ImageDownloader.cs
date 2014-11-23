using Pixeek.Game;
using System.Collections.Generic;

namespace Pixeek.ImageLoader
{
    public interface ImageDownloader
    {
        List<Image> downloadAll();
    }
}