#region Namespace References
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;
using UKParliament.CodeTest.Pattern.Service.Interfaces;
#endregion

namespace UKParliament.CodeTest.Pattern.Service
{
    public class SmartService<T> : ISmartService<T> where T : class
    {
        public SmartService(DbContext dbContext)
        {
            Context = dbContext;
        }

        public virtual ValueTask<EntityEntry<T>> InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = Context.Set<T>().AddAsync(entity, cancellationToken);
            Context.SaveChangesAsync(cancellationToken);
            return result;
        }

        public virtual T Insert(T entity)
        {
            var result = Context.Set<T>().Add(entity).Entity;
            Context.SaveChanges();
            return result;
        }

        public virtual T Update(T entity)
        {
            var result = Context.Set<T>().Update(entity).Entity;
            Context.SaveChanges();
            return result;
        }

        public virtual async Task<T> UpdateAsync(T entity, object key)
        {
            if (entity == null)
                return null;
            T result = await Context.Set<T>().FindAsync(key);
            if (result != null)
            {
                Context.Entry(result).CurrentValues.SetValues(entity);
                await Context.SaveChangesAsync();
            }
            return result;
        }

        public virtual bool Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
            return true;
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            return await Context.SaveChangesAsync();
        }

        private DbContext Context { get; }
    }
}
