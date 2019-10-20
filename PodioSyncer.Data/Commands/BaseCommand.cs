using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Commands
{
    public abstract class BaseCommand
    {
        protected readonly ApplicationDbContext dbContext;

        public BaseCommand(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected abstract void RunCommand();

        public void Run()
        {
            // Logging, and other default checks can be done here
            RunCommand();
        }
    }
}
