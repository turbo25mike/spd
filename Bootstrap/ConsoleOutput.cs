namespace Spd.Console.Bootstrap
{
    public interface IConsoleOutput
    {
        int Width { get; }
        void WriteLine(string value);
        void WriteLine(string format, params object[] args);
        void Write(string s);
    }

    public class ConsoleOutput : IConsoleOutput
    {
        public int Width => System.Console.BufferWidth;

        public void WriteLine(string value)
        {
            System.Console.WriteLine(value);
        }

        public void WriteLine(string format, params object[] args)
        {
            System.Console.WriteLine(format, args);
        }

        public void Write(string s)
        {
            System.Console.Write(s);
        }
    }
}
