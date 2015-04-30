namespace Pixeek.Game
{
    /// <summary>
    /// NORMAL - fix számú kép megkeresése
    /// TIME - Megadott idõre végtelen számú kép megkeresése, minden képtalálatnál az idõ kitolása
    /// ENDLESS - Végtelen kép és végtelen idõ, ellenben hatalmas pálya
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