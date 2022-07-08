using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace DroneMonitoring.Helpers
{
    public class AppConfig
    {
        public AppConfig()
        {
           
        }
        public static void Load()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings != null)
            {
                var testVal = localSettings.Values["AppKey"] as string;
                if (string.IsNullOrEmpty(testVal)) return;
                // load a setting that is local to the device
                AppConstants.AppKey = localSettings.Values["AppKey"] as string;
               
            }
        }
        public static void Save()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            // Save a setting locally on the device
            localSettings.Values["AppKey"] = AppConstants.AppKey;
           

        }
    }
}
