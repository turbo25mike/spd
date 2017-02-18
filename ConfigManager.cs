using System;
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
        public UserConfiguration Config { get; }

        public ConfigManager()
        {
            _path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.spd";
            if (!File.Exists(_path))
                File.WriteAllText(_path, "{}");

            Config = JsonConvert.DeserializeObject<UserConfiguration>(File.ReadAllText(_path));
        }

        public async Task LogIn()
        {
            if (DateTime.Now > Config.ExpirationDate)
            {
                var userToken = await Auth0.Login(Config.JWT);
                Config.JWT = userToken.id_token;
                Config.ExpirationDate = userToken.timeStamp.AddSeconds(userToken.expires_in);
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
        }

        private void Save()
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(Config, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }
    }
}
