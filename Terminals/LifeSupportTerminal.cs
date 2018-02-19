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
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to use life support to deal 1 damage to ALL players." , () => LifeSupportDamage(), null  )),           
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to order medical robot to attend to a player." , () => MedicalRobot(true), null  )),           
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to order medical robot to ignore a player." , () => MedicalRobot(false), null  )),           
                new Mapping(AvailableKeys, new KeyFunctionDTO ( "Press \"{0}\" to add any substance to the food." , () => AddSubstance(), null  )),           
            
            };
        }

        private void AddSubstance()
        {
            Console.WriteLine("You may add any substance to the generated food, such as deadly poison, vitamin supplements, or Ink.");
            Console.WriteLine("Type the substance you would like to add. (ESC to cancel)");
            var substance = ReadLineWithCancel();
            if(substance == null)
                return;
            Console.WriteLine($"Adding \"{substance}\" to the food. Notify director.");
        }

        private void LifeSupportDamage(){
            Console.WriteLine("The air gets thinner in the station, it becomes hard to breathe. Notify director.");
        }

        private void MedicalRobot(bool attend){
            Console.WriteLine($"Who would you like the medical robot {(attend ? "to attend?" : "deny attention to?")} (ESC to cancel)");
            var player = ReadLineWithCancel();
            if(player == null)
                return;
            
            Console.WriteLine($"{(attend ? "Attending" : "Denying attention to")} \"{player}\". Notify Director.");
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
            if(CurrentModes.Contains(Mode.Director))
                CurrentModes.Remove(Mode.Director);
        }
    }
}
