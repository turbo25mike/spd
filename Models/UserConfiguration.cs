using Newtonsoft.Json;

namespace Spd.Console.Models
{
    public class UserConfiguration
    {
        [JsonProperty("u")]
        public string UserName { get; set; }

        [JsonProperty("e")]
        public byte[] PasswordEntropy { get; set; }

        [JsonProperty("c")]
        public byte[] PasswordCipher { get; set; }
    }
}
