using System;

namespace Bifrost.GraphElements
{
    public class Edge
    {
        public Guid StartId { get; }
        public Guid EndId { get; }

        public Edge(Guid startId, Guid endId)
        {
            StartId = startId;
            EndId = endId;
        }
    }
}