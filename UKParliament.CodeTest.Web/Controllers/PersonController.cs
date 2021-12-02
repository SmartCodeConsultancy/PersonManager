#region Namespace References
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Services;
#endregion

namespace UKParliament.CodeTest.Web.Controllers
{
    /// <summary>
    /// An API for person manager
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : BaseController<PersonController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="personService">Inject Person Service </param>
        /// <param name="logger">Inject logger</param>
        public PersonController(
            IPersonService personService,
            ILogger<PersonController> logger) : base(logger)
        {
            try
            {
                Service = personService;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        /// <summary>
        /// A collection of person
        /// </summary>
        /// <returns>A collection of person entity in JSON</returns>
        [HttpGet("Persons")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetPersonsAsync()
        {
            try
            {
                var result = await Service.GetPersonsAsync();
                if ((bool)result.IsError)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return NoContent();
            }
        }

        /// <summary>
        /// A person by its unique key value
        /// </summary>
        /// <param name="id">Unique key value</param>
        /// <returns>Person entity in JSON</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetPersonAsync(int id)
        {
            try
            {
                var result = await Service.GetPersonAsync(id);
                if ((bool)result.IsError)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return NoContent();
            }
        }

        /// <summary>
        /// An endpoint allow to create new person entity.
        /// </summary>
        /// <param name="person">Person object</param>
        /// <returns>New created person entity</returns>
        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] Person person)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Model state");

                var result = await Service.AddOrUpdateAsync(person);
                if ((bool)result.IsError)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return NoContent();
            }
        }

        /// <summary>
        /// An endpoint allow to edit person entity.
        /// </summary>
        /// <param name="person">Person object</param>
        /// <returns>Updated person entity</returns>
        [HttpPut]
        public virtual async Task<IActionResult> Edit([FromBody] Person person)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Model state");

                var result = await Service.AddOrUpdateAsync(person, person.Id);
                if ((bool)result.IsError)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return NoContent();
            }
        }

        /// <summary>
        /// An endpoint allow to remove person entity.
        /// </summary>
        /// <param name="id">Unique key value</param>
        /// <returns>True or False</returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Remove(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest($"Invalid key value : {id}");
                var result = await Service.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return NoContent();
            }
        }

        private IPersonService Service { get; set; }
    }
}
