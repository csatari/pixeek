using Pixeek.Game;
namespace Pixeek.Saving
{
    public abstract class Save
    {
        
        public abstract Board load(); //ha más platformon is ugyanilyen, akkor nem kell absztraktnak lennie
        public abstract void save(Board board);

        public string serialize(Board board)
        {
            return "";
        }
    }
}