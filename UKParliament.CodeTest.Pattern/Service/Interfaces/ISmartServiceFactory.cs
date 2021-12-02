#region Namespace References
using Microsoft.EntityFrameworkCore;
#endregion


namespace UKParliament.CodeTest.Pattern.Service.Interfaces
{
    public interface ISmartServiceFactory
    {
        ISmartService<T> GetService<T>() where T : class;
        DbContext Context { get; }
    }
}
