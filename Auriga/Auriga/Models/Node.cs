using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Auriga.Models
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
