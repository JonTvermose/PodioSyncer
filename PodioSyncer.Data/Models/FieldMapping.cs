using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Models
{
    public class FieldMapping
    {
        public int Id { get; set; }
        public string AzureFieldName { get; set; }
        public string PodioFieldName { get; set; }
        public FieldType FieldType { get; set; }
        public ICollection<CategoryMapping> CategoryMappings { get; set; }
        public int TypeMappingId { get; set; }
        public TypeMapping TypeMapping { get; set; }
        public string PrefixValue { get; set; }
    }

    public enum FieldType
    {
        Boolean = 0,
        Int = 1,
        String = 2,
        Image = 3,
        User = 5,
        Category = 6,
        Date = 7
    }
}
