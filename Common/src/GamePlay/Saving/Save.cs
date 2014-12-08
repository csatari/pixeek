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

        public Tuple<Board, Double, List<Image>, int, int> load(List<Image> imageList)     //lehet absztrakt ha m�s platformon nem ilyen
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
              

                //kiolvassuk az x �s y �rt�keket
                x = Convert.ToInt16(sr.ReadLine());
                y = Convert.ToInt16(sr.ReadLine());

                //elk�sz�t�nk egy boardot. az imagelistet a megh�v� f�ggv�nyn�l kell biztos�tani
                Board board = new Board(imageList);

              
                board.X = x;
                board.Y = y;

                //beolvassuk az eltelt m�sodperceket
                Double time = Convert.ToDouble(sr.ReadLine());

                

                //beolvassuk a score-t
                int score = Convert.ToInt16(sr.ReadLine());

                //beolvassuk a combo-t
                int combo = Convert.ToInt16(sr.ReadLine());

                //beolvassuk a megkeresend� k�pek list�j�nak elemsz�m�t
                int imagesToFindNum = Convert.ToInt16(sr.ReadLine());

                //beolvassuk a megkeresend� k�peket
                for (int i = 0; i < imagesToFindNum; i++)
                {
                    //az aktu�lis k�p nev�t beolvassuk
                    tempImage.Name = sr.ReadLine();

                    //a k�p neve alapj�n azonos�tjuk a k�pet
                    temp = 0;
                    while (tempImage.Name != imageList[temp].Name)
                    {
                        temp++;
                    }

                    //az azonos�tott k�p text�r�j�t az imagelist megfelel� elem�b�l �tm�soljuk az aktu�lis k�pbe
                    tempImage.ImageTexture = imageList[temp].ImageTexture;

                    //az aktu�lisan fel�p�tett k�pet (n�v �s texture2d) hozz�adjuk a megkeresend� k�pek list�j�hoz
                    imagesToFind.Add(tempImage);

                }

                    while ((line = sr.ReadLine()) != null)
                    {
                        temp = Convert.ToInt16(line);

                        //elk�sz�tj�k a k�vetkez� fieldet
                        Field field = new Field(imageList[temp], temp, colind, rowind, true, trf);

                        //be�ll�tjuk megfelel�en az oszlop �s sorindexeket
                        colind++;

                        if (colind == y)
                        {
                            colind = 0;
                            rowind++;
                        }

                        //be�ll�tjuk k�v�lr�l a board allFields nev� list�j�t
                        board.AddToAllFields(field);
                    }

                sr.Close();

            var result = Tuple.Create(board, time, imagesToFind, score, combo);

            return result;
        }
        public void save(Board board, Double timeToSave, List<Image> imagesToFindSave, int score, int combo)  //lehet absztrakt ha m�s platformon nem ilyen
        {

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(GameManager.Instance.Content.RootDirectory + "/save.txt"))
            {

                int x = board.X;
                int y = board.Y;
                int imageToSaveNum = imagesToFindSave.Count;

                
                //difficulty m�retek ki�r�sa
                file.WriteLine(x);
                file.WriteLine(y);

                //id� ki�r�sa
                file.WriteLine(timeToSave);


                //score ki�r�sa
                file.WriteLine(score);

                //combo ki�r�sa
                file.WriteLine(combo);

                //megkeresend� k�pek list�j�nak elemsz�ma
                file.WriteLine(imageToSaveNum);

                //megkeresend� k�pek ki�r�sa
                for (int i = 0; i < imageToSaveNum; i++)
                {
                    file.WriteLine(imagesToFindSave[i].Name);
                }

                //k�p sorsz�m�nak ki�r�sa
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