using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Commands
{
    public class VerifyWebhookCommand
    {
        public int PodioAppId { get; set; }

        public VerifyWebhookCommand()
        {

        }
    }
}
