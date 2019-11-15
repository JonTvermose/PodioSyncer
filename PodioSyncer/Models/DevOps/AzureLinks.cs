using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Models.DevOps
{
    public class AzureLinks
    {
        public AzureLink Self { get; set; }
        public AzureLink WorkItemUpdates { get; set; }

        public AzureLink Parent { get; set; }

        public AzureLink Html { get; set; }
        public AzureLink workItemRevisions { get; set; }

    }
}
