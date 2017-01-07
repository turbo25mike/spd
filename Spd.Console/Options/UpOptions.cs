using clipr;

namespace Spd.Console.Options
{
    internal class UpOptions
    {
        [NamedArgument('l', "levels", Action = ParseAction.Store, Description = "Move up (x) levels", Required = false)]
        public int? levels { get; set; }
    }
}
