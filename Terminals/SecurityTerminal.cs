using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Terminal.Terminals
{
    public class SecurityTerminal : BaseTerminal
    {
        private bool isBigRedButtonsEnabled = true;

        public SecurityTerminal() : base()
        {
            InitAvailableKeys();
            InitModes();
        }

        protected override void InitModes()
        {
            base.InitModes();
            AllModes.Add(Mode.Security, SecurityMappings());
            NormalUsage();
        }

        private List<Mapping> SecurityMappings()
        {
            return new List<Mapping>{
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to lock out a terminal." , () => LockoutTerminal(), null  )),           
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to cancel all BIG RED BUTTON uses and prevent their further use." , () => ControlBigRedButtons(false), () => {return isBigRedButtonsEnabled;}  )),           
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to allow all BIG RED BUTTONS to be used again." , () => ControlBigRedButtons(true), () => {return !isBigRedButtonsEnabled;}  )),           
            
            };
        }

        private void LockoutTerminal()
        {
            Console.WriteLine("Choose a terminal to lockout. Options are COMMUNICATIONS, ENGINEERING, LIFE SUPPORT, or WEAPONS (ESC to cancel):");
            var terminal = ReadLineWithCancel();
            if(terminal == null)
                return;
            Console.WriteLine($"Locking out \"{terminal}\" terminal! Notify director.");
        }

        private void ControlBigRedButtons(bool enabled)
        {
            if(enabled){
                Console.WriteLine("BIG RED BUTTONS are ENABLED! Notify Director!");
            }else
            {
                Console.WriteLine("BIG RED BUTTONS are DISABLED! Notify Director!");
            }
            isBigRedButtonsEnabled = enabled;
        }

        public override void TerminalReadLoop()
        {
            Console.WriteLine("You are currently at the SECURITY terminal.");
            base.TerminalReadLoop();
        }

        protected override void NormalUsage()
        {
            Console.WriteLine("Accessing normal functions...");
            OnlyModes(new List<Mode> () { Mode.Normal, Mode.Security } );
            if(CurrentModes.Contains(Mode.Director))
                CurrentModes.Remove(Mode.Director);
        }
    }
}
