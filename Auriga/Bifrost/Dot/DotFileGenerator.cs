using System;
using System.Collections.Generic;
using System.Text;

namespace Bifrost.Dot
{
    public class DotFileGenerator
    {
        public static string Serialize(DotGraph gr)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("digraph {");
            if(gr.GraphAttributes.Count > 0)
            {
                builder.AppendFormat("graph {0}\n", Serialize(gr.GraphAttributes));
            }
            if (gr.NodeAttributes.Count > 0)
            {
                builder.AppendFormat("node {0}\n", Serialize(gr.NodeAttributes));
            }
            foreach (var node in gr.Nodes)
            {
                builder.AppendFormat("\"{0}\"{1}\n", node.Key, Serialize(node.Value.Attributes));
            }
            foreach (var edgeGroup in gr.Edges)
            {
                var nodeEdges = edgeGroup.Value;
                foreach (var edge in nodeEdges)
                {
                    builder.AppendFormat("\"{0}\" -> \"{1}\"{2}\n", 
                        edgeGroup.Key.Item1, edgeGroup.Key.Item2, Serialize(edge.Attributes));
                }
            }
            builder.Append("}");
            return builder.ToString();
        }

        private static string Serialize(IDictionary<string, string> attributes)
        {
            if (attributes.Count == 0)
            {
                return "";
            }
            List<string> attrParts = new List<string>();
            foreach (var attr in attributes)
            {
                attrParts.Add(string.Format("\"{0}\"=\"{1}\"", attr.Key, attr.Value));
            }
            return string.Format("[{0}]", string.Join(", ", attrParts));
        }
    }
}
