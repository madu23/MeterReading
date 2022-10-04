using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        public bool SaveChanges();
    }
}
