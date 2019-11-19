using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Models
{
    public class TypeMapping
    {
        public int Id { get; set; }
        public string PodioType { get; set; }
        public string AzureType { get; set; }
        public int PodioAppId { get; set; }
        public PodioApp PodioApp { get; set; }
        public ICollection<FieldMapping> FieldMappings { get; set; }

    }
}
