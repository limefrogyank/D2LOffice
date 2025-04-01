using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace D2LOffice.Service
{
    public class Settings
    {
        public string GoogleClientId = "";
        public string GoogleClientSecret = "";
        public string[] GoogleScopes = [
            "https://www.googleapis.com/auth/userinfo.email",
            "https://www.googleapis.com/auth/userinfo.profile",
            "openid",
            "profile",
            "https://www.googleapis.com/auth/drive.file"
        ];

    }

    public class SettingsService
    {
        private Settings _settings = new Settings();

        public string GoogleClientSecret { get => _settings.GoogleClientSecret; }
        public string[] GoogleScopes { get => _settings.GoogleScopes; }
        public string GoogleClientId { get => _settings.GoogleClientId; }


        public SettingsService()
        {

        }
    }
}
