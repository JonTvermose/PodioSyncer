using Microsoft.AspNetCore.Mvc.Filters;
using PodioSyncer.Data;
using PodioSyncer.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Middleware
{
    public class DblExceptionFilter: ExceptionFilterAttribute
    {
        private readonly LogDbContext _context;
        
        public DblExceptionFilter(LogDbContext context)
        {
            _context = context;
        }

        public override void OnException(ExceptionContext context)
        {
            LogEntry log = new LogEntry
            {
                TimeStamp = DateTime.UtcNow,
                ActionDescriptor = context.ActionDescriptor.DisplayName,
                IpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                Message = context.Exception.Message,
                RequestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier,
                RequestPath = context.HttpContext.Request.Path,
                Source = context.Exception.Source,
                StackTrace = context.Exception.StackTrace,
                Type = context.Exception.GetType().ToString(),
                User = context.HttpContext.User.Identity.Name,
            };
            try
            {
                using (var reader = new StreamReader(context.HttpContext.Request.Body))
                {
                    var body = reader.ReadToEnd();
                    log.Data = body;
                }
            }
            catch {}
            _context.LogEntries.Add(log);
            _context.SaveChanges();
        }
    }
}
