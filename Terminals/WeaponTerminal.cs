using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Terminal.Terminals
{
    public class WeaponTerminal : BaseTerminal
    {
        public WeaponTerminal() : base()
        {
            InitAvailableKeys();
            InitModes();
        }

        protected override void InitModes()
        {
            base.InitModes();
            AllModes.Add(Mode.Weapon, WeaponMappings());
            // AllModes.Add(Mode.EngineeringRoomPower, EngineeringRoomPowerMappings());
            NormalUsage();
        }

        public override void TerminalReadLoop()
        {
            Console.WriteLine("You are currently at the WEAPONS terminal.");
            base.TerminalReadLoop();
        }
        private List<Mapping> WeaponMappings()
        {
            return new List<Mapping>{
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to autofire ON. Autofire is the automatic destruction of anything that approaches the planet." ,  () => Toggle("autofire", out isRoomPowerOn, true), () => {return !isRoomPowerOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to autofire OFF.  Autofire is the automatic destruction of anything that approaches the planet." ,  () => Toggle("autofire", out isRoomPowerOn, false), () => {return isRoomPowerOn;}  )),

                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn room power ON" ,  () => Toggle("room", out isRoomPowerOn, true), () => {return !isRoomPowerOn;} )),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn room power OFF" ,  () => Toggle("room", out isRoomPowerOn, false), () => {return isRoomPowerOn;}  )),

                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn weapon power ON" ,  () => Toggle("weapon", out isWeaponPowerOn, true), () => {return !isWeaponPowerOn;} )),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn weapon power OFF" ,  () => Toggle("weapon", out isWeaponPowerOn, false), () => {return isWeaponPowerOn;}  )),

                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn medical robot power ON" ,  () => Toggle("medical robot", out isMedicalRobotPowerOn, true), () => {return !isMedicalRobotPowerOn;} )),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn medical robot power OFF" ,  () => Toggle("medical robot", out isMedicalRobotPowerOn, false), () => {return isMedicalRobotPowerOn;}  )),

                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn communications power ON" ,  () => Toggle("communications", out isCommunicationsPowerOn, true), () => {return !isCommunicationsPowerOn;} )),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn communications power OFF" ,  () => Toggle("communications", out isCommunicationsPowerOn, false), () => {return isCommunicationsPowerOn;}  )),

                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to GRANT access to the escape pod" ,  () => Toggle("escape pod", out isEscapePodOn, true), () => {return !isEscapePodOn;} )),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to REVOKE access to the escape pod" ,  () => Toggle("escape pod", out isEscapePodOn, false), () => {return isEscapePodOn;}  )),

                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to self destruct station" ,  () => SelfDestruct(), () => {return selfDestructTime == null;}  )),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to cancel self destruct station" ,  () => CancelSelfDestruct(), () => {return selfDestructTime != null;}  )),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to check self destruct timer" ,  () => PrintSelfDestruct(), () => {return selfDestructTime != null;}  )),
                };
        }

        protected override void NormalUsage()
        {
            Console.WriteLine("Accessing normal functions...");
            OnlyModes(new List<Mode> () { Mode.Normal, Mode.Weapon } );
        }

    }
}
