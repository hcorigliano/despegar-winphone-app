using Windows.ApplicationModel.Resources;

namespace Despegar.WP.UI.Resource
{
    public static class AppResources
    {
        private static ResourceLoader legacyManager = new ResourceLoader("LegacyStrings");

        public static string GetLegacyString(string resourceString)
        {
            return legacyManager.GetString(resourceString);
        }
    }
}
