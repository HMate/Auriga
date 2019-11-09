using DotParser;
using System;

namespace Bifrost
{
    public class DotLoader
    {
        public static GraphData Load(string filePath)
        {
            GraphData gr = DotParser.DotParser.parse(filePath);
            return gr;

        }
    }
}
