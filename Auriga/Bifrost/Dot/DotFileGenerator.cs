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
            foreach(var node in gr.Nodes)
            {
                builder.AppendFormat("\"{0}\"{1}\n", node.Key, Serialize(node.Value.Attributes));
            }
            foreach (var edge in gr.Edges)
            {
                builder.AppendFormat("\"{0}\" -> \"{1}\"\n", edge.Key.Item1, edge.Key.Item2);
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
