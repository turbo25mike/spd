using System;
using System.Threading.Tasks;
using clipr;
using Spd.Console.Models;

namespace Spd.Console.Options
{
    public class ToOptions
    {
        [PositionalArgument(0)]
        public string ID { get; set; }

        public async Task Run(ConfigManager configManager)
        {
            if (string.IsNullOrEmpty(ID))
            {
                configManager.LogIn().Wait();
                var path = $"{Constants.API_Uri}/{SecurePath}";
                System.Console.WriteLine($"Requesting: {path}");
                try
                {
                    var env = await _webservice.Request<string>(RequestType.Get, path, token: configManager.Config.JWT);
                    System.Console.WriteLine($"Server reponse: {env}");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Server reponse: {ex.Message}");
                }
            }

            if (!string.IsNullOrEmpty(ApiUri))
                configManager.SetLocalApiUri(ApiUri);
            
        }
    }
}
