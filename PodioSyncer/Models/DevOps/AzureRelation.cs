using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Models.DevOps
{
    public class AzureRelation
    {
        public string Rel { get; set; }
        public string Url { get; set; }
        public AzureRelationAttributes Attributes { get; set; }
    }
}
