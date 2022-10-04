using ENSEK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity 
    {
        IUnitOfWork UnitOfWork { get; }
        TEntity Find(params object[] keyValues);
        Task InsertAsync(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        IQueryable<TEntity> GetAll();
        IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> filter);
        Task DeleteAsync(string id);
    }
}
