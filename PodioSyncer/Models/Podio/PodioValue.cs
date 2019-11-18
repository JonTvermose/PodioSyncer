using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Models.Podio
{
    public class PodioValue
    {
        public PodioValueValue Value { get; set; }
    }

    public class PodioValueValue
    {
        public string Status { get; set; }
        public string Text { get; set; }
        public int Id { get; set; }
        public string Color { get; set; }
    }
}
