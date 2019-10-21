using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodioSyncer.Data.Commands
{
    public class VerifyWebhookCommand : BaseCommand
    {
        public int PodioAppId { get; set; }

        public VerifyWebhookCommand(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override void RunCommand()
        {
            var podioApp = dbContext.PodioApps.SingleOrDefault(x => x.PodioAppId == PodioAppId.ToString());
            if (podioApp == null)
                throw new ArgumentException(nameof(PodioAppId));

            podioApp.Verified = true;
            dbContext.SaveChanges();
        }
    }
}
