using System;
using System.Collections.Generic;
using System.Windows;

namespace Bifrost.GraphElements
{
    public class Graph
    {
        public ICollection<Node> Nodes { get; } = new List<Node>();
        public ICollection<Edge> Edges { get; } = new List<Edge>();
        
        public Node AddNode(Node node)
        {
            Nodes.Add(node);
            return node;
        }

        public Edge AddEdge(Edge edge)
        {
            Edges.Add(edge);
            return edge;
        }

        public Node? FindNode(string id)
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
