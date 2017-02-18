using System;
using Newtonsoft.Json;

namespace Spd.Console.Models
{
    public class UserConfiguration
    {
        [JsonProperty("user")]
        public string UserName { get; set; }

        [JsonProperty("jwt")]
        public string JWT { get; set; }

        [JsonProperty("exp")]
        public DateTime ExpirationDate { get; set; }
    }
}
