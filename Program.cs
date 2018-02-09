using System;
using Terminal.Terminals;

namespace Terminal
{
    class Program
    {
        private static BaseTerminal terminal;
        static void Main(string[] args)
        {
            terminal = new BaseTerminal();
                        
            do {
                    if(terminal.Password != null)
                    {
                        Console.WriteLine("There is a password set on this terminal");
                        Console.WriteLine("Press A to enter password");
                        Console.WriteLine("Press B to attempt to break password security");
                        
                        ConsoleKeyInfo key = Console.ReadKey();
                        Console.WriteLine();
                        switch(key.Key)
                        {
                            case ConsoleKey.A:
                                if(terminal.EnterPassword()){
                                    NormalTerminalUsage();
                                }
                                break;
                            case ConsoleKey.B:
                                terminal.BreakPassword();
                                break;
                        }
                    }
                    else{
                        NormalTerminalUsage();   
                    }
            } while (true);
        }

        private static void NormalTerminalUsage()
        {
            if(terminal.LockoutPreventionCount > 0)
                terminal.LockoutPreventionCount--;
            terminal.PrintTerminalInstructions();
            
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            switch(key.Key)
            {
                case ConsoleKey.A:
                    terminal.InfoRequest();
                    break;
                case ConsoleKey.B:
                    terminal.SetPassword();
                    break;
                case ConsoleKey.C:
                    terminal.PreventLockout();
                    break;
                case ConsoleKey.D:
                    terminal.BigRedButton();
                    break;
                case ConsoleKey.E:
                    terminal.BigRedButtonOff();
                    break;
                case ConsoleKey.F:
                    string toOnOff = terminal.BigRedButtonActive ? "ON" : "OFF";
                    Console.WriteLine($"Current big red button status is \"{toOnOff}\"");
                    break;
                case ConsoleKey.OemComma:
                    Console.WriteLine("Accessing director functions...");
                    Console.WriteLine("If you are using this and are not Seth or Megan, you are cheating and ruining the game for everyone.");
                    DirectorUsage();
                    break;
                default:
                    Console.WriteLine("Invalid input. Press ESC to stop");
                    break;
            }
        }

        private static void DirectorUsage()
        {
            Console.WriteLine("Press A to print password");
            Console.WriteLine("Press B to check lockout prevention");
            Console.WriteLine("Press C to activate big red button");
            Console.WriteLine("Press D to de-activate big red button");
            Console.WriteLine("Press , resume normal function");
            
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            switch(key.Key)
            {
                case ConsoleKey.A:
                    Console.WriteLine($"Current password is {terminal.Password}");
                    break;
                case ConsoleKey.B:
                    Console.WriteLine($"Current lockout prevention count is {terminal.LockoutPreventionCount}");
                    break;
                case ConsoleKey.C:
                    terminal.BigRedButtonActive = true;
                    break;
                case ConsoleKey.D:
                    terminal.BigRedButtonActive = false;
                    break;
                case ConsoleKey.OemComma:
                    Console.WriteLine("Resuming normal functions...");
                    NormalTerminalUsage();
                    break;
                default:
                    Console.WriteLine("Invalid input. Press ESC to stop");
                    break;
            }
        }
    }
}
