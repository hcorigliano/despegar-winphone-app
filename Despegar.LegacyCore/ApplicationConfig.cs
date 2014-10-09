
using System;
using Despegar.LegacyCore.Util;
using Windows.System.Profile;
using Windows.Storage.Streams;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;

namespace Despegar.LegacyCore
{
    public class ApplicationConfig
    {
        private string GetDeviceID()
        {
            HardwareToken token = HardwareIdentification.GetPackageSpecificToken(null);
            IBuffer hardwareId = token.Id;

            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer hashed = hasher.HashData(hardwareId);

            string hashedString = CryptographicBuffer.EncodeToHexString(hashed);
            return hashedString;
        }

        public void Init()
        {
            if (Initialized) return;

            DeviceId =          GetDeviceID();
            Country           = string.Empty;
            UpaId             = string.Empty;
            LocationChecked   = false;
            LocationByPhone   = false;
            LocationByApp     = true;
            BrowsingPages     = new BrowsingStack();

            Initialized     = true;
            //LocalSettings.Set(this);
        }

        private static string _DeviceDescription = "{0} Windows Phone App/{1}; (MSIE 10.0; WindowsPhone8 OS {2})";

        public bool Initialized { get; set; }

        public string DeviceId { get; set; }

        public string Brand 
        { 
            get 
            { 
                #if DECOLAR
                    return "Decolar";
                #else
                    return "Despegar";
                #endif
            }
        }

        public string Ver { get { return "2.0.0.0"; } }
        public string Lang
        {
            get
            { 
                #if DECOLAR
                    return "pt-" + Country;
                #else
                    return "es-" + Country;
                #endif
            }
        }
        public string DeviceDescription { get { return String.Format(_DeviceDescription, Brand, Ver, Lang); } }
        public string UpaId { get; set; }

        public string Country { get; set; }
        public bool CountrySelected { get { return !string.IsNullOrWhiteSpace(Country); } }
        public void ResetCountry() { Country = string.Empty; }
        public string IATA { get; set; }

        public bool Location { get { return LocationByPhone && LocationByApp; } }
        public bool LocationByPhone { get; set; }
        public bool LocationChecked { get; set; }
        public bool LocationByApp { get; set; }


        public string PushChannel { get; set; }

        public BrowsingStack BrowsingPages { get; set; }
        public void ResetBrowsingPages(Uri newPage) 
        { 
            BrowsingPages.Clear(); 
            BrowsingPages.Push(newPage); 
        }


        public static volatile ApplicationConfig instance;
        private static object syncRoot = new Object();
        public static ApplicationConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ApplicationConfig();
                        }
                    }
                }

                return instance;
            }
        }
    }
}