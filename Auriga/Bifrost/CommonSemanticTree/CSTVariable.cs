﻿using System.Collections.Generic;

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

        public string Name { get; set; }
        public CSTType Type { get; set; }
        public string Value { get; set; } = "";
        public CSTBinding Binding { get; set; } = CSTBinding.Constant;
    }
}