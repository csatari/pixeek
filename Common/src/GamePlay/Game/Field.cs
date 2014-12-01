using Pixeek.Transformation;
namespace Pixeek.Game
{
    public class Field
    {
        public Field(Image image, int column, int row, Transformator trf)
        {
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

    }
}