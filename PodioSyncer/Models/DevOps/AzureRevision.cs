using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PodioSyncer.Models.DevOps
{
    public class AzureRevision
    {
        public int Id { get; set; }
        public int Rev { get; set; }
        public AzureFields Fields { get; set; }

        public AzureRelation[] Relations { get; set; }

        [JsonPropertyName("_links")]
        public AzureLinks Links { get; set; }

        public string Url { get; set; }
    }
}
