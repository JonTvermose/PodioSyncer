using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Models.DevOps
{
    public class AzureUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string UniqueName { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}
