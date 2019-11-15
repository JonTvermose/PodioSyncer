using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Models.DevOps
{
    public class AzureResourceContainers
    {
        public AzureResourceContainer Collection { get; set; }
        public AzureResourceContainer Account { get; set; }
        public AzureResourceContainer Project { get; set; }
    }
}
