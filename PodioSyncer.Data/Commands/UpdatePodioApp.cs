using PodioSyncer.Data.Commands.InputModels;
using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodioSyncer.Data.Commands
{
    public class UpdatePodioApp : BaseCommand
    {
        public PodioAppInputModel InputModel { get; set; }

        public UpdatePodioApp(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override void RunCommand()
        {            
            PodioApp podioApp = dbContext.PodioApps.SingleOrDefault(x => x.PodioAppId == InputModel.PodioAppId); ;
            if (podioApp == null)
                throw new ArgumentException(nameof(InputModel.PodioAppId));

            podioApp.AppToken = InputModel.AppToken;
            podioApp.Name = InputModel.Name;
            podioApp.Verified = false;           
            dbContext.SaveChanges();
        }
    }
}
