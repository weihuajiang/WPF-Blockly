namespace ScratchNet
{
    public static class Localize
    {
        public static string GetString(string key)
        {
            return localization.Strings.ResourceManager.GetString(key, System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}