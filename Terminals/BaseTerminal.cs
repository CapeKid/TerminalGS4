using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Terminal.Terminals
{
    public abstract class BaseTerminal
    {
        public string Password {get;set;}
        public int LockoutPreventionCount {get;set;}
        public bool BigRedButtonActive {get;set;}
        public bool AllowNextCommand {get;set;}

        public List<Mode> CurrentModes {get;set;} = new List<Mode>();
        public Dictionary<Mode, List<Mapping>> AllModes {get;set;} = new Dictionary<Mode, List<Mapping>>();

        protected Stack<ConsoleKey> AvailableKeys {get;set;} = new Stack<ConsoleKey>();

        protected void InitAvailableKeys()
        {
            AvailableKeys.Push(ConsoleKey.Z);
            AvailableKeys.Push(ConsoleKey.Y);
            AvailableKeys.Push(ConsoleKey.X);
            AvailableKeys.Push(ConsoleKey.W);
            AvailableKeys.Push(ConsoleKey.V);
            AvailableKeys.Push(ConsoleKey.U);
            AvailableKeys.Push(ConsoleKey.T);
            AvailableKeys.Push(ConsoleKey.S);
            AvailableKeys.Push(ConsoleKey.R);
            AvailableKeys.Push(ConsoleKey.Q);
            AvailableKeys.Push(ConsoleKey.P);
            AvailableKeys.Push(ConsoleKey.O);
            AvailableKeys.Push(ConsoleKey.N);
            AvailableKeys.Push(ConsoleKey.M);
            AvailableKeys.Push(ConsoleKey.L);
            AvailableKeys.Push(ConsoleKey.K);
            AvailableKeys.Push(ConsoleKey.J);
            AvailableKeys.Push(ConsoleKey.I);
            AvailableKeys.Push(ConsoleKey.H);
            AvailableKeys.Push(ConsoleKey.G);
            AvailableKeys.Push(ConsoleKey.F);
            AvailableKeys.Push(ConsoleKey.E);
            AvailableKeys.Push(ConsoleKey.D);
            AvailableKeys.Push(ConsoleKey.C);
            AvailableKeys.Push(ConsoleKey.B);
            AvailableKeys.Push(ConsoleKey.A);
        }

        protected virtual void InitModes(){
            CurrentModes.Add(Mode.Normal);
            AllModes.Add(Mode.Normal, NormalMappings());
            AllModes.Add(Mode.Director, DirectorMappings());
            AllModes.Add(Mode.Password, PasswordMappings());
        }

        protected List<Mapping> NormalMappings(){
            return new List<Mapping>{
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to request info" ,  () => InfoRequest(), null )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to set a password on this terminal",  () => SetPassword(), null )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to prevent lockout",  () => PreventLockout(), null )) ,
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to press BIG RED BUTTON",  () => BigRedButton(), null )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn off your own BIG RED BUTTON",  () => BigRedButtonOff(), () => {return BigRedButtonActive;})),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to check BIG RED BUTTON status",  () => BigRedButtonState(), null )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to Remove Current Password",  () => RemovePassword(), () => {return !String.IsNullOrEmpty(Password);})),
                new Mapping(ConsoleKey.OemComma, new KeyFunctionDTO ( null,  () => DirectorUsage(), null ))
            };
        }
        
        protected virtual List<Mapping> DirectorMappings()
        {
            return new List<Mapping>(){ 
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to print password" ,  () => PrintPassword(), null )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to check lockout prevention",  () => PrintLockoutPreventionCount(), null ) ),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to activate big red button",  () => ActivateBigRedButton(), null ) ),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to de-activate big red button",  () => DeactivateBigRedButton(), null ) ),
                new Mapping(ConsoleKey.OemPeriod, new KeyFunctionDTO ( "Press \".\" resume normal function",  () => TerminalReadLoop(), () => {return BigRedButtonActive;}) ),
            };
        }

        protected List<Mapping> PasswordMappings()
        {
            return new List<Mapping>(){ 
                new Mapping(AvailableKeys,
                    new KeyFunctionDTO ( "Press \"{0}\" to enter password" ,  () => EnterPassword(), null, breaksPassword: false )),
                new Mapping(AvailableKeys,
                    new KeyFunctionDTO ( "Press \"{0}\" to attempt to break password security",  () => BreakPassword(), null )),
                new Mapping(ConsoleKey.OemComma, new KeyFunctionDTO ( null,  () => DirectorUsage(), null ))
            };
        }

        public void DirectorUsage()
        {
            Console.WriteLine("Accessing director functions...");
            Console.WriteLine("If you are using this and are not Seth or Megan, you are cheating and ruining the game for everyone.");
            OnlyMode(Mode.Director);
        }

        protected virtual void NormalUsage()
        {
            Console.WriteLine("Accessing normal functions...");
            OnlyMode(Mode.Normal);
        }

        public void PasswordUsage()
        {
            Console.WriteLine("Password is set, using password functions...");
            OnlyMode(Mode.Password);
        }

        private Dictionary<ConsoleKey, KeyFunctionDTO> GetCurrentMappings()
        {
            Dictionary<ConsoleKey, KeyFunctionDTO> currentMappings = new Dictionary<ConsoleKey, KeyFunctionDTO>();
            foreach(var mode in CurrentModes)
            {
                foreach(var mapping in AllModes[mode])
                {
                    currentMappings.Add(mapping.Key, mapping.KeyFunctionDTO);
                }
            }
            return currentMappings;
        }

        protected void OnlyModes(List<Mode> modes)
        {
            RemoveAllModesBut(modes);
            foreach(var mode in modes)
            {
                if (!CurrentModes.Contains(mode))
                CurrentModes.Add(mode);
            }
        }

        protected void OnlyMode(Mode mode)
        {
            RemoveAllModesBut(mode);
            if (!CurrentModes.Contains(mode))
                CurrentModes.Add(mode);
        }

        private void RemoveAllModesBut(Mode mode){
            CurrentModes.RemoveAll(x => x != Mode.Director && x != mode);
        }

        private void RemoveAllModesBut(List<Mode> modes){
            CurrentModes.RemoveAll(x => x != Mode.Director && !modes.Contains(x));
        }

        public virtual void TerminalReadLoop()
        {
            if(Password != null && !AllowNextCommand)
            {
                PasswordUsage();
            }else{
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
                if(mappingList.Key == key.Key && (mappingList.Value.AppearanceCondition == null || mappingList.Value.AppearanceCondition.Invoke()))
                {
                    mappingList.Value.Action.Invoke();
                    if(mappingList.Value.BreaksPassword)
                        AllowNextCommand = false;
                    return;
                }
            }
            Console.WriteLine("Invalid input. Press ESC to stop");
        }

        public void Toggle(string extraInfo,
        out bool thingToControl,
        bool on,
        string onString = "{extraInfo} power is ON. Notify director.",
        string offString = "{extraInfo} power is OFF. Notify director."){
            if(on)
            {
                Console.WriteLine(string.Format(onString, extraInfo));
                thingToControl = true;
            }
            else
            {
                Console.WriteLine(string.Format(offString, extraInfo));
                thingToControl = false;
            }

            NormalUsage();
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
            Console.WriteLine("Press 'y' to activate the BIG RED BUTTON (ESC to cancel):");
            char activateButton = Console.ReadKey().KeyChar;
            
            if(activateButton == 'y')
            {
                Console.WriteLine("BIG RED BUTTON active, PLEASE NOTIFY DIRECTOR!");
                BigRedButtonActive = true;
            }
        }

        public void BigRedButtonOff(){
            Console.WriteLine("You may only turn off the BIG RED BUTTON if you were the one that turned it on.");
            Console.WriteLine("Press 'y' to de-activate the BIG RED BUTTON (ESC to cancel):");
            char deactivateButton = Console.ReadKey().KeyChar;
            
            if(deactivateButton == 'y')
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
        protected static string ReadLineWithCancel()
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
