using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Terminal.Terminals
{
    public class WeaponTerminal : BaseTerminal
    {
        private bool isAutofireOn = true;

        public WeaponTerminal() : base()
        {
            InitAvailableKeys();
            InitModes();
        }

        protected override void InitModes()
        {
            base.InitModes();
            AllModes.Add(Mode.Weapon, WeaponMappings());
            AllModes.Add(Mode.FireWeapon, FireWeaponMappings());
            NormalUsage();
        }

        private List<Mapping> FireWeaponMappings()
        {
            return new List<Mapping>{
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "There are X Union of Planets planets remaining. Press \"{0}\" to fire on a Union of Planets target." ,  () => Toggle("Autofire", out isAutofireOn, true, "{0} is ON. Notify director!",  "{0} is OFF. Notify director!"), () => {return !isAutofireOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "There are X Cerian planets remaining. Press \"{0}\" to fire on a Cerian target." ,  () => Toggle("Autofire", out isAutofireOn, false, "{0} is ON. Notify director!",  "{0} is OFF. Notify director!"), () => {return isAutofireOn;}  )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "There are X planets total remaining. Press \"{0}\" to fire on an unaffiliated planet." ,  () => Toggle("Autofire", out isAutofireOn, false, "{0} is ON. Notify director!",  "{0} is OFF. Notify director!"), () => {return isAutofireOn;}  )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to fire on some other target." ,  () => Toggle("Autofire", out isAutofireOn, false, "{0} is ON. Notify director!",  "{0} is OFF. Notify director!"), () => {return isAutofireOn;}  )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to return to go back to other terminal options." ,  () => NormalUsage(), null  )),
                
            };
        }

        public override void TerminalReadLoop()
        {
            Console.WriteLine("You are currently at the WEAPONS terminal.");
            base.TerminalReadLoop();
        }
        private List<Mapping> WeaponMappings()
        {
            return new List<Mapping>{
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to autofire ON. Autofire is the automatic destruction of anything that approaches the planet." ,  () => Toggle("Autofire", out isAutofireOn, true, "{0} is ON. Notify director!",  "{0} is OFF. Notify director!"), () => {return !isAutofireOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to autofire OFF.  Autofire is the automatic destruction of anything that approaches the planet." ,  () => Toggle("Autofire", out isAutofireOn, false, "{0} is ON. Notify director!",  "{0} is OFF. Notify director!"), () => {return isAutofireOn;}  )),
                
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to fire on a target. Check with ENGINEERING terminal to determine if the weapon power is on." ,  () => FireWeaponUsage(), null, breaksPassword: false  )),
                
                };
        }

        protected override void NormalUsage()
        {
            Console.WriteLine("Accessing normal functions...");
            OnlyModes(new List<Mode> () { Mode.Normal, Mode.Weapon } );
        }
        
        private void FireWeaponUsage()
        {
            Console.WriteLine("Accessing weapon fire...");
            OnlyModes(new List<Mode> () { Mode.Normal, Mode.FireWeapon } );
        }
    }
}
