using Pixeek.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pixeek.Transformation
{
    public class Rotate : Transformator
    {
        public Rotate(Difficulty difficulty, int random) :
            base(difficulty, random) { }

        public override Texture2D transform(Texture2D texture)
        {
            // No rotation when difficulty is easy
            if (difficulty == Difficulty.EASY)
                return texture;

            // Load texture data
            GraphicsDevice gd = GameManager.Instance.GraphicsDevice;
            int width = texture.Width;
            int height = texture.Height;
            Color[] textureData = new Color[width * height];
            texture.GetData<Color>(textureData);

            // Rotate 180 degrees
            if (difficulty == Difficulty.HARD && random % 3 == 0)
            {
                for (int i = 0; i < height / 2; i++)
                    for (int j = 0; j < width; j++)
                    {
                        Color tmp = textureData[i * width + j];
                        textureData[i * width + j] =
                            textureData[(height - i) * width - j - 1];
                        textureData[(height - i) * width - j - 1] = tmp;
                    }
            }

            // Rotate -90 degrees
            else if (difficulty == Difficulty.HARD && random % 3 == 1 ||
                     difficulty == Difficulty.NORMAL && random % 2 == 0)
            {
                for (int i = 0; i < height / 2; i++)
                    for (int j = 0; j < width / 2; j++)
                    {
                        Color tmp = textureData[i * width + j];
                        textureData[i * width + j] =
                            textureData[(height - j - 1) * width + i];
                        textureData[(height - j - 1) * width + i] =
                            textureData[(height - i) * width - j - 1];
                        textureData[(height - i) * width - j - 1] =
                            textureData[j * width + width - i - 1];
                        textureData[j * width + width - i - 1] = tmp;
                    }
            }

            // Rotate +90 degrees
            else
            {
                for (int i = 0; i < height / 2; i++)
                    for (int j = 0; j < width / 2; j++)
                    {
                        Color tmp = textureData[i * width + j];
                        textureData[i * width + j] =
                            textureData[j * width + width - i - 1];
                        textureData[j * width + width - i - 1] =
                            textureData[(height - i) * width - j - 1];
                        textureData[(height - i) * width - j - 1] =
                            textureData[(height - j - 1) * width + i];
                        textureData[(height - j - 1) * width + i] = tmp;
                    }
            }

            // Return transformed texture
            Texture2D newTexture = new Texture2D(gd, width, height);
            newTexture.SetData<Color>(textureData);
            return newTexture;
        }
    }
}