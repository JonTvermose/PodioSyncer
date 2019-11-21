using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Extensions
{
    public static class FieldMappingExtensions
    {
        public static bool IsComplexType(this FieldMapping mapping)
        {
            return mapping.FieldType == FieldType.Category || mapping.FieldType == FieldType.Image || mapping.FieldType == FieldType.User;
        }
    }
}
