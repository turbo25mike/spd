using clipr;

namespace Spd.Console.Options
{
    internal class ToOptions
    {
        [PositionalArgument(0)]
        public int ID { get; set; }
    }
}
