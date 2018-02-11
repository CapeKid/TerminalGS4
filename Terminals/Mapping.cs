using System;
using System.Collections.Generic;

namespace Terminal.Terminals
{
    public class Mapping{
        public ConsoleKey Key {get;set;}
        public KeyFunctionDTO KeyFunctionDTO {get;set;}

        public Mapping (Stack<ConsoleKey> availableKeys, KeyFunctionDTO keyFunctionDTO)
        {
            var currentKey = availableKeys.Pop();
            var keyNameReplaced = String.Format(keyFunctionDTO.Instruction, currentKey);
            keyFunctionDTO.Instruction = keyNameReplaced;
            Key = currentKey;
            KeyFunctionDTO = keyFunctionDTO;
        }

        public Mapping (ConsoleKey key, KeyFunctionDTO keyFunctionDTO)
        {
            Key = key;
            KeyFunctionDTO = keyFunctionDTO;
        }
    }
}