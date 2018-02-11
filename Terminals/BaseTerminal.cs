using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Terminal.Terminals
{
    public class BaseTerminal
    {
        public string Password {get;set;}
        public int LockoutPreventionCount {get;set;}
        public bool BigRedButtonActive {get;set;}
        public bool AllowNextCommand {get;set;}

        public List<Mode> CurrentModes {get;set;}
        public Dictionary<Mode, Dictionary<List<ConsoleKey>, KeyFunctionDTO>> AllModes {get;set;}

        public BaseTerminal(){
            InitModes();
        }

        private void InitModes(){
            CurrentModes = new List<Mode>();
            CurrentModes.Add(Mode.Normal);
            AllModes = new Dictionary<Mode, Dictionary<List<ConsoleKey>, KeyFunctionDTO>>(){
                {Mode.Normal, NormalMappings()},
                {Mode.Director, DirectorMappings()},
                {Mode.Password, PasswordMappings()}
                };
        }

        private Dictionary<List<ConsoleKey>, KeyFunctionDTO> NormalMappings(){
            return new Dictionary<List<ConsoleKey>, KeyFunctionDTO>(){ 
                { new List<ConsoleKey> { ConsoleKey.A },
                    new KeyFunctionDTO ( "Press A to request info" ,  () => InfoRequest(), null ) },
                { new List<ConsoleKey> { ConsoleKey.B },
                    new KeyFunctionDTO ( "Press B to set a password on this terminal",  () => SetPassword(), null ) },
                { new List<ConsoleKey> { ConsoleKey.C },
                    new KeyFunctionDTO ( "Press C to prevent lockout",  () => PreventLockout(), null ) },
                { new List<ConsoleKey> { ConsoleKey.D },
                    new KeyFunctionDTO ( "Press D to press BIG RED BUTTON",  () => BigRedButton(), null ) },
                { new List<ConsoleKey> { ConsoleKey.E },
                    new KeyFunctionDTO ( "Press E to turn off your own BIG RED BUTTON",  () => BigRedButtonOff(), () => {return BigRedButtonActive;}) },
                { new List<ConsoleKey> { ConsoleKey.F },
                    new KeyFunctionDTO ( "Press F to check BIG RED BUTTON status",  () => BigRedButtonState(), null ) },
                { new List<ConsoleKey> { ConsoleKey.Z },
                    new KeyFunctionDTO ( "Press Z to Remove Current Password",  () => RemovePassword(), () => {return !String.IsNullOrEmpty(Password);}) },
                { new List<ConsoleKey> { ConsoleKey.Oem3, ConsoleKey.OemComma },
                    new KeyFunctionDTO ( null,  () => DirectorUsage(), null ) },
            };
        }
        
        public Dictionary<List<ConsoleKey>, KeyFunctionDTO> DirectorMappings()
        {
            return new Dictionary<List<ConsoleKey>, KeyFunctionDTO>(){ 
                { new List<ConsoleKey> { ConsoleKey.A },
                    new KeyFunctionDTO ( "Press A to print password" ,  () => PrintPassword(), null ) },
                { new List<ConsoleKey> { ConsoleKey.B },
                    new KeyFunctionDTO ( "Press B to check lockout prevention",  () => PrintLockoutPreventionCount(), null ) },
                { new List<ConsoleKey> { ConsoleKey.C },
                    new KeyFunctionDTO ( "Press C to activate big red button",  () => ActivateBigRedButton(), null ) },
                { new List<ConsoleKey> { ConsoleKey.D },
                    new KeyFunctionDTO ( "Press D to de-activate big red button",  () => DeactivateBigRedButton(), null ) },
                { new List<ConsoleKey> { ConsoleKey.Oem3, ConsoleKey.OemComma },
                    new KeyFunctionDTO ( "Press , resume normal function",  () => TerminalReadLoop(), () => {return BigRedButtonActive;}) },
            };
        }

        public Dictionary<List<ConsoleKey>, KeyFunctionDTO> PasswordMappings()
        {
            return new Dictionary<List<ConsoleKey>, KeyFunctionDTO>(){ 
                { new List<ConsoleKey> { ConsoleKey.A },
                    new KeyFunctionDTO ( "Press A to enter password" ,  () => EnterPassword(), null ) },
                { new List<ConsoleKey> { ConsoleKey.B },
                    new KeyFunctionDTO ( "Press B to attempt to break password security",  () => BreakPassword(), null ) },
                { new List<ConsoleKey> { ConsoleKey.Oem3, ConsoleKey.OemComma },
                    new KeyFunctionDTO ( null,  () => DirectorUsage(), null ) },
            };
        }

        public void DirectorUsage()
        {
            Console.WriteLine("Accessing director functions...");
            Console.WriteLine("If you are using this and are not Seth or Megan, you are cheating and ruining the game for everyone.");
            CurrentModes.Clear();
            if (!CurrentModes.Contains(Mode.Director))
                CurrentModes.Add(Mode.Director);
        }

        public void NormalUsage()
        {
            Console.WriteLine("Accessing normal functions...");
            if (CurrentModes.Contains(Mode.Password))
                CurrentModes.Remove(Mode.Password);
            if (!CurrentModes.Contains(Mode.Normal))
                CurrentModes.Add(Mode.Normal);
        }

        public void PasswordUsage()
        {
            Console.WriteLine("Password is set, using password functions...");
            if (CurrentModes.Contains(Mode.Normal))
                CurrentModes.Remove(Mode.Normal);
            if (!CurrentModes.Contains(Mode.Password))
                CurrentModes.Add(Mode.Password);
        }

        private Dictionary<List<ConsoleKey>, KeyFunctionDTO> GetCurrentMappings()
        {
            Dictionary<List<ConsoleKey>, KeyFunctionDTO> currentMappings = new Dictionary<List<ConsoleKey>, KeyFunctionDTO>();
            foreach(var mode in CurrentModes)
            {
                foreach(var mapping in AllModes[mode])
                {
                    currentMappings.Add(mapping.Key, mapping.Value);
                }
            }
            return currentMappings;
        }

        public virtual void TerminalReadLoop()
        {
            if(Password != null && !AllowNextCommand)
            {
                PasswordUsage();
            }
            else{
                NormalUsage();   
            }

            PrintTerminalInstructions();
            
            ConsoleKeyInfo key = Console.ReadKey();
            if(LockoutPreventionCount > 0)
                LockoutPreventionCount--;

            Console.WriteLine();
            Options(key);
        }
        public virtual void PrintTerminalInstructions()
        {
            foreach(var mappingListValue in GetCurrentMappings().Values){
                if((mappingListValue.AppearanceCondition == null || mappingListValue.AppearanceCondition.Invoke()) && mappingListValue.Instruction != null)
                    Console.WriteLine(mappingListValue.Instruction);
            }
        }

        public virtual void Options(ConsoleKeyInfo key){
            foreach(var mappingList in GetCurrentMappings()){
                if(mappingList.Key.Contains(key.Key))
                {
                    mappingList.Value.Action.Invoke();
                    return;
                }
            }
            Console.WriteLine("Invalid input. Press ESC to stop");
        }

        private void BigRedButtonState(){
            string toOnOff = BigRedButtonActive ? "ON" : "OFF";
            Console.WriteLine($"Current big red button status is \"{toOnOff}\"");
        }

        public void RemovePassword()
        {
            Console.WriteLine("Removing Password...");
            Password = null;
        }

        public void InfoRequest()
        {
            Console.WriteLine("What do you want to know about the station or about science?");
            var input = Console.ReadLine();
            //!!!Replace with text to me via an API
            Console.WriteLine($"Notify director to recieve information about \"{input}\"");
        }
        private void PrintPassword()
        {
            Console.WriteLine($"Current password is {Password}");
        }

        private void PrintLockoutPreventionCount()
        {
            Console.WriteLine($"Current lockout prevention count is {LockoutPreventionCount}");
        }

        public void SetPassword()
        {
            Console.WriteLine("Choose a password to use for this terminal. (ESC to cancel):");
            string tempPassword1 = ReadLineWithCancel();
            if(tempPassword1 == null)
                return;
            if(tempPassword1 == string.Empty)
            {
                Console.WriteLine("Entered an empty password!");
                return;
            }
            Console.WriteLine(Environment.NewLine + "Confirm password to use for this terminal. (ESC to cancel):");
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

        public void ActivateBigRedButton(){
            BigRedButtonActive = true;
        }
        public void DeactivateBigRedButton(){
            BigRedButtonActive = false;
        }

        public void BigRedButton(){
            Console.WriteLine("Puts up shields that prevent all movement in or out of the seats next to all terminals.");
            Console.WriteLine("No physical actions or abilities can cross the shields.");
            Console.WriteLine("However sound, light, and air can.");
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

        public void EnterPassword()
        {
            Console.WriteLine("Enter the current password:");
            var attempt = Console.ReadLine();
            if(string.Equals(attempt,Password))
            {
                AllowNextCommand = true;
            }
            else{
                Console.WriteLine("You entered the wrong password!");
                AllowNextCommand = false;
            }
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
