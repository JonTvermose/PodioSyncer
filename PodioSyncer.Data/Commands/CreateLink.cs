using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodioSyncer.Data.Commands
{
    public class CreateLink : BaseCommand
    {
        public PodioAzureItemLink InputModel { get; set; }

        public CreateLink(ApplicationDbContext context) : base(context)
        {
        }
        
        protected override void RunCommand()
        {
            if (InputModel == null)
                throw new ArgumentException(nameof(InputModel));

            if (dbContext.PodioAzureItemLinks.Any(x => x.PodioId == InputModel.PodioId))
                throw new ArgumentException(nameof(InputModel.PodioId));

            dbContext.PodioAzureItemLinks.Add(InputModel);
            dbContext.SaveChanges();
        }
    }
}
