#region Namespace References
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Pattern.WebAPI.SmartWrapper;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Tests.Logging;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;
#endregion

namespace UKParliament.CodeTest.Tests.Controller
{
    public class PersonControllerUnitTests : BaseControllerUnitTest
    {
        public PersonControllerUnitTests()
        {
            Setup_MockPersonManagerContext();
            Setup_MockSmartServiceFactory();
            Setup_MockPersonService();
            Setup_MockPersonController();
        }
        private Mock<AbstractLogger<PersonController>> MockPersonControllerLogger => new Mock<AbstractLogger<PersonController>>();

        private Mock<AbstractLogger<PersonService>> MockPersonServiceLogger => new Mock<AbstractLogger<PersonService>>();

        private Mock<PersonController> MockPersonController { get; set; }

        private Mock<PersonService> MockPersonService { get; set; }

        [Fact]
        public void Ensure_Person_DbSet_Is_Not_Null()
        {
            //ARRANGE
            var MockDbSet = new Mock<DbSet<Person>>();
            MockDbSet.Setup(d => d.AddRange(GetPeople()));

            MockPersonManagerContext.Setup(x => x.People).Returns(MockDbSet.Object);

            Assert.NotNull(MockPersonManagerContext.Object.People);
        }

        [Fact]
        public void Ensure_MockPersonManagerContext_Initialized() => Assert.NotNull(MockPersonManagerContext);

        [Fact]
        public void Ensure_MockPersonManagerContext_Object_Initialized() => Assert.NotNull(MockPersonManagerContext.Object);

        [Fact]
        public void Ensure_MockPersonService_Initialized() => Assert.NotNull(MockPersonService);

        [Fact]
        public void Ensure_MockPersonService_Object_Initialized() => Assert.NotNull(MockPersonService.Object);

        [Fact]
        public void Ensure_MockPersonController_Initialized() => Assert.NotNull(MockPersonController);

        [Fact]
        public void Ensure_MockPersonController_Object_Initialized() => Assert.NotNull(MockPersonController.Object);

