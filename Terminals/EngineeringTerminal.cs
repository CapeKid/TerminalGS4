using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Terminals
{
    public class EngineeringTerminal : BaseTerminal
    {
        private bool isPowerOn = true;

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
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to control room power" ,  () => PowerUsage(), null )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn room power ON" ,  () => RoomPower(true), () => {return !isPowerOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn room power OFF" ,  () => RoomPower(false), () => {return isPowerOn;}  )),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to control weapon power",  () => SetPassword(), null )),
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to control medical robot power",  () => PreventLockout(), null )) ,
                // new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to control communications power",  () => BigRedButton(), null )),
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

        public void RoomPower(bool on){
            if(on)
            {
                Console.WriteLine("Room power is ON. Notify director.");
                isPowerOn = true;
            }
            else
            {
                Console.WriteLine("Room power is OFF. Notify director.");
                isPowerOn = false;
            }

            NormalUsage();
        }
    }
}
