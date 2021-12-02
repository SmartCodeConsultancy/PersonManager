#region Namespace References
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Pattern.Service.Interfaces;
using UKParliament.CodeTest.Pattern.WebAPI.SmartWrapper;
#endregion

namespace UKParliament.CodeTest.Services
{
    /// <summary>
    /// Service layer of Person manager
    /// </summary>
    public class PersonService : BaseService<PersonService, Person>, IPersonService
    {
        private readonly DbSet<Person> People;
        readonly ISmartService<Person> service;
        public PersonService(ISmartServiceFactory serviceFactory,
            ILogger<PersonService> logger) : base(serviceFactory, logger)
        {
            People = ServiceFactory.Context.Set<Person>();
            service = ServiceFactory.GetService<Person>();
        }

        /// <summary>
        /// Person list
        /// </summary>
        /// <returns>A collection of<seealso cref="Person"/> Entity</returns>
        public async Task<ApiResponse<Person>> GetPersonsAsync()
        {
            try
            {
                List<Person> persons = await People.AsNoTracking().ToListAsync();
                if(persons == null)
                {
                    return GenerateApiResponse(default(List<Person>),
                    true, "No Content", HttpStatusCode.NoContent, "Not Content");
                }
                return GenerateApiResponse(persons);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return GenerateApiResponse(default(List<Person>),
                    true, ex.Message, HttpStatusCode.NotFound, ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// A person by <paramref name="id"/> value.
        /// </summary>
        /// <param name="id">Unique key value</param>
        /// <returns><seealso cref="Person"/> entity</returns>
        public async Task<ApiResponse<Person>> GetPersonAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return GenerateApiResponse(new Person
                    {
                        Id = id,
                        Name = string.Empty,
                        DateOfBirth = default,
                        Address = string.Empty,
                        Email = string.Empty,
                        Phone = string.Empty
                    },
                    true, "BadRequest", HttpStatusCode.BadRequest, "Bad Request");
                }
                var person = await People.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (person == null)
                {
                    return GenerateApiResponse(default(Person),
                    true, "No Content", HttpStatusCode.NoContent, "Not Content");
                }
                return GenerateApiResponse(person);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return GenerateApiResponse(default(Person),
                    true, ex.Message, HttpStatusCode.NotFound, ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Create or Update Person
        /// </summary>
        /// <param name="person"><see cref="Person"/> details</param>
        /// <param name="key"></param>
        /// <returns>Created or Update <see cref="Person"/> entity</returns>

        public async Task<ApiResponse<Person>> AddOrUpdateAsync(Person person, int key = 0)
        {
            try
            {
                if(person == null)
                    return GenerateApiResponse(person,
                   true, "BadRequest", HttpStatusCode.BadRequest, "Bad Request");

                if(key > 0)
                {
                    var result = await service.UpdateAsync(person, key);
                    return GenerateApiResponse(result);
                }
                else
                {
                    var result = await service.InsertAsync(person);
                    return GenerateApiResponse(result.Entity);
                }
            }
            catch(Exception ex)
            {
                LogException(ex);
                return GenerateApiResponse(person,
                   true, ex.Message, HttpStatusCode.BadRequest, ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// Delete Person
        /// </summary>
        /// <param name="id">Unique key value</param>
        /// <returns>True or False</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if(id > 0)
                {
                    var person = await People.FindAsync(id);
                    if (person != null)
                        return await service.DeleteAsync(person) > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return false;
            }
        }
    }
}