        [Fact]
        public async Task PersonController_GetPersonsAsync_Return_Result()
        {
            //ARRANGE
            var MockApiResponse = new Mock<ApiResponse<Person>>();
            MockApiResponse.Object.IsError = false;
            MockApiResponse.Object.Message = "success";
            MockApiResponse.Object.StatusCode = (int)HttpStatusCode.OK;
            MockApiResponse.Object.ExceptionMessage = string.Empty;
            MockApiResponse.Object.Results = GetPeople();
            MockPersonController.Setup(x => x.GetPersonsAsync()).ReturnsAsync(Ok(MockApiResponse.Object));

            //ACT
            var apiResponse = await MockPersonController.Object.GetPersonsAsync();
            var okObjectResult = apiResponse as OkObjectResult;

            //ASSERT
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task PersonController_GetPersonAsync_Return_Result()
        {
            //ARRANGE
            var MockApiResponse = new Mock<ApiResponse<Person>>();
            MockApiResponse.Object.IsError = false;
            MockApiResponse.Object.Message = "success";
            MockApiResponse.Object.StatusCode = (int)HttpStatusCode.OK;
            MockApiResponse.Object.ExceptionMessage = string.Empty;
            var entity = new Mock<Person>();
            var entityValue = GetPeople().First();
            entity.Object.Id = entityValue.Id;
            entity.Object.Name = entityValue.Name;
            entity.Object.Address = entityValue.Address;
            entity.Object.DateOfBirth = entityValue.DateOfBirth;
            entity.Object.Email = entityValue.Email;
            entity.Object.Gender = entityValue.Gender;
            entity.Object.Phone = entityValue.Phone;
            MockPersonController.Setup(x => x.GetPersonAsync(It.IsAny<int>())).ReturnsAsync(Ok(entity.Object));

            // ACT
            var apiResponse = await MockPersonController.Object.GetPersonAsync(3);
            var okObjectResult = apiResponse as OkObjectResult;

            //ASSERT
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task PersonController_Delete_Async()
        {
            // ARRANGE
            MockPersonController.Setup(x => x.Remove(It.IsAny<int>())).ReturnsAsync(Ok(true));

            // ACT
            var apiResponse = await MockPersonController.Object.Remove(2);
            var okObjectResult = apiResponse as OkObjectResult;

            //ASSERT
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task PersonController_Create_Async()
        {
            // ARRANGE
            var newEntity = new Mock<Person>();
            newEntity.Object.Id = AddNew.Id;
            newEntity.Object.Name = AddNew.Name;
            newEntity.Object.DateOfBirth = AddNew.DateOfBirth;
            newEntity.Object.Address = AddNew.Address;
            newEntity.Object.Email = AddNew.Email;
            newEntity.Object.Gender = AddNew.Gender;
            newEntity.Object.Phone = AddNew.Phone;
            MockPersonController.Setup(x => x.Create(It.IsIn(newEntity.Object))).ReturnsAsync(Ok(AddNew));

            //ACT
            var apiResponse = await MockPersonController.Object.Create(newEntity.Object);
            var okObjectResult = apiResponse as OkObjectResult;

            //ASSERT
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task PersonController_Edit_Async()
        {
            // ARRANGE
            var editEntity = new Mock<Person>();
            var entity = GetPeople().First(x => x.Id == 3);
            editEntity.Object.Id = entity.Id;
            editEntity.Object.Name = entity.Name;
            editEntity.Object.Address = entity.Address;
            editEntity.Object.DateOfBirth = entity.DateOfBirth;
            editEntity.Object.Email = entity.Email;
            editEntity.Object.Gender = entity.Gender;
            editEntity.Object.Phone = entity.Phone;
            editEntity.Object.Name = $"Edited {editEntity.Object.Name}";
            MockPersonController.Setup(x => x.Edit(It.IsIn(editEntity.Object))).ReturnsAsync(Ok(1));

            // ACT
            var apiResponse = await MockPersonController.Object.Edit(editEntity.Object);
            var okObjectResult = apiResponse as OkObjectResult;

            //ASSERT
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }


        private void Setup_MockPersonService()
        {
            MockPersonService = new Mock<PersonService>(MockSmartServiceFactory.Object, MockPersonServiceLogger.Object);
        }

        private void Setup_MockPersonController()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            MockPersonController =
                new Mock<PersonController>(MockPersonService.Object,
                MockPersonControllerLogger.Object);
            MockPersonController.Object.ControllerContext = controllerContext;
        }

        private Person AddNew
        {
           get
            {
                return new Person
                {
                    Id = 5,
                    Name = "Mike Lambo",
                    Address = "7A Clapton Mose, York, YO1 0HS",
                    DateOfBirth = DateTime.Parse("25/09/1978"),
                    Gender = "Male",
                    Email = "Lambo_Mike@Outlook.com",
                    Phone = "0796471672"
                };
            }
        }

        private List<Person> GetPeople()
        {
            return new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "Jay Patel",
                    Address = "15 Woods Drive, Leicester, LE2 3TY",
                    DateOfBirth = DateTime.Parse("05/12/1980"),
                    Gender = "Male",
                    Email = "Patel.Jay1@outlook.com",
                    Phone = "0789145895"
                },

                new Person
                {
                    Id = 2,
                    Name = "Alex Clark",
                    Address = "1 Cotton Avenue, Nottingham, NG1 3KU",
                    DateOfBirth = DateTime.Parse("12/06/1977"),
                    Gender = "Not Specified",
                    Email = "Alex.Clark@Yahoo.com",
                    Phone = "0777591279"
                },

                new Person
                {
                    Id = 3,
                    Name = "Ahmad Khan",
                    Address = "12 Derby Road, Sheffield, S1 1GE",
                    DateOfBirth = DateTime.Parse("2/07/1982"),
                    Gender = "Male",
                    Email = "Ahmad.Khan2@Yahoo.com",
                    Phone = "0778551273"
                },

                new Person
                {
                    Id = 4,
                    Name = "Rita Jain",
                    Address = "12 Lime Avenue, Derby, DE1 1GR",
                    DateOfBirth = DateTime.Parse("28/03/1979"),
                    Gender = "Female",
                    Email = "Jain.Rita@bt.com",
                    Phone = "0786571272"
                }
            };
        }
    }
}
