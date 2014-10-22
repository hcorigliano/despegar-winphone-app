using Windows.ApplicationModel.Resources;

namespace Despegar.WP.UI.Strings
{
    public static class AppResources
    {
        public static string GetLegacyString(string resourceString)
        {
            return ResourceLoader.GetForCurrentView("LegacyStrings").GetString(resourceString);
        }
    }
}
