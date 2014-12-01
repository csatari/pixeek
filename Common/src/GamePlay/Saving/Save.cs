using Pixeek.Game;
using System.IO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pixeek.Saving
{
    public class Save  //lehet absztrakt is
    {

        public Board load(List<Image> imageList)     //lehet absztrakt ha m�s platformon nem ilyen
        {
            StreamReader sr = new StreamReader(GameManager.Instance.Content.RootDirectory + "/test.txt");
            
                string line;
                int x;
                int y;
                int temp;
                int rowind = 0;
                int colind = 0;

                //kiolvassuk az x �s y �rt�keket
                x = Convert.ToInt16(sr.ReadLine());
                y = Convert.ToInt16(sr.ReadLine());

                //elk�sz�t�nk egy boardot. az imagelistet a megh�v� f�ggv�nyn�l kell biztos�tani
                Board board = new Board(imageList);

              
                board.X = x;
                board.Y = y;

                while ((line = sr.ReadLine()) != null)
                {
                    temp = Convert.ToInt16(line);

                    //elk�sz�tj�k a k�vetkez� fieldet
                    Field field = new Field()
                    {
                        
                        ImageProperty = imageList[temp],
                        ImageNumber = temp,
                        ColumnIndex = colind,
                        RowIndex = rowind
                    };


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

            return board;
        }
        public void save(Board board)  //lehet absztrakt ha m�s platformon nem ilyen
        {

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(GameManager.Instance.Content.RootDirectory + "/test.txt"))
            {

                int x = board.X;
                int y = board.Y;
                
                //difficulty m�retek ki�r�sa
                file.WriteLine(x);
                file.WriteLine(y);

                //k�p sorsz�m�nak ki�r�sa
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        file.WriteLine(board.getField(j,i).ImageNumber);
                    }
                }

            }
        }

      
    }
}