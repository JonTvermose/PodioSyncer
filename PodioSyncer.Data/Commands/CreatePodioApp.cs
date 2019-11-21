using PodioSyncer.Data.Commands.InputModels;
using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodioSyncer.Data.Commands
{
    public class CreatePodioApp : BaseCommand
    {
        public PodioAppInputModel InputModel { get; set; }

        public CreatePodioApp(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override void RunCommand()
        {            
            PodioApp podioApp = dbContext.PodioApps.SingleOrDefault(x => x.PodioAppId == InputModel.PodioAppId); ;
            if (podioApp != null)
                throw new ArgumentException(nameof(InputModel.PodioAppId));

            if (string.IsNullOrWhiteSpace(InputModel.Name))
                throw new ArgumentNullException(nameof(InputModel.Name));

            if (string.IsNullOrWhiteSpace(InputModel.AppToken))
                throw new ArgumentNullException(nameof(InputModel.AppToken));

            if (InputModel.PodioAppId == 0)
                throw new ArgumentNullException(nameof(InputModel.PodioAppId));

            podioApp = new PodioApp
            {
                AppToken = InputModel.AppToken,
                Name = InputModel.Name,
                PodioAppId = InputModel.PodioAppId,
                PodioTypeExternalId = InputModel.PodioTypeExternalId
            };
            dbContext.PodioApps.Add(podioApp);
            dbContext.SaveChanges();
        }
    }
}
