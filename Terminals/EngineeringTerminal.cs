using System;
using System.Text;

namespace Terminal.Terminals
{
    public class EngineeringTerminal : BaseTerminal
    {
        public override void PrintTerminalInstructions()
        {
            base.PrintTerminalInstructions();
        }

        public override void NormalTerminalUsage()
        {
            PrintTerminalInstructions();
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            Options(key);
        }        

        public override void Options(ConsoleKeyInfo key){
            base.Options(key);
        }
    }
}
