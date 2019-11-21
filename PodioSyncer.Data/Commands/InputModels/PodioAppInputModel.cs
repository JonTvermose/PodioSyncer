using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Commands.InputModels
{
    public class PodioAppInputModel
    {
        public int? Id { get; set; }
        public int PodioAppId { get; set; }
        public string Name { get; set; }
        public string AppToken { get; set; }
        public string PodioTypeExternalId { get; set; }
        public bool Verified { get; set; }
    }
}
