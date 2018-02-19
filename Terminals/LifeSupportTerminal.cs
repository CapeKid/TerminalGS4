using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Terminal.Terminals
{
    public class LifeSupportTerminal : BaseTerminal
    {
        public LifeSupportTerminal() : base()
        {
            InitAvailableKeys();
            InitModes();
        }

        protected override void InitModes()
        {
            base.InitModes();
            AllModes.Add(Mode.LifeSupport, LifeSupportMappings());
            NormalUsage();
        }

        private List<Mapping> LifeSupportMappings()
        {
            return new List<Mapping>{
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to use life support to deal 1 damage to ALL players." , () => NormalUsage(), null  )),           
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to order medical robot to attend to a player." , () => NormalUsage(), null  )),           
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to order medical robot to ignore a player." , () => NormalUsage(), null  )),           
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to use life support to deal 1 damage to ALL players." , () => NormalUsage(), null  )),           
            
            };
        }

        public override void TerminalReadLoop()
        {
            Console.WriteLine("You are currently at the WEAPONS terminal.");
            base.TerminalReadLoop();
        }

        protected override void NormalUsage()
        {
            Console.WriteLine("Accessing normal functions...");
            OnlyModes(new List<Mode> () { Mode.Normal, Mode.LifeSupport } );
        }
    }
}
