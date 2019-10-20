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
            PodioApp podioApp = null;
            if (InputModel.PodioAppId.HasValue)
            {
                podioApp = dbContext.PodioApps.SingleOrDefault(x => x.PodioAppId == InputModel.PodioAppId); // eksploderer hvis der er 2 i databasen
            }
            if (podioApp != null)
                throw new ArgumentException(nameof(InputModel.PodioAppId)); // eksploderer hvis der er 1 i databasen 

            // Her opretter vi en ny en, den findes ikke i databasen i forvejen
            podioApp = new PodioApp
            {
                AppToken = InputModel.AppToken,
                Name = InputModel.Name,
                PodioAppId = InputModel.PodioAppId.Value 
            };
            dbContext.PodioApps.Add(podioApp);
            dbContext.SaveChanges();
        }
    }
}
