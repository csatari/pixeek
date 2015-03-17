using Pixeek.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Pixeek.Transformation
{
    public class Blur : Transformator
    {
        public Blur(Difficulty difficulty, int random) :
            base(difficulty, random) { }

        public override Texture2D transform(Texture2D texture)
        {

            // No Gaussian-blur when difficulty is easy
            if (difficulty == Difficulty.EASY)
                return texture;

            // Load texture data
            GraphicsDevice gd = GameManager.Instance.GraphicsDevice;
            int width = texture.Width;
            int height = texture.Height;
            Color[] textureData = new Color[width * height];
            texture.GetData<Color>(textureData);

            // Separate channels
            int n = width * height;
            byte[] red = new byte[n];
            byte[] green = new byte[n];
            byte[] blue = new byte[n];
            for (int i = 0; i < n; i++)
            {
                red[i] = textureData[i].R;
                green[i] = textureData[i].G;
                blue[i] = textureData[i].B;
            }

            // Apply Gaussian-blur
            int r = difficulty == Difficulty.HARD ? 8 : 4;
            byte[] new_red = new byte[n];
            byte[] new_green = new byte[n];
            byte[] new_blue = new byte[n];
            gaussianBlur(red, new_red, width, height, r);
            gaussianBlur(green, new_green, width, height, r);
            gaussianBlur(blue, new_blue, width, height, r);
            for (int i = 0; i < n; i++)
            {
                textureData[i].R = new_red[i];
                textureData[i].G = new_green[i];
                textureData[i].B = new_blue[i];
            }


            // Return transformed texture
            Texture2D newTexture = new Texture2D(gd, width, height);
            newTexture.SetData<Color>(textureData);


            return newTexture;
        }

        private void gaussianBlur(byte[] in_ch, byte[] out_ch, int width, int height, int r)
        {
            int[] boxes = boxesForGauss(r, 3);
            boxBlur(in_ch, out_ch, width, height, (boxes[0] - 1) / 2);
            boxBlur(out_ch, in_ch, width, height, (boxes[1] - 1) / 2);
            boxBlur(in_ch, out_ch, width, height, (boxes[2] - 1) / 2);
        }

        private void boxBlur(byte[] in_ch, byte[] out_ch, int width, int height, int r)
        {
            for (int i = 0; i < in_ch.Length; i++)
                out_ch[i] = in_ch[i];
            boxBlurHorizontal(out_ch, in_ch, width, height, r);
            boxBlurTotal(in_ch, out_ch, width, height, r);
        }

        private int[] boxesForGauss(int sigma, int rounds)
        {
            double wIdeal = Math.Sqrt((12.0d * sigma * sigma / rounds) + 1.0d);
            int wl = (int)Math.Floor(wIdeal);
            if (wl % 2 == 0)
                wl -= 1;
            int wu = wl + 2;

            double mIdeal = (12.0d * sigma * sigma -
                             rounds * wl * wl -
                             4.0d * rounds * wl -
                             3.0d * rounds) / (-4.0d * wl - 4.0d);
            int m = (int)Math.Round(mIdeal);

            int[] sizes = new int[rounds];
            for (int i = 0; i < rounds; i++)
                sizes[i] = i < m ? wl : wu;
            return sizes;
        }

        private void boxBlurHorizontal(byte[] in_ch, byte[] out_ch, int width, int height, int r)
        {
            double iarr = 1.0d / (r + r + 1);
            for (int i = 0; i < height; i++)
            {
                int ti = i * width;
                int li = ti;
                int ri = ti + r;
                int fv = in_ch[ti];
                int lv = in_ch[ti + width - 1];
                int val = (r + 1) * fv;
                for (int j = 0; j < r; j++)
                    val += in_ch[ti + j];
                for (int j = 0; j <= r; j++)
                {
                    val += in_ch[ri] - fv;
                    ri += 1;
                    out_ch[ti] = (byte)Math.Round(val * iarr);
                    ti += 1;
                }
                for (int j = r + 1; j < width - r; j++)
                {
                    val += in_ch[ri] - in_ch[li];
                    ri += 1;
                    li += 1;
                    out_ch[ti] = (byte)Math.Round(val * iarr);
                    ti += 1;
                }
                for (int j = width - r; j < width; j++)
                {
                    val += lv - in_ch[li];
                    li += 1;
                    out_ch[ti] = (byte)Math.Round(val * iarr);
                    ti += 1;
                }
            }
        }

        private void boxBlurTotal(byte[] in_ch, byte[] out_ch, int width, int height, int r)
        {
            double iarr = 1.0d / (r + r + 1);
            for (int i = 0; i < width; i++)
            {
                int ti = i;
                int li = ti;
                int ri = ti + r * width;
                int fv = in_ch[ti];
                int lv = in_ch[ti + width * (height - 1)];
                int val = (r + 1) * fv;
                for (int j = 0; j < r; j++)
                    val += in_ch[ti + j * width];
                for (int j = 0; j <= r; j++)
                {
                    val += in_ch[ri] - fv;
                    ri += width;
                    out_ch[ti] = (byte)Math.Round(val * iarr);
                    ti += width;
                }
                for (int j = r + 1; j < height - r; j++)
                {
                    val += in_ch[ri] - in_ch[li];
                    li += width;
                    ri += width;
                    out_ch[ti] = (byte)Math.Round(val * iarr);
                    ti += width;
                }
                for (int j = height - r; j < height; j++)
                {
                    val += lv - in_ch[li];
                    li += width;
                    out_ch[ti] = (byte)Math.Round(val * iarr);
                    ti += width;
                }
            }
        }
    }
}