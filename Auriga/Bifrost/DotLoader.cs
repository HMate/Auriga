using DotParser;
using System;

namespace Bifrost
{
    public class DotLoader
    {
        public static Dot.DotGraph Load(string dotString)
        {
            Dot.DotGraph gr = new Dot.DotGraph();

            dotString = dotString.Trim();
            string[] tokens = dotString.Split(new [] { " ", "\t", "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            return gr;
        }


        public static GraphData LoadF(string dotString)
        {
            try
            {
                GraphData gr = DotParser.DotParser.parse(dotString);
                return gr;
            }
#pragma warning disable CA1031 // Do not catch general exception types -> Sadly DotParser throws System.Exception
            catch (Exception)
            {
                return DotParser.DotParser.parse("graph{}");
            }
#pragma warning restore CA1031 // Do not catch general exception types

        }
    }
}
