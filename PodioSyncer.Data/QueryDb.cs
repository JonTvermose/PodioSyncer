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

        public IQueryable<PodioApp> PodioApps => _dbContext.PodioApps;
    }
}
