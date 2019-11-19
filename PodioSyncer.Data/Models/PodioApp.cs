using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Models
{
    public class PodioApp
    {
        public int Id { get; set; }
        public string PodioAppId { get; set; }
        public string Name { get; set; }
        public string AppToken { get; set; }
        public bool Verified { get; set; }
        public bool Active { get; set; }
        public ICollection<PodioAzureItemLink> PodioAzureItemLinks { get; set; }
        public ICollection<TypeMapping> TypeMappings { get; set; }

    }
}
