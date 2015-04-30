namespace Pixeek.Game
{
    /// <summary>
    /// NORMAL - fix sz�m� k�p megkeres�se
    /// TIME - Megadott id�re v�gtelen sz�m� k�p megkeres�se, minden k�ptal�latn�l az id� kitol�sa
    /// ENDLESS - V�gtelen k�p �s v�gtelen id�, ellenben hatalmas p�lya
    /// </summary>
    public enum GameMode
    {
        NORMAL, 
        TIME,
        ENDLESS,
        TIMER,
        FIGHT
    }
}