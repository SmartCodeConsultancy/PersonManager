#region Namespace References
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace UKParliament.CodeTest.Pattern.Service.Interfaces
{
    public interface ISmartService<T> where T : class
    {
        ValueTask<EntityEntry<T>> InsertAsync(T entity,
            CancellationToken cancellationToken = default);
        T Insert(T entity);
        T Update(T entity);
        Task<T> UpdateAsync(T entity, object key);
        bool Delete(T entity);
        Task<int> DeleteAsync(T entity);
    }
}
