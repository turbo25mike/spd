using System;
using System.Threading;

namespace Spd.Console.Extensions
{
    public class Animations
    {
        public class Spinner
        {
            public Spinner(int left, int top, int delay = 100)
            {
                _left = left;
                _top = top;
                _delay = delay;
                _thread = new Thread(Spin);
            }

            public void Start()
            {
                _active = true;
                if (!_thread.IsAlive)
                    _thread.Start();
            }

            public void Stop()
            {
                _active = false;
                Draw(' ');
            }

            public void Dispose()
            {
                Stop();
            }

            private void Spin()
            {
                while (_active)
                {
                    Turn();
                    Thread.Sleep(_delay);
                }
            }

            private void Draw(char c)
            {
                System.Console.SetCursorPosition(_left, _top);
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.Write(Message + " "  + c);
            }

            private void Turn()
            {
                Draw(Sequence[++_counter % Sequence.Length]);
            }

            public string Message { get; set; }

            private const string Sequence = @"/-\|";
            private readonly int _delay;
            private readonly int _left;
            private readonly Thread _thread;
            private readonly int _top;
            private bool _active;
            private int _counter;
        }
    }
}