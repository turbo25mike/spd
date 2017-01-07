using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Spd.Console.Extensions;
using Spd.Console.Models;

namespace Spd.Console
{
    public class ConfigManager
    {
        private readonly string _Path;
        private readonly UserConfiguration _Config;

        public string UserName => _Config.UserName;

        [JsonIgnore]
        public bool IsLoggedIn { get; set; }

        [JsonIgnore]
        public bool PasswordNeeded { get; set; }
        

        public ConfigManager()
        {
            _Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.spd";
            if (!File.Exists(_Path))
                File.WriteAllText(_Path, "{}");

            _Config = JsonConvert.DeserializeObject<UserConfiguration>(File.ReadAllText(_Path));

            if (_Config.pc == null || _Config.pe == null)
                PasswordNeeded = true;
        }

        public void LogIn()
        {
            //TODO send to server to validate
            //client.Login(_Config.UserName, DecryptPassword());
            IsLoggedIn = true;
            PasswordNeeded = false;
        }

        public void SetUsername(string username)
        {
            _Config.UserName = username;
            Save();
        }

        public void SetPassword(SecureString password)
        {
            StorePassword(password);
            Save();
        }

        private void StorePassword(SecureString password)
        {
            // Data to protect. Convert a string to a byte[] using Encoding.UTF8.GetBytes().
            byte[] plaintext = Encoding.UTF8.GetBytes(password.ConvertToUnsecureString());

            // Generate additional entropy (will be used as the Initialization vector)
            var entropy = new byte[20];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }

            var ciphertext = ProtectedData.Protect(plaintext, entropy,
                DataProtectionScope.CurrentUser);


            _Config.pe = entropy;
            _Config.pc = ciphertext;
        }


        private string DecryptPassword()
        {
            return Encoding.UTF8.GetString(ProtectedData.Unprotect(_Config.pc, _Config.pe, DataProtectionScope.CurrentUser));
        }

        private void Save()
        {
            File.WriteAllText(_Path, JsonConvert.SerializeObject(_Config, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }
    }
}
