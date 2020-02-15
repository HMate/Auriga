using System.Collections.Generic;

namespace Bifrost.CommonSemanticTree
{
    public class CSTVariable
    {
        public CSTVariable(string name, CSTType type, string initValue = "")
        {
            Name = name;
            Type = type;
            Value = initValue;
        }

        public string Name { get; }
        public CSTType Type { get; }
        public string Value { get; set; }
    }
}