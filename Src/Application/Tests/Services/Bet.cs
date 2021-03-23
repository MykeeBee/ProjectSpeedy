using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Tests.Services
{
    public class Bet
    {
        [Test]
        public async System.Threading.Tasks.Task TestCreateValidAsync()
        {
            // Arrange
            var mockTest = new Mock<ProjectSpeedy.Services.IServiceBase>();
            var betService = new ProjectSpeedy.Services.Bet(mockTest.Object);
            var form = new ProjectSpeedy.Models.Bet.BetNew()
            {
                Name = "Test Bet"
            };
            mockTest.Setup(d => d.DocumetCreate(It.IsAny<ProjectSpeedy.Models.Bet.Bet>(), "bet"))
                .Returns(Task.FromResult("TestNewId"));

            // Act
            var test = await betService.CreateAsync("ProjectId", "ProblemId", form);

            // Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public async System.Threading.Tasks.Task TestCreateInValidAsync()
        {
            // Arrange
            var mockTest = new Mock<ProjectSpeedy.Services.IServiceBase>();
            var betService = new ProjectSpeedy.Services.Bet(mockTest.Object);
            var form = new ProjectSpeedy.Models.Bet.BetNew()
            {
                Name = "Test Bet"
            };
            mockTest.Setup(d => d.DocumetCreate(It.IsAny<ProjectSpeedy.Models.Bet.Bet>(), "bet"))
                .Returns(Task.FromResult(""));

            // Act
            var test = await betService.CreateAsync("ProjectId", "ProblemId", form);

            // Assert
            Assert.AreEqual(test, false);
        }
    }
}