#region Namespace References
using System.Threading.Tasks;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Pattern.WebAPI.SmartWrapper;
#endregion

namespace UKParliament.CodeTest.Services
{
    public interface IPersonService
    {
        Task<ApiResponse<Person>> GetPersonsAsync();
        Task<ApiResponse<Person>> GetPersonAsync(int id);
        Task<ApiResponse<Person>> AddOrUpdateAsync(Person person, int key = 0);
        Task<bool> DeleteAsync(int id);
    }
}
