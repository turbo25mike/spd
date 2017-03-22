using System;
using Spd.Console.Bootstrap;
using Spd.Console.Options;

namespace Spd.Console
{
    internal class SpdProgram : Program<Verbs>
    {
        public override bool OptionsValid(bool argumentsProvided, out string errorText)
        {
            if (Options.Add != null)
            {
                if (string.IsNullOrWhiteSpace(Options.Add.Description))
                    Options.Add.Description = "test";

                //foreach (var prop in Options.Add.GetType().GetProperties())
                //   Output.WriteLine($"{prop.Name} : {prop.GetValue(Options.Add)}");
            }

            return base.OptionsValid(true, out errorText);
        }

        public override void Run()
        {
            try
            {
                var configManager = new ConfigManager();
                if (Options == null) return;

                if (Options.Logout)
                {
                    configManager.LogOut();
                    return;
                }

                if (Options.Dev != null)
                {
                    Options.Dev?.Run(configManager).Wait();
                    return;
                }

                configManager.LogIn().Wait();
                new ProjectManager(configManager).Run(Options);
            }
            catch (Exception ex)
            {
                Output.WriteLine(ex.ToString());
            }
        }
    }
}
