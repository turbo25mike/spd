using Spd.Console.Options;

namespace Spd.Console.Bootstrap
{
    internal class SpdMain
    {
        private static void Main(string[] args)
        {
            using (var framework = new Framework<Verbs>(args))
                framework.Run(new SpdProgram());
        }
    }
}
