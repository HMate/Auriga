﻿using Bifrost.GraphElements;
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
        /// <returns><see cref="Graph"/>></returns>
        public static Graph ToGraph(DotGraph dot)
        {
            Graph result = new Graph();
            Rect bb = new Rect(0, 0, 100, 100);
            if (dot.GraphAttributes.ContainsKey("bb"))
                bb = parseDotBoundingBox(dot.GraphAttributes["bb"]);

            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            foreach (var dotNode in dot.Nodes)
            {
                Point p = new Point(0, 0);
                if (dotNode.Value.Attributes.ContainsKey("pos"))
                {
                    p = Point.Parse(dotNode.Value.Attributes["pos"]);
                    // Have to flip position, because WPF orig is leftop, graphviz is leftbot
                    p = new Point(p.X, bb.Bottom - p.Y);
                }
                Node n = result.AddNode(Guid.NewGuid(), dotNode.Key, p);
                nodes.Add(dotNode.Key, n);
            }
            foreach (var dotEdge in dot.Edges)
            {
                (string start, string end) = dotEdge.Key;
                result.AddEdge(nodes[start].Id, nodes[end].Id);
            }
            return result;
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
                result.Nodes.Add(node.Id.ToString(), dotNode);
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
    }
}