using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Models
{
    public class PodioAzureItemLink
    {
        public int Id { get; set; }
        public int PodioId { get; set; }
        public int PodioRevision { get; set; }
        public int AzureId { get; set; }
        public int AzureRevision { get; set; }
        public int PodioAppId { get; set; }
        public PodioApp PodioApp { get; set; }
    }
}
