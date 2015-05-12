namespace Pixeek.Settings
{
    public class SettingsMenu
    {
        private Settings settings;
        private static SettingsMenu settingsMenu;

        public SettingsMenu GetInstance()
        {
            if (settingsMenu == null)
            {
                settingsMenu = new SettingsMenu();
            }
            return settingsMenu;
        }
    }
}