using System;
using System.Linq;
using clipr;

namespace Spd.Console.Bootstrap
{
    public class Framework<TOpts> : IDisposable where TOpts : class, new()
    {
        public Framework(string[] args)
        {
            _anyArgs = args?.Length > 0;
            _opts = CliParser.StrictParse<TOpts>(args);
        }

        public void Run(IProgram<TOpts> program)
        {
            program.Options = _opts;
            if (program.Output == null)
                program.Output = new ConsoleOutput();
            string errorText;
            if (program.OptionsValid(_anyArgs, out errorText))
            {
                try
                {
                    program.Run();
                }
                catch (Exception e)
                {
                    const string repeated = "oO0O";
                    var bar = string.Concat(Enumerable.Repeat(repeated, program.Output.Width / repeated.Length + 1)).Substring(0, program.Output.Width - 1);
                    program.Output.WriteLine($"\n{bar}\n\n{e}\n\n{bar}");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(errorText))
                    program.Output.WriteLine($"Error: {errorText}");
            }
        }

        public void Dispose() { }

        private readonly bool _anyArgs;
        private readonly TOpts _opts;
    }
}