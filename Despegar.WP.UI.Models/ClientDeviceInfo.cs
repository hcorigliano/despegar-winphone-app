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
         private String deviceOsVersion;
         private String deviceOsVersionCode;
         private String appVersion;
         private String appBrand;

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
            
             deviceType = "Phone";
             appBrand = "Despegar";
            #if DECOLAR
                appBrand = "Decolar";            
            #endif
         }

        public string GetClientInfo()
         {
             return string.Format("Device:{0} Brand:{1} Model:{2} OS:{3} AppVersion:{4}",deviceType, deviceBrandName, deviceModelName, deviceOsName, appVersion); 
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