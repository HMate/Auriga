using Bifrost.GraphElements;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Bifrost.Dot
{
    /// <summary>
    /// Converts a DotGraph to the Graph representation which Auriga uses internally.
    /// </summary>
    public class DotGraphConverter
    {
        /// <summary>
        /// Creates a Graph from a DotGraph that contains "bb" graph property and "pos" properties for every node.
        /// </summary>
        /// <param name="dot"></param>
        /// <returns><see cref="Graph"/></returns>
        public static Graph ToGraph(DotGraph dot)
        {
            Graph result = new Graph();
            Rect bb = new Rect(0, 0, 100, 100);
            if (dot.GraphAttributes.ContainsKey("bb"))
                bb = parseDotBoundingBox(dot.GraphAttributes["bb"]);

            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            foreach (var dotNode in dot.Nodes)
            {
                Node n = result.AddNode(fromDotNode(dotNode.Key, dotNode.Value, bb));
                nodes.Add(dotNode.Key, n);
            }
            foreach (var dotEdge in dot.Edges)
            {
                (string start, string end) = dotEdge.Key;
                result.AddEdge(new Edge(nodes[start].Id, nodes[end].Id));
            }
            return result;
        }

        private static Node fromDotNode(string nodeId, DotNode dotNode, Rect bb)
        {
            Point p = new Point(0, 0);
            if (dotNode.Attributes.ContainsKey("pos"))
            {
                p = Point.Parse(dotNode.Attributes["pos"]);
                // Have to flip position, because WPF orig is leftop, graphviz is leftbot
                p = new Point(p.X, bb.Bottom - p.Y);
            }

            string name = nodeId;
            if (dotNode.Attributes.ContainsKey("label"))
            {
                name = dotNode.Attributes["label"];
            }

            return new Node(nodeId, name, p);
        }

        private static Rect parseDotBoundingBox(string boundBoxString)
        {
            Rect lbwh = Rect.Parse(boundBoxString);
            return new Rect(lbwh.Location, new Size(lbwh.Width, lbwh.Height));
        }

        public static DotGraph ToDot(Graph gr)
        {
            DotGraph result = new DotGraph();
            foreach (var node in gr.Nodes)
            {
                DotNode dotNode = new DotNode();
                dotNode.Attributes.Add("label", node.NodeName);
                var id = ensureUniqueId(result.Nodes, node.Id);
                result.Nodes.Add(id, dotNode);
            }
            foreach (var edge in gr.Edges)
            {
                var key = (edge.StartId.ToString(), edge.EndId.ToString());
                if (!result.Edges.ContainsKey(key))
                {
                    result.Edges.Add(key, new List<DotEdge>());
                }
                DotEdge dotEdge = new DotEdge();
                result.Edges[key].Add(dotEdge);
            }
            return result;
        }

        private static string ensureUniqueId(IDictionary<string, DotNode> nodes, string id)
        {
            if(!nodes.ContainsKey(id))
            {
                return id;
            }
            int counter = 0;
            string candidate = string.Format("{0}{1}", id, counter);
            while (nodes.ContainsKey(candidate))
            {
                candidate = string.Format("{0}{1}", id, counter);
            }
            return candidate;
        }
    }
}
