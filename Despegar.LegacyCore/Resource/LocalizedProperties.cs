
namespace Despegar.LegacyCore.Resource
{
    /// <summary>
    /// Proporciona acceso a los recursos de cadena.
    /// </summary>
    public class LocalizedProperties
    {
        private static Properties _appProperties = new Properties();

        public Properties AppProperties { get { return _appProperties; } }
    }
}