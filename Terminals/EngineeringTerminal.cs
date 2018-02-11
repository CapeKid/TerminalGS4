using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Terminals
{
    public class EngineeringTerminal : BaseTerminal
    {
        // private Dictionary<List<ConsoleKey>, (string instruction, Action action)> mappings;

        public EngineeringTerminal() : base()
        {
            InitMappings();
        }

        private void InitMappings()
        {
            
        }

        public override void PrintTerminalInstructions()
        {
            // Console.WriteLine("You are currently at the ENGINEERING terminal.");
            // Console.WriteLine("Press G to control room power");
            // Console.WriteLine("Press H to control weapon power");
            // Console.WriteLine("Press I to control medical robot power");
            // Console.WriteLine("Press J to control communications power");
            // Console.WriteLine("Press K to self destruct station");
            // Console.WriteLine("Press L to grant or revoke access to the escape pod");
            //!!!Only if self destruct active
            //Console.WriteLine("Press M to check self destruct timer");
            //Console.WriteLine("Press N to cancel self destruct station");
            base.PrintTerminalInstructions();
        }

        // public override void NormalTerminalUsage()
        // {
        //     PrintTerminalInstructions();
        //     ConsoleKeyInfo key = Console.ReadKey();
        //     Console.WriteLine();
        //     Options(key);
        // }  

        public override void Options(ConsoleKeyInfo key){
            base.Options(key);
        }
    }
}
