using Microsoft.EntityFrameworkCore;
using PodioSyncer.Data.Commands.InputModels;
using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodioSyncer.Data.Commands
{
    public class DeletePodioApp : BaseCommand
    {
        public int InputModel { get; set; }

        public DeletePodioApp(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override void RunCommand()
        {
            var podioApp = dbContext.PodioApps
                .Include(x => x.PodioAzureItemLinks)
                .Include(x => x.TypeMappings)
                .ThenInclude(x => x.FieldMappings)
                .ThenInclude(x => x.CategoryMappings)
                .SingleOrDefault(x => x.PodioAppId == InputModel);
            if (podioApp == null)
                throw new ArgumentException(nameof(InputModel));

            dbContext.CategoryMappings.RemoveRange(podioApp.TypeMappings.SelectMany(x => x.FieldMappings.SelectMany(f => f.CategoryMappings)));
            dbContext.FieldMappings.RemoveRange(podioApp.TypeMappings.SelectMany(x => x.FieldMappings));
            dbContext.TypeMappings.RemoveRange(podioApp.TypeMappings);
            dbContext.PodioApps.Remove(podioApp);
            dbContext.SaveChanges();
        }
    }
}
