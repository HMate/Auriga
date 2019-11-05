using System;

namespace Auriga.Models
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