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

        private int totalPlanets = 358;
        private int upPlanets = 44;
        private int cerianPlanets = 37;

        private Timer weaponTimer = new Timer(new TimeSpan(0, 1, 0).TotalMilliseconds);
        private int remainingFireCount = 10;
        private DateTime timerStartTime;

        public WeaponTerminal() : base()
        {
            weaponTimer.Start();
            timerStartTime = DateTime.Now;
            weaponTimer.Elapsed += HandleResetFireCount;
            InitAvailableKeys();
            InitModes();
        }

        private void HandleResetFireCount(object sender, ElapsedEventArgs e)
        {
            if(remainingFireCount != 10)
            {
                timerStartTime = DateTime.Now;
                Console.WriteLine("Weapon ready to fire!");
                remainingFireCount = 10;
            }
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
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to fire on a Union of Planets planet." ,  () => FireWeaponAtTarget(Contingent.UP), () => {return upPlanets > 0;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to fire on a Cerian planet." ,  () => FireWeaponAtTarget(Contingent.Cerian), () => {return cerianPlanets > 0;}  )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to fire on an unaffiliated planet." ,   () => FireWeaponAtTarget(Contingent.Unaffiliated), () => {return totalPlanets > 0;}  )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to fire on some other target." , () => FireWeaponAtTarget(Contingent.Other), null  )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to return to go back to other terminal options." , () => NormalUsage(), null  )),           
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

        private void FireWeaponAtTarget(Contingent contingent)
        {
            if(!IsWeaponReady())
            {
                Console.WriteLine($"The weapon is currently cooling down, you must wait {(DateTime.Now - timerStartTime).Seconds} seconds until it is ready to fire.");
                return;
            }

            if(contingent == Contingent.Unaffiliated
             || contingent == Contingent.Cerian
             || contingent == Contingent.UP)
                totalPlanets--;
            
            if(contingent == Contingent.UP)
                upPlanets--;

            if(contingent == Contingent.Cerian)
                cerianPlanets--;

            string target = String.Empty;
            if(contingent == Contingent.Other)
            {
                Console.WriteLine("Notify director what you would like to blow up, then enter it here (ESC to cancel):");
                target = ReadLineWithCancel();
                if(target == null)
                    return;
            }
            
            remainingFireCount--;
            RemainingPlanets(contingent);
            Console.WriteLine("You have blown up " + (contingent == Contingent.Other ? ("\"" + target + "\"!") : DescribeTarget(contingent)));
        }

        private void RemainingPlanets(Contingent contingent){
            if(contingent == Contingent.UP)
                Console.WriteLine("There are " + upPlanets + " Union of Planets planets remaining.");
            if(contingent == Contingent.Cerian)
                Console.WriteLine("There are " + cerianPlanets + " Cerian planets remaining.");
            if(contingent == Contingent.UP || contingent == Contingent.Cerian || contingent == Contingent.Unaffiliated)
                Console.WriteLine("There are " + totalPlanets + " total planets remaining.");
        
        }
        
        private bool IsWeaponReady()
        {
            return remainingFireCount > 0;
        }

        private string DescribeTarget(Contingent contingent)
        {
            if(contingent == Contingent.Cerian)
                return "a Cerian planet!";
            if(contingent == Contingent.UP)
                return "a Union of Planets planet!";
            if(contingent == Contingent.Unaffiliated)
                return "a planet!";
            else
            {
                return "the thing you described!"   ;    
            }
        }
        protected override void NormalUsage()
        {
            Console.WriteLine("Accessing normal functions...");
            OnlyModes(new List<Mode> () { Mode.Normal, Mode.Weapon } );
            if(CurrentModes.Contains(Mode.Director))
                CurrentModes.Remove(Mode.Director);
        }
        
        private void FireWeaponUsage()
        {
            Console.WriteLine("Accessing weapon fire...");
            OnlyModes(new List<Mode> () { Mode.Normal, Mode.FireWeapon } );
        }
    }
}
