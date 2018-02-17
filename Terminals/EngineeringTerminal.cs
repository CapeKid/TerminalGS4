using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Terminals
{
    public class EngineeringTerminal : BaseTerminal
    {
        private bool isRoomPowerOn = true;
        private bool isWeaponPowerOn = true;
        private bool isMedicalRobotPowerOn = true;
        private bool isCommunicationsPowerOn = true;

        public EngineeringTerminal() : base()
        {
            InitAvailableKeys();
            InitModes();
        }

        protected override void InitModes()
        {
            base.InitModes();
            AllModes.Add(Mode.Engineering, EngineeringMappings());
            // AllModes.Add(Mode.EngineeringRoomPower, EngineeringRoomPowerMappings());
            NormalUsage();
        }

        public override void TerminalReadLoop()
        {
            Console.WriteLine("You are currently at the ENGINEERING terminal.");
            base.TerminalReadLoop();
        }
        private List<Mapping> EngineeringMappings()
        {
            return new List<Mapping>{
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn room power ON" ,  () => Power("room", out isRoomPowerOn, true), () => {return !isRoomPowerOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn room power OFF" ,  () => Power("room", out isRoomPowerOn, false), () => {return isRoomPowerOn;}  )),

                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn weapon power ON" ,  () => Power("weapon", out isWeaponPowerOn, true), () => {return !isWeaponPowerOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn weapon power OFF" ,  () => Power("weapon", out isWeaponPowerOn, false), () => {return isWeaponPowerOn;}  )),

                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn medical robot power ON" ,  () => Power("medical robot", out isMedicalRobotPowerOn, true), () => {return !isMedicalRobotPowerOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn medical robot power OFF" ,  () => Power("medical robot", out isMedicalRobotPowerOn, false), () => {return isMedicalRobotPowerOn;}  )),

                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn communications power ON" ,  () => Power("medical robot", out isCommunicationsPowerOn, true), () => {return !isCommunicationsPowerOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn communications power OFF" ,  () => Power("medical robot", out isCommunicationsPowerOn, false), () => {return isCommunicationsPowerOn;}  )),

                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to self destruct station",  () => BigRedButtonOff(), () => {return BigRedButtonActive;})),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to grant or revoke access to the escape pod",  () => BigRedButtonState(), null )),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to check self destruct timer",  () => RemovePassword(), () => {return !String.IsNullOrEmpty(Password);})),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to cancel self destruct station",  () => RemovePassword(), () => {return !String.IsNullOrEmpty(Password);})),
            };
        }

        protected override void NormalUsage()
        {
            Console.WriteLine("Accessing normal functions...");
            OnlyModes(new List<Mode> () { Mode.Normal, Mode.Engineering } );
        }

        // private List<Mapping> EngineeringRoomPowerMappings()
        // {
        //     return new List<Mapping>{
        //         new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn room power ON" ,  () => RoomPower(true), () => {return !isPowerOn;} )),
        //         new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn room power OFF" ,  () => RoomPower(false), () => {return isPowerOn;}  )),
        //     };
        // }

        // public void PowerUsage()
        // {
        //     OnlyMode(Mode.EngineeringRoomPower);
        // }

        public void Power(string type, out bool thingToControl, bool on){
            if(on)
            {
                Console.WriteLine($"{type} power is ON. Notify director.");
                thingToControl = true;
            }
            else
            {
                Console.WriteLine($"{type} power is OFF. Notify director.");
                thingToControl = false;
            }

            NormalUsage();
        }
    }
}
