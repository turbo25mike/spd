using clipr;

namespace Spd.Console.Options
{
    public class DevOptions
    {

        [NamedArgument('e', "env", Action = ParseAction.StoreTrue, Description = "Env", Required = false)]
        public bool Environment { get; set; }

        [NamedArgument('s', "status", Action = ParseAction.StoreTrue, Description = "API Status", Required = false)]
        public bool Status { get; set; }

        [NamedArgument('d', "db", Action = ParseAction.StoreTrue, Description = "DB Status", Required = false)]
        public bool DBStatus { get; set; }

        [NamedArgument('a', "authorized", Action = ParseAction.StoreTrue, Description = "Validate User", Required = false)]
        public bool Authorized { get; set; }
    }
}
