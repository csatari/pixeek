using Pixeek.Game;
using System.IO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pixeek.Saving
{
    public class Save  //lehet absztrakt is
    {

        public Board load(List<Image> imageList)     //lehet absztrakt ha más platformon nem ilyen
        {
            StreamReader sr = new StreamReader(GameManager.Instance.Content.RootDirectory + "/test.txt");
            
                string line;
                int x;
                int y;
                int temp;
                int rowind = 0;
                int colind = 0;

                //kiolvassuk az x és y értékeket
                x = Convert.ToInt16(sr.ReadLine());
                y = Convert.ToInt16(sr.ReadLine());

                //elkészítünk egy boardot. az imagelistet a meghívó függvénynél kell biztosítani
                Board board = new Board(imageList);

              
                board.X = x;
                board.Y = y;

                while ((line = sr.ReadLine()) != null)
                {
                    temp = Convert.ToInt16(line);

                    //elkészítjük a következõ fieldet
                    Field field = new Field()
                    {
                        
                        ImageProperty = imageList[temp],
                        ImageNumber = temp,
                        ColumnIndex = colind,
                        RowIndex = rowind
                    };


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

            return board;
        }
        public void save(Board board)  //lehet absztrakt ha más platformon nem ilyen
        {

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(GameManager.Instance.Content.RootDirectory + "/test.txt"))
            {

                int x = board.X;
                int y = board.Y;
                
                //difficulty méretek kiírása
                file.WriteLine(x);
                file.WriteLine(y);

                //kép sorszámának kiírása
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