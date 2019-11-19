using Microsoft.EntityFrameworkCore;
using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
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

            builder.Entity<FieldMapping>()
                .HasOne(x => x.TypeMapping)
                .WithMany(x => x.FieldMappings);

            builder.Entity<FieldMapping>()
                .HasIndex(x => new { x.TypeMappingId, x.PodioFieldName });

            builder.Entity<TypeMapping>()
                .HasOne(x => x.PodioApp)
                .WithMany(x => x.TypeMappings);
        }

        public DbSet<PodioApp> PodioApps { get; set; }
        public DbSet<PodioAzureItemLink> PodioAzureItemLinks { get; set; }
        public DbSet<FieldMapping> FieldMappings { get; set; }
        public DbSet<CategoryMapping> CategoryMappings { get; set; }
        public DbSet<TypeMapping> TypeMappings { get; set; }

    }
}
