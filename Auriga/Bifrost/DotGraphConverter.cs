using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Bifrost.GraphElements;
using DotParser;

namespace Bifrost
{
    public class DotGraphConverter
    {
        public static Graph ToGraph(GraphData dot)
        {
            Graph result = new Graph();
            Rect bb = parseDotBoundingBox(dot.GraphAttributes.GetValueOrDefault("bb"));
            foreach (var dotNode in dot.Nodes) 
            {
                Point p = Point.Parse(dotNode.Value.GetValueOrDefault("pos"));
                // Have to flip position, because WPF orig is leftop, graphviz is leftbot
                p = new Point(p.X, bb.Bottom - p.Y);
                result.AddNode(Guid.NewGuid(), dotNode.Key, p);
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
