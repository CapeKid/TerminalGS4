﻿using System;
using Terminal.Terminals;

namespace Terminal
{
    class Program
    {
        private static BaseTerminal terminal;
        static void Main(string[] args)
        {
            //!!!Add code to choose terminal type
            terminal = new SecurityTerminal();
                        
            do {
                    terminal.TerminalReadLoop();
            } while (true);
        }
    }
}
