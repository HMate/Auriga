using GraphX.Controls;
using GraphX.PCL.Common.Models;
using GraphX.PCL.Logic.Models;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auriga.ViewModels
{
    //Graph data class
    public class GraphExample : BidirectionalGraph<IncludeGraphNode, IncludeGraphEdge> { }

    //Logic core class
    public class GXLogicCoreExample : GXLogicCore<IncludeGraphNode, IncludeGraphEdge, BidirectionalGraph<IncludeGraphNode, IncludeGraphEdge>> { }

    //Vertex data object
    public class IncludeGraphNode : VertexBase
    {
        public string FullPath { get; set; }
        public string Name { get; set; }

        public IncludeGraphNode(int id, string fullPath, string name)
        {
            ID = id;
            FullPath = fullPath;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    //Edge data object
    public class IncludeGraphEdge : EdgeBase<IncludeGraphNode>
    {
        public IncludeGraphEdge(IncludeGraphNode source, IncludeGraphNode target, double weight = 1)
            : base(source, target, weight)
        {
        }

        public IncludeGraphEdge()
            : base(null, null, 1)
        {
        }

        public string Text => $"{Source} -> {Target}";

        public override string ToString()
        {
            return Text;
        }
    }
}
