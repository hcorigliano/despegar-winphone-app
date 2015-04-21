using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;


namespace Despegar.WP.UI.Model
{
    public class ClientDeviceInfo
    {

         private String deviceType;
         private String deviceBrandName;
         private String deviceModelName;
         private String deviceOsName;
         //private String deviceOsVersion;
         //private String deviceOsVersionCode;
         private String appVersion;
         private String appBrand;
         private String installationSource;

         public ClientDeviceInfo()
         {
             EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
             deviceBrandName = deviceInfo.SystemManufacturer;
             deviceModelName = deviceInfo.SystemProductName;
             deviceOsName = deviceInfo.OperatingSystem;

             Package package = Package.Current;
             PackageId packageId = package.Id;
             PackageVersion version = packageId.Version;
             appVersion = string.Format("{0}.{1}.{2}.{3}",version.Major,version.Minor,version.Build,version.Revision);

             var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
             installationSource = localSettings.Values["InstallationSource"].ToString();


             deviceType = "Phone";
             appBrand = "Despegar";
            #if DECOLAR
                appBrand = "Decolar";            
            #endif
         }

        public string GetClientInfo()
         {
             return string.Format("window-phone-{0}", appVersion);
            //return string.Format("Device:{0} Brand:{1} Model:{2} OS:{3} AppVersion:{4}",deviceType, deviceBrandName, deviceModelName, deviceOsName, appVersion);
         }

        public string GetUserAgent()
        {
            if (GlobalConfiguration.CoreContext != null)
                return string.Format("({0}; {1}; {2}; {3}; App Version {4}; {5}-{6}; {7}; {8})", deviceType, deviceBrandName, deviceModelName, deviceOsName, appVersion, GlobalConfiguration.Language, GlobalConfiguration.Site, installationSource , GlobalConfiguration.UPAId);
            return string.Format("({0}; {1}; {2}; {3}; App Version {4}; {5})", deviceType, deviceBrandName, deviceModelName, deviceOsName, appVersion, installationSource);

        }

        public string GetUOW()
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values["UUID"] == null)
            {
                roamingSettings.Values["UUID"] = System.Guid.NewGuid().ToString().Replace("-", "");
            }
            return "WindowsPhone-" + appBrand + "-" + appVersion + "-" + roamingSettings.Values["UUID"].ToString() ;
        }
    }
} 