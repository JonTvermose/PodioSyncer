using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodioSyncer.Data.Commands
{
    public class UpdateLink : BaseCommand
    {
        public PodioAzureItemLink InputModel { get; set; }
        public UpdateLink(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override void RunCommand()
        {
            var entity = dbContext.PodioAzureItemLinks.SingleOrDefault(x => x.AzureId == InputModel.AzureId && x.PodioId == InputModel.PodioId);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.PodioRevision = InputModel.PodioRevision;
            entity.AzureRevision = InputModel.AzureRevision;
            dbContext.PodioAzureItemLinks.Update(entity);
            dbContext.SaveChanges();
        }
    }
}
