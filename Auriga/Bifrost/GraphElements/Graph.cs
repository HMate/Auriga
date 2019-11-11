using System;
using System.Collections.Generic;
using System.Windows;

namespace Bifrost.GraphElements
{
    public class Graph
    {
        public ICollection<Node> Nodes { get; } = new List<Node>();
        public ICollection<Edge> Edges { get; } = new List<Edge>();

        public Node AddNode(Guid id, string nodeName, Point pos)
        {
            Node n = new Node(id, nodeName, pos);
            Nodes.Add(n);
            return n;
        }

        public void AddEdge(Guid startId, Guid endId)
        {
            Edges.Add(new Edge(startId, endId));
        }

        public Node? FindNode(Guid id)
        {
            foreach (var node in Nodes)
            {
                if (node.Id == id)
                {
                    return node;
                }
            }
            return null;
        }
    }
}
