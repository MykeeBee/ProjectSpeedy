using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ProjectSpeedy.Controllers;

namespace Tests.Controllers
{
    [TestFixture]
    public class Problem
    {
        private Mock<ILogger<ProblemController>> _logger;

        private Mock<ProjectSpeedy.Services.IServiceBase> _serviceBase;

        private Mock<ProjectSpeedy.Services.Problem> _problemService;

        private ProjectSpeedy.Controllers.ProblemController _controller;

        [SetUp]
        public void init()
        {
            this._serviceBase = new Mock<ProjectSpeedy.Services.IServiceBase>();
            this._logger = new Mock<ILogger<ProblemController>>();
            this._problemService = new Mock<ProjectSpeedy.Services.Problem>(this._serviceBase.Object);
            this._controller = new ProjectSpeedy.Controllers.ProblemController(this._logger.Object, this._problemService.Object);
        }

        /// <summary>
        /// Gets a document successfully.
        /// </summary>
        [Test]
        public async void Get()
        {
            using (var stream = new MemoryStream())
            {
                using (var streamBets = new MemoryStream())
                {
                    // Arrange
                    // Problem Object
                    await JsonSerializer.SerializeAsync(stream, new ProjectSpeedy.Models.Problem.Problem()
                    {
                        ProjectId = "ProjectId",
                        Name = "Problem"
                    });
                    stream.Position = 0;
                    using var reader = new StreamReader(stream);
                    string content = await reader.ReadToEndAsync();
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Content = new StringContent(content);
                    this._serviceBase.Setup(d => d.GetDocument("problem:ProblemId"))
                        .Returns(Task.FromResult(response.Content));

                    // List of bets
                    await JsonSerializer.SerializeAsync(streamBets, new ProjectSpeedy.Models.CouchDb.View.ViewResult()
                    {
                        total_rows = 1,
                        offset = 0,
                        rows = new List<ProjectSpeedy.Models.CouchDb.View.ListItem>(){
                            new ProjectSpeedy.Models.CouchDb.View.ListItem(){
                                id= "project:ProjectId",
                                value= new ProjectSpeedy.Models.CouchDb.View.ListItemValue(){
                                    id= "bet:e5273e69704d8c4ee3f8b50c6500d053",
                                    name = "Bet Name"
                                }
                            }
                        }
                    });
                    streamBets.Position = 0;
                    using var readerBets = new StreamReader(streamBets);
                    string contentBets = await readerBets.ReadToEndAsync();
                    HttpResponseMessage responseBets = new HttpResponseMessage();
                    responseBets.Content = new StringContent(contentBets);
                    this._serviceBase.Setup(d => d.GetView("bet", "bets", "bets", "problem:ProblemId", "problem:ProblemId"))
                        .Returns(Task.FromResult(responseBets.Content));

                    // Act
                    var test = await this._controller.GetAsync("ProjectId", "ProblemId");

                    // Assert
                    var result = test.Result as OkObjectResult;
                    Assert.IsNull(test.Value);
                    Assert.AreEqual(result.StatusCode, 200);
                    Assert.AreEqual(((ProjectSpeedy.Models.Problem.Problem) result.Value).Name, "Problem");
                }
            }
        }

        /// <summary>
        /// Tries to load a document which does not exist.
        /// </summary>
        [Test]
        public async void GetNotfound()
        {
            // Throws an error when calling the view
            this._serviceBase.Setup(d => d.GetDocument(It.IsAny<string>()))
                .Throws(new HttpRequestException("Document not found",new System.Exception("Document not found"), System.Net.HttpStatusCode.NotFound));

            // Act
            var test = await this._controller.GetAsync("ProblemId","ProjectId");

            // Assert
            // Taken from https://stackoverflow.com/questions/51489111/how-to-unit-test-with-actionresultt
            var result = test.Result as NotFoundResult;
            Assert.IsNull(test.Value);
            Assert.AreEqual(result.StatusCode, 404);
        }

