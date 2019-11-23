using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Models
{
    public class CategoryMapping
    {
        public int Id { get; set; }
        public string PodioValue { get; set; }
        public string AzureValue { get; set; }
        public FieldType FieldType { get; set; }
        public int FieldMappingId { get; set; }
        public bool Required { get; set; }

        public FieldMapping FieldMapping { get; set; }
        public int PodioValueId { get; set; }
    }
}
