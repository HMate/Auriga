using System;
using System.Windows;

namespace Bifrost.GraphElements
{
    public class Node
    {
        public Guid Id; // TODO: Change Id to string
        public string NodeName;
        public Point Position;

        public Node(Guid id, string nodeName) : this(id, nodeName, new Point(0d, 0d))
        {}

        public Node(Guid id, string nodeName, Point pos)
        {
            Id = id;
            NodeName = nodeName;
            Position = pos;
        }
    }
}
