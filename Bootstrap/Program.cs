using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spd.Console.Bootstrap
{
    public interface IProgram<T>
    {
        IConsoleOutput Output { get; set; }
        T Options { get; set; }
        bool OptionsValid(bool argumentsProvided, out string errorText);
        void Run();
    }

    public abstract class Program<T> : IProgram<T>
    {
        public IConsoleOutput Output { get; set; }
        public T Options { get; set; }

        public virtual bool OptionsValid(bool argumentsProvided, out string errorText)
        {
            errorText = "";
            return argumentsProvided;
        }

        public abstract void Run();
    }
}
