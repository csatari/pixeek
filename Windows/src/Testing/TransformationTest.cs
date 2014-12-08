using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pixeek.Game;
using Pixeek.Transformation;

namespace Pixeek.Transformation
{
    [TestClass]
    public class TransformationTest
    {
        private Texture2D createRandomTexture()
        {
            GraphicsDevice gd = new GraphicsDevice(
                GraphicsAdapter.DefaultAdapter,
                GraphicsProfile.Reach,
                new PresentationParameters());
            Texture2D texture = new Texture2D(gd, 150, 150);
            Color[] textureData = new Color[22500];
            Random random = new Random();
            for (int i = 0; i < 22500; i++)
            {
                textureData[i] = new Color(
                    random.Next(256),  // red
                    random.Next(256),  // green
                    random.Next(256),  // blue
                    0);                // alpha
            }
            texture.SetData<Color>(textureData);
            return texture;
        }

        private Color[] fetchTextureData(Texture2D texture)
        {
            Color[] textureData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(textureData);
            return textureData;
        }

        private Texture2D copyTexture(Texture2D texture)
        {
            Color[] textureData = fetchTextureData(texture);
            GraphicsDevice gd = new GraphicsDevice(
                GraphicsAdapter.DefaultAdapter,
                GraphicsProfile.Reach,
                new PresentationParameters());
            Texture2D newTexture = new Texture2D(gd, texture.Width, texture.Height);
            int size = texture.Width * texture.Height;
            Color[] newTextureData = new Color[size];
            for (int i = 0; i < size; i++)
            {
                Color pixel = textureData[i];
                newTextureData[i] = new Color(pixel.R, pixel.G, pixel.B, pixel.A);
            }
            newTexture.SetData<Color>(newTextureData);
            return newTexture;
        }

        private void checkTrfCompsEqual(
            List<Transformator> trfList1,
            List<Transformator> trfList2)
        {
            Texture2D texture1 = createRandomTexture();
            Texture2D texture2 = copyTexture(texture1);
            foreach (Transformator trf in trfList1)
            {
                texture1 = trf.transform(texture1);
            }
            foreach (Transformator trf in trfList2)
            {
                texture2 = trf.transform(texture2);
            }
            Color[] textureData1 = fetchTextureData(texture1);
            Color[] textureData2 = fetchTextureData(texture2);
            for (int i = 0; i < texture1.Width * texture1.Height; i++)
            {
                Assert.AreEqual(textureData1[i], textureData2[i]);
            }
        }

        private void checkComposedTrfIsID(List<Transformator> trfList)
        {
            checkTrfCompsEqual(trfList, new List<Transformator>());
        }

        private void checkRepeatedTrfIsID(Transformator trf, int repetitions)
        {
            List<Transformator> trfList = new List<Transformator>(repetitions);
            for (int i = 0; i < repetitions; i++)
                trfList.Add(trf);
            checkComposedTrfIsID(trfList);
        }

        [TestMethod]
        public void fourPositiveRotationIsID()
        {
            Transformator trf = new Rotate(Difficulty.NORMAL, 0);
            checkRepeatedTrfIsID(trf, 4);
        }

        [TestMethod]
        public void fourNegativeRotationIsID()
        {
            Transformator trf = new Rotate(Difficulty.NORMAL, 1);
            checkRepeatedTrfIsID(trf, 4);
        }

        [TestMethod]
        public void two180DegreeRotationIsID()
        {
            Transformator trf = new Rotate(Difficulty.HARD, 0);
            checkRepeatedTrfIsID(trf, 2);
        }

        [TestMethod]
        public void positiveThenNegativeRotationIsID()
        {
            Transformator pos = new Rotate(Difficulty.NORMAL, 0);
            Transformator neg = new Rotate(Difficulty.NORMAL, 1);
            checkComposedTrfIsID(new List<Transformator>() { pos, neg });
        }

        [TestMethod]
        public void twoPositiveRotationIs180Rot()
        {
            Transformator pos = new Rotate(Difficulty.NORMAL, 0);
            Transformator r180 = new Rotate(Difficulty.HARD, 0);
            checkTrfCompsEqual(
                new List<Transformator>() { pos, pos },
                new List<Transformator>() { r180 });
        }

        [TestMethod]
        public void twoHorizontalMirroringIsID()
        {
            Transformator trf = new Mirror(Difficulty.NORMAL, 0);
            checkRepeatedTrfIsID(trf, 2);
        }

        [TestMethod]
        public void twoVerticalMirroringIsID()
        {
            Transformator trf = new Mirror(Difficulty.NORMAL, 1);
            checkRepeatedTrfIsID(trf, 2);
        }

        [TestMethod]
        public void twoDiagonalMirroringIsID()
        {
            Transformator trf = new Mirror(Difficulty.HARD, 0);
            checkRepeatedTrfIsID(trf, 2);
        }

        [TestMethod]
        public void twoDiagonal2MirroringIsID()
        {
            Transformator trf = new Mirror(Difficulty.HARD, 1);
            checkRepeatedTrfIsID(trf, 2);
        }

        [TestMethod]
        public void hMirrorIsVMirrorPlus180Rot()
        {
            Transformator hm = new Mirror(Difficulty.NORMAL, 0);
            Transformator vm = new Mirror(Difficulty.NORMAL, 1);
            Transformator r180 = new Rotate(Difficulty.HARD, 0);
            checkTrfCompsEqual(
                new List<Transformator>() { hm },
                new List<Transformator>() { vm, r180 });
        }

        [TestMethod]
        public void dMirrorIsD2MirrorPlus180Rot()
        {
            Transformator dm = new Mirror(Difficulty.HARD, 0);
            Transformator d2m = new Mirror(Difficulty.HARD, 1);
            Transformator r180 = new Rotate(Difficulty.HARD, 0);
            checkTrfCompsEqual(
                new List<Transformator>() { dm },
                new List<Transformator>() { d2m, r180 });
        }

        [TestMethod]
        public void dPlusHMirrorIsD2PlusVMirror()
        {
            Transformator dm = new Mirror(Difficulty.HARD, 0);
            Transformator hm = new Mirror(Difficulty.NORMAL, 0);
            Transformator d2m = new Mirror(Difficulty.HARD, 1);
            Transformator vm = new Mirror(Difficulty.NORMAL, 1);
            checkTrfCompsEqual(
                new List<Transformator>() { dm, hm },
                new List<Transformator>() { d2m, vm });
        }
    }
}