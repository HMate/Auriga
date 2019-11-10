using System;
using System.Windows;

namespace Bifrost.GraphElements
{
    public class Node
    {
        public Guid Id;
        public string NodeName;
        public Point Position;

        public Node(Guid id, string nodeName, Point pos)
        {
            Id = id;
            NodeName = nodeName;
            Position = pos;
        }
    }
}
