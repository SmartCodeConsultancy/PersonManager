#region Namespace References
using Microsoft.AspNetCore.Mvc;
using Xunit;
using EntityFrameworkCore3Mock;
using UKParliament.CodeTest.Data.Context;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Pattern.WebAPI.SmartWrapper;
using System.Net;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Pattern.Service;
using Moq;
using System.Collections.Generic;
#endregion

namespace UKParliament.CodeTest.Tests.Controller
{
    public abstract class BaseControllerUnitTest : ControllerBase
    {
        protected DbContextMock<PersonManagerContext> MockPersonManagerContext { get; set; }

        protected Mock<SmartServiceFactory> MockSmartServiceFactory { get; set; }

        protected DbContextOptions<PersonManagerContext> Options { get; } = new DbContextOptionsBuilder<PersonManagerContext>().Options;

        protected void Setup_MockPersonManagerContext() => MockPersonManagerContext = new DbContextMock<PersonManagerContext>(Options);

        protected void Setup_MockSmartServiceFactory() => MockSmartServiceFactory = new Mock<SmartServiceFactory>(MockPersonManagerContext.Object);

        protected void Assert_Api_Response(ApiResponse<Person> response)
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.False(response.IsError);
        }

        protected ApiResponse<Person> GenerateTestApiResponse(OkObjectResult result,
            string message = default,
            bool isError = false)
        {
            var response = new ApiResponse<Person>();
            response.Message = message;
            response.StatusCode = (int)result.StatusCode;
            response.IsError = isError;
            response.Result = (Person)result.Value;
            response.Results = (ICollection<Person>)result.Value;
            return response;
        }
    }
}
