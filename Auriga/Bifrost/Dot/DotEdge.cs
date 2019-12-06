using System.Collections.Generic;

namespace Bifrost.Dot
{
    public class DotEdge
    {
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    }
}