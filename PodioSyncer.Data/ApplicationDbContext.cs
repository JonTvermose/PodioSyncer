using Microsoft.EntityFrameworkCore;
using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<PodioApp> PodioApps { get; set; }
    }
}
