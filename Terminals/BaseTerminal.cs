using System;
using System.Text;

namespace Terminal.Terminals
{
    public class BaseTerminal
    {
        public string Password {get;set;}
        public int LockoutPreventionCount {get;set;}

        public bool BigRedButtonActive {get;set;}

        public virtual void PrintTerminalInstructions()
        {
            Console.WriteLine("Press A to request info");
            Console.WriteLine("Press B to set a password on this terminal");
            Console.WriteLine("Press C to prevent lockout");
            Console.WriteLine("Press D to press BIG RED BUTTON");
            Console.WriteLine("Press E to turn off your own BIG RED BUTTON");
            Console.WriteLine("Press F to check BIG RED BUTTON status");
        }

        public void InfoRequest()
        {
            Console.WriteLine("What do you want to know about the station or about science?");
            var input = Console.ReadLine();
            //!!!Replace with text to me via an API
            Console.WriteLine($"Notify director to recieve information about \"{input}\"");
        }

        public void SetPassword()
        {
            Console.WriteLine("Set a password to use this terminal. (ESC to cancel):");
            string tempPassword1 = ReadLineWithCancel();
            if(tempPassword1 == null)
                return;
            Console.WriteLine(Environment.NewLine + "Confirm password to use this terminal. (ESC to cancel):");
            string tempPassword2 = ReadLineWithCancel();
            if(tempPassword2 == null)
                return;
            if(tempPassword1 == tempPassword2)
            {
                Console.WriteLine(Environment.NewLine + "Password Set.");
                Password = tempPassword1;
            }else
            {
                Console.WriteLine(Environment.NewLine + "Passwords did not match.");
                return;
            }
        }

        public void BigRedButton(){
            Console.WriteLine("Puts up shields that prevent all movement in or out of the seats next to all terminals.");
            Console.WriteLine("No physical actions or abilities can cross the shields.");
            Console.WriteLine("However sound, light and air can.");
            Console.WriteLine("Social and emotional abilities can still cross.");
            Console.WriteLine("The alien tech laser can destroy one of the shields in one weapons use.");
            Console.WriteLine("\"y\" to activate the BIG RED BUTTON (ESC to cancel):");
            string activateButton = ReadLineWithCancel();
            if(activateButton == null)
                return;
            
            if(string.Equals(activateButton, "y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("BIG RED BUTTON active, PLEASE NOTIFY DIRECTOR!");
                BigRedButtonActive = true;
            }
        }

        public void BigRedButtonOff(){
            Console.WriteLine("You may only turn off the BIG RED BUTTON if you were the one that turned it on.");
            Console.WriteLine("\"y\" to de-activate the BIG RED BUTTON (ESC to cancel):");
            string deactivateButton = ReadLineWithCancel();
            if(deactivateButton == null)
                return;
            
            if(string.Equals(deactivateButton, "y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("BIG RED BUTTON de-activated, PLEASE NOTIFY DIRECTOR!");
                BigRedButtonActive = false;
            }

        }

        public void PreventLockout(){
            Console.WriteLine("Prevent this terminal from being disabled by the security terminal for a fixed amount of commands.");
            Console.WriteLine("How many commands from 1 to 10 would you like to prevent lockout for (ESC to cancel):");
            string preventLockoutTimesString = ReadLineWithCancel();
            if(preventLockoutTimesString == null)
                return;

            //What happens if you enter a nonstring
            int.TryParse(preventLockoutTimesString, out int preventLockoutTimes);
            if (preventLockoutTimes <= 10 && preventLockoutTimes > 0)
                LockoutPreventionCount = preventLockoutTimes;
            
            if (preventLockoutTimes > 10)
                Console.WriteLine("YOU HAVE ATTEMPTED TO CHEAT ME! *A burst of electricity jumps out of the terminal and shocks you for 1 damage.*");
        }

        public void BreakPassword()
        {
            if(Password == null)
            {
                Console.WriteLine("There is no password set at this terminal. No need to break the password.");
                return;
            }

            Console.WriteLine("Give chips to the director to attempt to break the password on this terminal. (ESC to cancel):");
            string tempPassword1 = ReadLineWithCancel();
            if(tempPassword1 == null)
                return;
            
            Console.WriteLine("Enter number of chips given to the director.");
            int.TryParse(Console.ReadLine(), out int chips);
            Random rnd = new Random();
            double attempt = chips * rnd.Next(1, 100);
            if(attempt > 100)
            {
                Console.WriteLine("Password removed");
                Password = null;
            }
            else{
                Console.WriteLine("You were unable to remove the current password");
            }
        }

        public bool EnterPassword()
        {
            Console.WriteLine("Enter the current password:");
            var attempt = Console.ReadLine();
            if(attempt == Password)
            {
                return true;
            }
            return false;
        }

        //Returns null if ESC key pressed during input.
        private static string ReadLineWithCancel()
        {
            string result = null;

            StringBuilder buffer = new StringBuilder();

            //The key is read passing true for the intercept argument to prevent
            //any characters from displaying when the Escape key is pressed.
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
            {
                Console.Write(info.KeyChar);
                buffer.Append(info.KeyChar);
                info = Console.ReadKey(true);
            } 

            if (info.Key == ConsoleKey.Enter)
            {
                result = buffer.ToString();
            }
            Console.WriteLine();

            return result;
        }
    }
}
