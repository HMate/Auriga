using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bifrost.CommonSemanticTree
{
    /// <summary>
    /// A common semantic tree contains programs in a form that is the greates common denominator of langagues.
    /// The Data context tells that what were the assigend alues of variables and outputs at a specific point of the program.
    /// </summary>
    public class CSTDataContext
    {
        public List<CSTVariable> Variables { get; } = new List<CSTVariable>();
        public List<CSTVariable> Outputs { get; } = new List<CSTVariable>();

        public CSTVariable? GetVariable(string name)
        {
            return Variables.Find(v => v.Name == name);
        }
    }
}
