using System;
using System.Collections.Generic;
using System.Text;

namespace Bifrost.Dot
{
    public class DotGraph
    {
        public IDictionary<string, DotNode> Nodes { get; } = new Dictionary<string, DotNode>();
        public IDictionary<(string, string), List<DotEdge>> Edges { get; } = new Dictionary<(string, string), List<DotEdge>>();
        public IDictionary<string, string> GraphAttributes { get; } = new Dictionary<string, string>();
        public IDictionary<string, string> NodeAttributes { get; } = new Dictionary<string, string>();

        public bool IsDirected = false;

        /// <summary>
        /// A strict graph can only have a single edge between nodes.
        /// </summary>
        public bool IsStrict = false;
    }
}
