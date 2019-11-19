using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
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

            dbContext.PodioAzureItemLinks.Add(InputModel);
            dbContext.SaveChanges();
        }
    }
}
