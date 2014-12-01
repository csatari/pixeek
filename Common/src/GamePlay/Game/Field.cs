using Pixeek.Transformation;
using System;
namespace Pixeek.Game
{
    
    public class Field
    {
        public Field(Image image, int imageNumber, int column, int row, Transformator trf)
        {
            ImageNumber = imageNumber;
            ColumnIndex = column;
            RowIndex = row;
            ImageProperty = new Image
            {
                Name = image.Name,
                ImageTexture = trf.transform(image.ImageTexture)
            };
        }

        public Image ImageProperty
        {
            get;
            set;
        }
        public int ColumnIndex
        {
            get;
            set;
        }
        public int RowIndex
        {
            get;
            set;
        }

        public int ImageNumber
        {
            get;
            set;
        }

        public bool Available
        {
            get;
            set;
        }

    }
}