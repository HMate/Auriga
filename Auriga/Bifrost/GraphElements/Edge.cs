using System;

namespace Bifrost.GraphElements
{
    public class Edge
    {
        public string StartId { get; }
        public string EndId { get; }

        public Edge(string startId, string endId)
        {
            StartId = startId;
            EndId = endId;
        }
    }
}