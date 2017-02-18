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
                new ProjectManager().Run(Options);
            }
            catch (Exception ex)
            {
                Output.WriteLine(ex.ToString());
            }
        }
    }
}
