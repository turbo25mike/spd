using clipr;

namespace Spd.Console.Options
{
    public class Verbs
    {
        [Verb]
        public AddOptions Add { get; set; }
        [Verb]
        public ToOptions To { get; set; }
        [Verb]
        public EditOptions Edit { get; set; }
    }
}
