using Pixeek.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pixeek.Transformation
{
    public class ColorTransformation : Transformator
    {
        public ColorTransformation(Difficulty difficulty, int random) :
            base(difficulty, random) { }

        public override Texture2D transform(Texture2D texture)
        {
            // No color transformation when difficulty is easy
            if (difficulty == Difficulty.EASY)
                return texture;

            // Load texture data
            GraphicsDevice gd = GameManager.Instance.GraphicsDevice;
            int width = texture.Width;
            int height = texture.Height;
            Color[] textureData = new Color[width * height];
            texture.GetData<Color>(textureData);

            // Translate pixels' colors
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    Color pixel = textureData[i * width + j];
                    byte gray = (byte)(0.299f * pixel.R + 0.587f * pixel.G + 0.114 * pixel.B);
                    textureData[i * width + j].R = gray;
                    textureData[i * width + j].G = gray;
                    textureData[i * width + j].B = gray;
                }

            // Return transformed texture
            Texture2D newTexture = new Texture2D(gd, width, height);
            newTexture.SetData<Color>(textureData);
            return newTexture;
        }
    }
}