using System.Collections.Generic;

namespace Bifrost.Dot
{
    public class DotNode
    {
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    }
}