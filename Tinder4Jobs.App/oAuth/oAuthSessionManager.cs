using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Tinder4Jobs.oAuth
{
    class oAuthSessionManager
    {
        static ApplicationDataContainer roamingSettings = null;

        public static void Save(oAuthData data)
        {
            roamingSettings = ApplicationData.Current.RoamingSettings;

            roamingSettings.Values["AccessToken"] = data.AccessToken;
            roamingSettings.Values["OAuthVerifier"] = data.OAuthVerifier;
            roamingSettings.Values["AccessTokenSecretKey"] = data.AccessTokenSecretKey;
        }

        public static oAuthData Load()
        {
            oAuthData oAuthData = new oAuthData();
            roamingSettings = ApplicationData.Current.RoamingSettings;

            Object value1 = roamingSettings.Values["AccessToken"];
            if (value1 != null)
                oAuthData.AccessToken = value1.ToString();

            Object value2 = roamingSettings.Values["OAuthVerifier"];
            if (value2 != null)
                oAuthData.OAuthVerifier = value2.ToString();

            Object value3 = roamingSettings.Values["AccessTokenSecretKey"];
            if (value3 != null)
                oAuthData.AccessTokenSecretKey = value3.ToString();

            return oAuthData;
        }

    }

    class oAuthData
    {
        public string AccessToken { get; set; }
        public string OAuthVerifier { get; set; }
        public string AccessTokenSecretKey { get; set; }
    }
}
