using System;
using System.Collections.Generic;
using System.Text;

namespace Bifrost.Dot
{
    public class DotGraph
    {
        public Dictionary<string, DotNode> Nodes { get; } = new Dictionary<string, DotNode>();
        public Dictionary<(string, string), DotEdge> Edges { get; } = new Dictionary<(string, string), DotEdge>();

        public bool IsDirected = false;

        /// <summary>
        /// A strict graph can only have a single edge between nodes.
        /// </summary>
        public bool IsStrict = false;
    }
}
