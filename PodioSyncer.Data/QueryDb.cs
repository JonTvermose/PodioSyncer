using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodioSyncer.Data
{
    public class QueryDb
    {
        private readonly ApplicationDbContext _dbContext;

        public QueryDb(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
            _dbContext = dbContext;
        }

        public IQueryable<PodioApp> PodioApps => _dbContext.PodioApps.AsQueryable();
        public IQueryable<PodioAzureItemLink> Links => _dbContext.PodioAzureItemLinks.AsQueryable();
        public IQueryable<FieldMapping> FieldMappings => _dbContext.FieldMappings.AsQueryable();
        public IQueryable<CategoryMapping> CategoryMappings => _dbContext.CategoryMappings.AsQueryable();
        public IQueryable<TypeMapping> TypeMappings => _dbContext.TypeMappings.AsQueryable();
        public IQueryable<SyncEvent> SyncEvents => _dbContext.SyncEvents.AsQueryable();


    }
}
