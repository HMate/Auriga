using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Auriga.Models
{
    public class Graph
    {
        public ICollection<Node> Nodes { get; } = new List<Node>();
        public ICollection<Edge> Edges { get; } = new List<Edge>();

        internal void AddNode(Guid id, string nodeName, Point pos)
        {
            Nodes.Add(new Node(id, nodeName, pos));
        }

        internal void AddEdge(Guid startId, Guid endId)
        {
            Edges.Add(new Edge(startId, endId));
        }
    }
}
