using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Models
{
    public class PodioApp
    {
        public int Id { get; set; }
        public int PodioAppId { get; set; }
        public string Name { get; set; }
        public string AppToken { get; set; }
        public bool Verified { get; set; }
        public bool Active { get; set; }
        public string WebhookUrl { get; set; }

    }
}
