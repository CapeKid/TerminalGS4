using System;
using Terminal.Terminals;

namespace Terminal
{
    class Program
    {
        private static BaseTerminal terminal;
        static void Main(string[] args)
        {
            // terminal = new BaseTerminal();
            terminal = new EngineeringTerminal();
                        
            do {
                    terminal.PasswordUsage();
            } while (true);
        }
    }
}
