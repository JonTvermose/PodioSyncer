using Microsoft.EntityFrameworkCore;
using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }

        public DbSet<LogEntry> LogEntries { get; set; }

    }
}
