using System;
using System.Windows;

namespace Bifrost.GraphElements
{
    public class Node
    {
        public string Id;
        public string NodeName;
        public Point Position;

        public Node(string id, string nodeName) : this(id, nodeName, new Point(0d, 0d))
        {}

        public Node(string id, string nodeName, Point pos)
        {
            Id = id;
            NodeName = nodeName;
            Position = pos;
        }
    }
}
