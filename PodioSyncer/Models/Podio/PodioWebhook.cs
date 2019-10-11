using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Models.Podio
{
    public class PodioWebhook
    {
        public string item_id { get; set; }
        public string item_revision_id { get; set; }
        public string code { get; set; }
        public string hook_id { get; set; }
        public string type { get; set; }

    }
}
