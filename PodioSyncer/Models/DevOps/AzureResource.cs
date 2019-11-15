using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PodioSyncer.Models.DevOps
{
    public class AzureResource
    {
        public int Id { get; set; }
        public int WorkItemId { get; set; }
        public int Rev { get; set; }
        public AzureUser RevisedBy { get; set; }
        public DateTime RevisedDate { get; set; }
        public dynamic Fields { get; set; }
        public string Url { get; set; }
        public AzureRevision Revision { get; set; }

        [JsonPropertyName("_links")]
        public AzureLinks Links { get; set; }

    }
}
