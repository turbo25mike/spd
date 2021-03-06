﻿using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spd.Console.Extensions;
using Spd.Console.Models;

namespace Spd.Console
{
    public class ConfigManager
    {
        private readonly string _path;
        private readonly IAuth _auth;
        public UserConfiguration Config { get; }

        public ConfigManager(IAuth auth0 = null)
        {
            _auth = auth0 ?? new Auth0();
            _path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.spd";
            if (!File.Exists(_path))
                File.WriteAllText(_path, "{}");

            Config = JsonConvert.DeserializeObject<UserConfiguration>(File.ReadAllText(_path));

            if (!string.IsNullOrEmpty(Config.ApiUri))
                Constants.API_Uri = Config.ApiUri;
        }

        public async Task LogIn()
        {
            if (DateTime.Now > Config.ExpirationDate)
            {
                var userToken = await _auth.Login(Config.JWT);
                Config.JWT = userToken.id_token;
                Config.ExpirationDate = userToken.timeStamp.AddMilliseconds(userToken.expires_in);
                Config.UserName = userToken.user_name;
                Save();
            }
        }

        public void LogOut()
        {
            Config.JWT = string.Empty;
            Config.ExpirationDate = DateTime.MinValue;
            Config.UserName = string.Empty;
            Save();
            System.Console.WriteLine("You are now logged out.");
        }

        private void Save()
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(Config, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }

        public void SetLocalApiUri(string uri = "")
        {
            Config.ApiUri = uri;
            Constants.API_Uri = uri;
            Save();
        }
    }
}
