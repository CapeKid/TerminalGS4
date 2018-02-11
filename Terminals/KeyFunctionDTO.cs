using System;

namespace Terminal.Terminals
{
    public class KeyFunctionDTO{
        public string Instruction {get;set;}
        public Action Action {get;set;}
        public Func<bool> AppearanceCondition {get;set;}

        public KeyFunctionDTO (string instruction, Action action, Func<bool> appearanceCondition = null)
        {
            Instruction = instruction;
            Action = action;
            AppearanceCondition = appearanceCondition;
        }
    }
}