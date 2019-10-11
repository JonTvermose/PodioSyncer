using Microsoft.EntityFrameworkCore;
using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS2017;Database=PodioSyncer;Integrated Security=True");
        }

        public DbSet<PodioApp> PodioApps { get; set; }
    }
}
