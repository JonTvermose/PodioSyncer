using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Models
{
    public class SyncEvent
    {
        public int Id { get; set; }
        public DateTime SyncDate { get; set; }
        public Initiator Initiator { get; set; }
        public int PodioRevision { get; set; }
        public int AzureRevision { get; set; }
        public int PodioAzureItemLinkId { get; set; }
        public PodioAzureItemLink PodioAzureItemLink { get; set; }
    }

    public enum Initiator {
        PodioHook = 0,
        AzureHook = 1,
        Manuel = 2
    }

}
