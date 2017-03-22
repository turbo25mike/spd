using System;
using System.Threading.Tasks;
using clipr;
using Spd.Console.Extensions;
using Spd.Console.Models;

namespace Spd.Console.Options
{
    public class DevOptions
    {

        [NamedArgument('p', "path", Action = ParseAction.Store, Description = "API unauthorized test", Required = false)]
        public string Path { get; set; }

        [NamedArgument('s', "secure", Action = ParseAction.Store, Description = "API authorized test", Required = false)]
        public string SecurePath { get; set; }

        [NamedArgument('u', "api", Action = ParseAction.Store, Description = "Set local API uri", Required = false)]
        public string ApiUri { get; set; }

        [NamedArgument('r', "reset", Action = ParseAction.StoreTrue, Description = "Reset API uri", Required = false)]
        public bool Reset { get; set; }


        public async Task Run(ConfigManager configManager)
        {

            if (Reset)
            {
                configManager.SetLocalApiUri();
            }

            if (!string.IsNullOrEmpty(Path))
            {
                var path = $"{Constants.API_Uri}/{Path}";
                System.Console.WriteLine($"Requesting: {path}");
                try
                {
                    var env = await WebService.Request<string>(RequestType.Get, path);
                    System.Console.WriteLine($"Server reponse: {env}");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Server reponse: {ex.Message}");
                }
            }

            if (!string.IsNullOrEmpty(SecurePath))
            {
                configManager.LogIn().Wait();
                var path = $"{Constants.API_Uri}/{SecurePath}";
                System.Console.WriteLine($"Requesting: {path}");
                try
                {
                    var env = await WebService.Request<string>(RequestType.Get, path, token: configManager.Config.JWT);
                    System.Console.WriteLine($"Server reponse: {env}");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Server reponse: {ex.Message}");
                }
            }

            if (!string.IsNullOrEmpty(ApiUri))
                configManager.SetLocalApiUri(ApiUri);


            System.Console.WriteLine("------------Development Settings------------");
            System.Console.WriteLine($"API Uri: {Constants.API_Uri}");
        }
    }
}
