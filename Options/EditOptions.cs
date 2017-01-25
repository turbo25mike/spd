using clipr;

namespace Spd.Console.Options
{
    public class EditOptions
    {
        [NamedArgument('d', "description", Action = ParseAction.Store, Description = "Work Item Description", Required = false)]
        public string Description { get; set; }

        [NamedArgument('t', "title", Action = ParseAction.Store, Description = "Work Item Title", Required = false)]
        public string Title { get; set; }

        [NamedArgument('l', "label", Action = ParseAction.Store, Description = "Work Item Title", Required = false)]
        public string Label { get; set; }
        
        [NamedArgument('o', "owner", Action = ParseAction.Store, Description = "Work Item Title", Required = false)]
        public string Owner { get; set; }

        [NamedArgument('s', "size", Action = ParseAction.Store, Description = "Work Item Title", Required = false)]
        public int? Size { get; set; }

        [NamedArgument('p', "priority", Action = ParseAction.Store, Description = "Work Item Title", Required = false)]
        public int? Priority { get; set; }
    }
}
