using Pixeek.Transformation;
using System;
namespace Pixeek.Game
{
    
    public class Field
    {
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
        public Transformator TransformatorProperty
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