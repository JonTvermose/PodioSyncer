using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Models
{
    public class PodioAzureItemLink
    {
        public int PodioId { get; set; }
        public int PodioRevision { get; set; }
        public string AzureId { get; set; }
        public int AzureRevision { get; set; }
    }
}
