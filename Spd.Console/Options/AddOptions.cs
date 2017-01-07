using clipr;

namespace Spd.Console.Options
{
    internal class AddOptions
    {
        [PositionalArgument(0)]
        public string Title { get; set; }

        [NamedArgument('d', "description", Action = ParseAction.Store, Description = "Work Item Description", Required = false)]
        public string Description { get; set; }

        [NamedArgument('t', "tags", Action = ParseAction.Store, Description = "Add tags to define things like status (comma delineated)", Required = false)]
        public string Tags { get; set; }
    }
}