        /// <summary>
        /// Tries to get a problem but gets a non not found exception when trying to load it.
        /// </summary>
        [Test]
        public async void GetExceptionHttpOther()
        {
            // Throws an error when calling the view
            this._serviceBase.Setup(d => d.GetDocument(It.IsAny<string>()))
                .Throws(new HttpRequestException("Exception",new System.Exception("Exeption"), System.Net.HttpStatusCode.BadRequest));

            // Act
            var test = await this._controller.GetAsync("ProblemId","ProjectId");

            // Assert
            // Taken from https://stackoverflow.com/questions/51489111/how-to-unit-test-with-actionresultt
            var result = test.Result as ObjectResult;
            Assert.IsNull(test.Value);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /// <summary>
        /// Tries to get a problem but gets an exception when trying to load it.
        /// </summary>
        [Test]
        public async void GetException()
        {
            // Throws an error when calling the view
            this._serviceBase.Setup(d => d.GetDocument(It.IsAny<string>()))
                .Throws(new System.Exception("Exception"));

            // Act
            var test = await this._controller.GetAsync("ProblemId","ProjectId");

            // Assert
            // Taken from https://stackoverflow.com/questions/51489111/how-to-unit-test-with-actionresultt
            var result = test.Result as ObjectResult;
            Assert.IsNull(test.Value);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /// <summary>
        /// Tries to load a problem with a project Id which is not linked to the problem.
        /// </summary>
        [Test]
        public async void GetInvalidIds()
        {
            using (var stream = new MemoryStream())
            {
                using (var streamBets = new MemoryStream())
                {
                    // Arrange
                    // Problem Object
                    await JsonSerializer.SerializeAsync(stream, new ProjectSpeedy.Models.Problem.Problem()
                    {
                        ProjectId = "DiferentProjectId"
                    });
                    stream.Position = 0;
                    using var reader = new StreamReader(stream);
                    string content = await reader.ReadToEndAsync();
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Content = new StringContent(content);
                    this._serviceBase.Setup(d => d.GetDocument("problem:ProblemId"))
                        .Returns(Task.FromResult(response.Content));

                    // List of bets
                    await JsonSerializer.SerializeAsync(streamBets, new ProjectSpeedy.Models.CouchDb.View.ViewResult()
                    {
                        total_rows = 1,
                        offset = 0,
                        rows = new List<ProjectSpeedy.Models.CouchDb.View.ListItem>(){
                            new ProjectSpeedy.Models.CouchDb.View.ListItem(){
                                id= "ProjectId",
                                value= new ProjectSpeedy.Models.CouchDb.View.ListItemValue(){
                                    id= "bet:e5273e69704d8c4ee3f8b50c6500d053",
                                    name = "Bet Name"
                                }
                            }
                        }
                    });
                    streamBets.Position = 0;
                    using var readerBets = new StreamReader(streamBets);
                    string contentBets = await readerBets.ReadToEndAsync();
                    HttpResponseMessage responseBets = new HttpResponseMessage();
                    responseBets.Content = new StringContent(contentBets);
                    this._serviceBase.Setup(d => d.GetView("bet", "bets", "bets", "problem:ProblemId", "problem:ProblemId"))
                        .Returns(Task.FromResult(responseBets.Content));

                    // Act
                    var test = await this._controller.GetAsync("ProjectId", "ProblemId");

                    // Assert
                    var result = test.Result as NotFoundResult;
                    Assert.IsNull(test.Value);
                    Assert.AreEqual(result.StatusCode, 404);
                }
            }
        }

        /// <summary>
        /// Creating a new problem successfully.
        /// </summary>
        [Test]
        public async void Put()
        {
            // Throws an error when calling the view
            this._serviceBase.Setup(d => d.DocumetCreate(It.IsAny<object>(),"problem"))
                .Returns(Task.FromResult("NewId"));

            // Act
            var test = await this._controller.PutAsync(new ProjectSpeedy.Models.Problem.ProblemNew(){
                Name = "New Problem"
            }, "ProjectId");

            // Assert
            // Taken from https://stackoverflow.com/questions/51489111/how-to-unit-test-with-actionresultt
            var result = test as AcceptedResult;
            Assert.AreEqual(result.StatusCode, 202);
        }

        /// <summary>
        /// We attempt to create a new problem without sending form data.
        /// </summary>
        [Test]
        public async void PutNullForm()
        {
            // Throws an error when calling the view
            this._serviceBase.Setup(d => d.DocumetCreate(It.IsAny<object>(),"problem"))
                .Returns(Task.FromResult("NewId"));

            // Act
            var test = await this._controller.PutAsync(null, "ProjectId");

            // Assert
            // Taken from https://stackoverflow.com/questions/51489111/how-to-unit-test-with-actionresultt
            var result = test as BadRequestResult;
            Assert.AreEqual(result.StatusCode, 400);
        }

        /// <summary>
        /// We attempt to create a valid problem but no ID is sent back.
        /// </summary>
        [Test]
        public async void PutNoCreate()
        {
            // Throws an error when calling the view
            this._serviceBase.Setup(d => d.DocumetCreate(It.IsAny<object>(),"problem"))
                .Returns(Task.FromResult(""));

            // Act
            var test = await this._controller.PutAsync(new ProjectSpeedy.Models.Problem.ProblemNew(){
                Name = "New Problem"
            }, "ProjectId");

            // Assert
            // Taken from https://stackoverflow.com/questions/51489111/how-to-unit-test-with-actionresultt
            var result = test as BadRequestResult;
            Assert.AreEqual(result.StatusCode, 400);
        }
    }
}