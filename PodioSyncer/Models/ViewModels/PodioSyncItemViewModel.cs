using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Models.ViewModels
{
    public class PodioSyncItemViewModel
    {
        public int PodioAppId { get; set; }
        public string PodioItemUrl { get; set; }

        public int AppItemId => int.Parse(new Uri(PodioItemUrl).AbsolutePath.Split('/').Last());
    }
}
