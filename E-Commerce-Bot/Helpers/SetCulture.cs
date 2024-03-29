using System.Globalization;

namespace E_Commerce_Bot.Helpers
{
    public static class SetCulture
    {
        public static void SetUserCulture(string language)
        {
            CultureInfo.CurrentCulture = new CultureInfo(language);
            CultureInfo.CurrentUICulture = new CultureInfo(language);
        }
    }
}
