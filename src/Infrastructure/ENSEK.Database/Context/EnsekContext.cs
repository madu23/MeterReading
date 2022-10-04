using ENSEK.Database.Config;
using ENSEK.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Database.Context
{
    public class EnsekContext : DbContext, IUnitOfWork
    {
        private readonly ILogger<EnsekContext> _logger;
        public EnsekContext(DbContextOptions<EnsekContext> options) : base(options) { }
        public EnsekContext(DbContextOptions<EnsekContext> options, ILogger<EnsekContext> logger) : base(options)
        {
            _logger = logger;
        }
        public EnsekContext()
        {

        }

        public new bool SaveChanges()
        {
            try
            {
                base.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return false;
            }
        }

        public new async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await base.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return false;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountConfig).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
