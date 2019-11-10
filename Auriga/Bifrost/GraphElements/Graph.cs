using System;
using System.Collections.Generic;
using System.Windows;

namespace Bifrost.GraphElements
{
    public class Graph
    {
        public ICollection<Node> Nodes { get; } = new List<Node>();
        public ICollection<Edge> Edges { get; } = new List<Edge>();

        public void AddNode(Guid id, string nodeName, Point pos)
        {
            Nodes.Add(new Node(id, nodeName, pos));
        }

        public void AddEdge(Guid startId, Guid endId)
        {
            Edges.Add(new Edge(startId, endId));
        }
    }
}
