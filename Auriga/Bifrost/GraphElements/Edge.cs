using System;

namespace Bifrost.GraphElements
{
    public class Edge
    {
        private Guid startId;
        private Guid endId;

        public Edge(Guid startId, Guid endId)
        {
            this.startId = startId;
            this.endId = endId;
        }
    }
}