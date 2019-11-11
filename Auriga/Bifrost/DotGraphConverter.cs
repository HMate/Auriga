using Bifrost.GraphElements;
using DotParser;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Bifrost
{
    public class DotGraphConverter
    {
        public static Graph ToGraph(GraphData dot)
        {
            Graph result = new Graph();
            Rect bb = parseDotBoundingBox(dot.GraphAttributes.GetValueOrDefault("bb"));
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            foreach (var dotNode in dot.Nodes)
            {
                Point p = Point.Parse(dotNode.Value.GetValueOrDefault("pos"));
                // Have to flip position, because WPF orig is leftop, graphviz is leftbot
                p = new Point(p.X, bb.Bottom - p.Y);
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

        private static Rect parseDotBoundingBox(string? boundBoxString)
        {
            Rect lbwh = Rect.Parse(boundBoxString);
            return new Rect(lbwh.Location, new Size(lbwh.Width, lbwh.Height));
        }
    }
}
