using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodioSyncer.Data.Commands
{
    public class CreateSyncEvent : BaseCommand
    {
        public SyncEvent InputModel { get; set; }

        public CreateSyncEvent(ApplicationDbContext context) : base(context)
        {
        }
        
        protected override void RunCommand()
        {
            if (InputModel == null)
                throw new ArgumentException(nameof(InputModel));

            if (dbContext.PodioAzureItemLinks.Any(x => x.Id == InputModel.Id) || InputModel.Id != 0)
                throw new ArgumentException(nameof(InputModel.Id));

            InputModel.SyncDate = DateTime.UtcNow;
            
            dbContext.SyncEvents.Add(InputModel);
            dbContext.SaveChanges();
        }
    }
}
