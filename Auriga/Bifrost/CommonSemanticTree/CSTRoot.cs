using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bifrost.CommonSemanticTree
{
    /// <summary>
    /// A common semantic tree contains programs in a form that is the greates common denominator of langagues
    /// </summary>
    public class CSTRoot
    {
        public List<CSTVariable> Variables { get; set; } = new List<CSTVariable>();
    }
}
