using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Models.ViewModels
{
    public class PodioAppViewModel
    {
        public int Id { get; set; }
        public string PodioAppId { get; set; }
        public string Name { get; set; }
        public string AppToken { get; set; }
        public bool Active { get; set; }
        public bool Verified { get; set; }
        public string WebhookUrl { get; set; }
    }
}
