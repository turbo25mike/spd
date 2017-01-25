using clipr;

namespace Spd.Console.Options
{
    public class ToOptions
    {
        [PositionalArgument(0)]
        public string ID { get; set; }
    }
}
