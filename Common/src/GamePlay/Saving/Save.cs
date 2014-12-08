using Pixeek.Game;
using Pixeek.Transformation;
using System.IO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pixeek.Saving
{
    public class Save  //lehet absztrakt is
    {

        public Tuple<Board, Double, List<Image>, int, int> load(List<Image> imageList)     //lehet absztrakt ha más platformon nem ilyen
        {
            StreamReader sr = new StreamReader(GameManager.Instance.Content.RootDirectory + "/save.txt");
            
                string line;
                int x;
                int y;
                int temp;
                int rowind = 0;
                int colind = 0;
                Transformator trf = new Transformator(Difficulty.NORMAL, 0);
                List<Image> imagesToFind = new List<Image>();
                Image tempImage = new Image();
              

                //kiolvassuk az x és y értékeket
                x = Convert.ToInt16(sr.ReadLine());
                y = Convert.ToInt16(sr.ReadLine());

                //elkészítünk egy boardot. az imagelistet a meghívó függvénynél kell biztosítani
                Board board = new Board(imageList);

              
                board.X = x;
                board.Y = y;

                //beolvassuk az eltelt másodperceket
                Double time = Convert.ToDouble(sr.ReadLine());

                

                //beolvassuk a score-t
                int score = Convert.ToInt16(sr.ReadLine());

                //beolvassuk a combo-t
                int combo = Convert.ToInt16(sr.ReadLine());

                //beolvassuk a megkeresendõ képek listájának elemszámát
                int imagesToFindNum = Convert.ToInt16(sr.ReadLine());

                //beolvassuk a megkeresendõ képeket
                for (int i = 0; i < imagesToFindNum; i++)
                {
                    //az aktuális kép nevét beolvassuk
                    tempImage.Name = sr.ReadLine();

                    //a kép neve alapján azonosítjuk a képet
                    temp = 0;
                    while (tempImage.Name != imageList[temp].Name)
                    {
                        temp++;
                    }

                    //az azonosított kép textúráját az imagelist megfelelõ elemébõl átmásoljuk az aktuális képbe
                    tempImage.ImageTexture = imageList[temp].ImageTexture;

                    //az aktuálisan felépített képet (név és texture2d) hozzáadjuk a megkeresendõ képek listájához
                    imagesToFind.Add(tempImage);

                }

                    while ((line = sr.ReadLine()) != null)
                    {
                        temp = Convert.ToInt16(line);

                        //elkészítjük a következõ fieldet
                        Field field = new Field(imageList[temp], temp, colind, rowind, true, trf);

                        //beállítjuk megfelelõen az oszlop és sorindexeket
                        colind++;

                        if (colind == y)
                        {
                            colind = 0;
                            rowind++;
                        }

                        //beállítjuk kívülrõl a board allFields nevû listáját
                        board.AddToAllFields(field);
                    }

                sr.Close();

            var result = Tuple.Create(board, time, imagesToFind, score, combo);

            return result;
        }
        public void save(Board board, Double timeToSave, List<Image> imagesToFindSave, int score, int combo)  //lehet absztrakt ha más platformon nem ilyen
        {

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(GameManager.Instance.Content.RootDirectory + "/save.txt"))
            {

                int x = board.X;
                int y = board.Y;
                int imageToSaveNum = imagesToFindSave.Count;

                
                //difficulty méretek kiírása
                file.WriteLine(x);
                file.WriteLine(y);

                //idõ kiírása
                file.WriteLine(timeToSave);


                //score kiírása
                file.WriteLine(score);

                //combo kiírása
                file.WriteLine(combo);

                //megkeresendõ képek listájának elemszáma
                file.WriteLine(imageToSaveNum);

                //megkeresendõ képek kiírása
                for (int i = 0; i < imageToSaveNum; i++)
                {
                    file.WriteLine(imagesToFindSave[i].Name);
                }

                //kép sorszámának kiírása
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        if (board.getField(j, i) == null)
                        {
                            file.WriteLine(-1);
                        }
                        else
                        {
                            file.WriteLine(board.getField(j, i).ImageNumber);
                        }
                    }
                }

                
            }
        }

      
    }
}