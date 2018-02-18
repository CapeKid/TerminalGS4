using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Terminal.Terminals
{
    public class EngineeringTerminal : BaseTerminal
    {
        private bool isRoomPowerOn = true;
        private bool isWeaponPowerOn = true;
        private bool isMedicalRobotPowerOn = true;
        private bool isCommunicationsPowerOn = true;
        private bool isEscapePodOn = true;
        private Stopwatch selfDestructTime = null;
        private static Timer selfDestructEvent = null;
        private TimeSpan oneHour = new TimeSpan(1,0,0); 

        public EngineeringTerminal() : base()
        {
            InitAvailableKeys();
            InitModes();
        }

        protected override void InitModes()
        {
            base.InitModes();
            AllModes.Add(Mode.Engineering, EngineeringMappings());
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
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn room power ON" ,  () => Toggle("room", out isRoomPowerOn, true), () => {return !isRoomPowerOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn room power OFF" ,  () => Toggle("room", out isRoomPowerOn, false), () => {return isRoomPowerOn;}  )),

                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn weapon power ON" ,  () => Toggle("weapon", out isWeaponPowerOn, true), () => {return !isWeaponPowerOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn weapon power OFF" ,  () => Toggle("weapon", out isWeaponPowerOn, false), () => {return isWeaponPowerOn;}  )),

                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn medical robot power ON" ,  () => Toggle("medical robot", out isMedicalRobotPowerOn, true), () => {return !isMedicalRobotPowerOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn medical robot power OFF" ,  () => Toggle("medical robot", out isMedicalRobotPowerOn, false), () => {return isMedicalRobotPowerOn;}  )),

                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn communications power ON" ,  () => Toggle("communications", out isCommunicationsPowerOn, true), () => {return !isCommunicationsPowerOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to turn communications power OFF" ,  () => Toggle("communications", out isCommunicationsPowerOn, false), () => {return isCommunicationsPowerOn;}  )),

                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to GRANT access to the escape pod" ,  () => Toggle("escape pod", out isEscapePodOn, true, "Escape pod access is ON. Notify director.", "Escape pod access is OFF. Notify director."), () => {return !isEscapePodOn;} )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to REVOKE access to the escape pod" ,  () => Toggle("escape pod", out isEscapePodOn, false, "Escape pod access is ON. Notify director.", "Escape pod access is OFF. Notify director."), () => {return isEscapePodOn;}  )),

                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to self destruct station" ,  () => SelfDestruct(), () => {return selfDestructTime == null;}  )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to cancel self destruct station" ,  () => CancelSelfDestruct(), () => {return selfDestructTime != null;}  )),
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to check self destruct timer" ,  () => PrintSelfDestruct(), () => {return selfDestructTime != null;}  )),
                };
        }

        protected override void NormalUsage()
        {
            Console.WriteLine("Accessing normal functions...");
            OnlyModes(new List<Mode> () { Mode.Normal, Mode.Engineering } );
        }

        private void PrintSelfDestruct()
        {
            var timeRemaining = oneHour - selfDestructTime.Elapsed;
            Console.WriteLine($"There is {timeRemaining.Hours} hours, {timeRemaining.Minutes} minutes, {timeRemaining.Seconds} seconds, {timeRemaining.Milliseconds} milliseconds remaining before self destruct.");
        }
        
        private void CancelSelfDestruct()
        {
            Console.WriteLine("Self destruct canceled...");
            selfDestructTime = null;
            selfDestructEvent.Stop();
            selfDestructEvent = null;
        }

        private void SelfDestruct()
        {
            Console.WriteLine("Press 'y' to force station to self destruct in 1 hour!");
            char confirm = Console.ReadKey().KeyChar;
            if(confirm == 'y')
            {
                selfDestructTime = new Stopwatch();
                selfDestructTime.Start();
                selfDestructEvent = new Timer(oneHour.TotalMilliseconds);
                selfDestructEvent.Elapsed += HandleStationExplodes;
                selfDestructEvent.Start();
                
                Console.WriteLine("SELF DESTRUCT ACTIVE! NOTIFY DIRECTOR!");
                return;
            }
        }
        
        public static void HandleStationExplodes(object sender, ElapsedEventArgs e)
        {
            selfDestructEvent.Stop();
            Console.WriteLine("STATION EXPLODES! You dead.");
            Console.WriteLine("STATION EXPLODES! You dead.");
            Console.WriteLine("STATION EXPLODES! You dead.");
            Console.WriteLine("STATION EXPLODES! You dead.");
            Console.WriteLine("STATION EXPLODES! You dead.");
            Console.WriteLine("STATION EXPLODES! You dead.");
            Console.WriteLine("STATION EXPLODES! You dead.");
            Console.WriteLine("STATION EXPLODES! You dead.");
            selfDestructEvent = null;
        }
    }
}
