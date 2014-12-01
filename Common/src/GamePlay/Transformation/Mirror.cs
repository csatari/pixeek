using Pixeek.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pixeek.Transformation
{
    public class Mirror : Transformator
    {
        public Mirror(Difficulty difficulty, int random) :
            base(difficulty, random) { }

        public override Texture2D transform(Texture2D texture)
        {
            // No mirroring when difficulty is easy
            if (difficulty == Difficulty.EASY)
                return texture;

            // Load texture data
            GraphicsDevice gd = GameManager.Instance.GraphicsDevice;
            int width = texture.Width;
            int height = texture.Height;
            Color[] textureData = new Color[width * height];
            texture.GetData<Color>(textureData);

            // Mirror on x = y
            if (difficulty == Difficulty.HARD && random % 4 == 0)
            {
                for (int i = 0; i < height - 1; i++)
                    for (int j = 0; j < width - i - 1; j++)
                    {
                        Color tmp = textureData[i * width + j];
                        textureData[i * width + j] =
                            textureData[(height - j) * width - i - 1];
                        textureData[(height - j) * width - i - 1] = tmp;
                    }
            }

            // Mirror on x = -y
            else if (difficulty == Difficulty.HARD && random % 4 == 1)
            {
                for (int i = 1; i < height; i++)
                    for (int j = 0; j < i; j++)
                    {
                        Color tmp = textureData[i * width + j];
                        textureData[i * width + j] =
                            textureData[j * width + i];
                        textureData[j * width + i] = tmp;
                    }
            }

            // Mirror on y = 0
            else if (random % 2 == 0)
            {
                for (int i = 0; i < height / 2; i++)
                    for (int j = 0; j < width; j++)
                    {
                        Color tmp = textureData[i * width + j];
                        textureData[i * width + j] =
                            textureData[(height - i - 1) * width + j];
                        textureData[(height - i - 1) * width + j] = tmp;
                    }
            }

            // Mirror on x = 0
            else
            {
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width / 2; j++)
                    {
                        Color tmp = textureData[i * width + j];
                        textureData[i * width + j] =
                            textureData[i * width + width - j - 1];
                        textureData[i * width + width - j - 1] = tmp;
                    }
            }

            // Return transformed texture
            Texture2D newTexture = new Texture2D(gd, width, height);
            newTexture.SetData<Color>(textureData);
            return newTexture;
        }
    }
}