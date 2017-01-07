using clipr;

namespace Spd.Console.Options
{
    internal class UpdateOptions
    {
        [NamedArgument('d', "description", Action = ParseAction.Store, Description = "Work Item Description", Required = false)]
        public string Description { get; set; }
    }
}
