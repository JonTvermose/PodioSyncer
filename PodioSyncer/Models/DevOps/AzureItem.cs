using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Models.DevOps
{
    public class AzureItem
    {
        public string SubscriptionId { get; set; }
        public int NotificationId { get; set; }
        public string Id { get; set; }
        public string EventType { get; set; }
        public AzureMessage Message { get; set; }
        public AzureMessage DetailedMessage { get; set; }
        public AzureResource Resource { get; set; }
        public DateTime CreatedDate { get; set; }
        public AzureResourceContainers ResourceContainers { get; set; }
    }
}
