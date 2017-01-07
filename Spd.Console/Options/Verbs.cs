using clipr;

namespace Spd.Console.Options
{
    internal class Verbs
    {
        [Verb]
        public AddOptions Add { get; set; }
        [Verb]
        public ToOptions To { get; set; }
        [Verb]
        public UpOptions Up { get; set; }
        [Verb]
        public UpdateOptions Update { get; set; }
    }
}
