using Microsoft.EntityFrameworkCore;
using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(){}

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PodioAzureItemLink>()
                .HasIndex(x => new { x.AzureId, x.PodioId });

            builder.Entity<CategoryMapping>()
                .HasOne(x => x.FieldMapping)
                .WithMany(x => x.CategoryMappings);

            builder.Entity<PodioAzureItemLink>()
                .HasOne(x => x.PodioApp)
                .WithMany(x => x.PodioAzureItemLinks);
        }

        public DbSet<PodioApp> PodioApps { get; set; }
        public DbSet<PodioAzureItemLink> PodioAzureItemLinks { get; set; }
        public DbSet<FieldMapping> FieldMappings { get; set; }
        public DbSet<CategoryMapping> CategoryMappings { get; set; }

    }
}
