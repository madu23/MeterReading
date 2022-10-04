using ENSEK.Database.Context;
using ENSEK.Domain.Entities;
using ENSEK.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Service.Implementation.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly EnsekContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public BaseRepository(EnsekContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task DeleteAsync(string id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            _context.Entry(entity).State = EntityState.Deleted;
            return Task.CompletedTask;
        }

        public TEntity Find(params object[] keyValues)
        {
            var entity = _context.Set<TEntity>().Find(keyValues);
            return entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> query)
        {
            return GetAll().Where(query).AsEnumerable();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return;
        }

        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            return;
        }

        public Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
