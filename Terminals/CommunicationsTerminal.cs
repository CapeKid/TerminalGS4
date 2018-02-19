using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Terminal.Terminals
{
    public class CommunicationsTerminal : BaseTerminal
    {
        public CommunicationsTerminal() : base()
        {
            InitAvailableKeys();
            InitModes();
        }

        protected override void InitModes()
        {
            base.InitModes();
            AllModes.Add(Mode.Communications, CommunicationsMappings());
            NormalUsage();
        }

        private List<Mapping> CommunicationsMappings()
        {
            return new List<Mapping>{
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to send a message." , () => SendMessage(), null  )),           
            
            };
        }

        private void SendMessage()
        {
            Console.WriteLine("Send a short voice and/or video message to another facility's computer or to another star system.");
            Console.WriteLine("The latter is subject to a delay of years. Type the message you would like to send: (ESC to cancel)");
            var message = ReadLineWithCancel();
            if(message == null)
                return;
            Console.WriteLine($"Tell or show message \"{message}\" to the director. Notify director.");
        }

        public override void TerminalReadLoop()
        {
            Console.WriteLine("You are currently at the COMMUNICATIONS terminal.");
            base.TerminalReadLoop();
        }

        protected override void NormalUsage()
        {
            Console.WriteLine("Accessing normal functions...");
            OnlyModes(new List<Mode> () { Mode.Normal, Mode.Communications } );
            if(CurrentModes.Contains(Mode.Director))
                CurrentModes.Remove(Mode.Director);
        }
    }
}
