using HackerNewsAPI.Controllers;
using HackerNewsAPI.Models;
using HackerNewsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace HackerNewsAPI.Tests
{
    public class HackerNewsControllerTests
    {
        private Mock<IHackerNewsService>? _hackerNewsServiceMock;
        private Mock<ILogger<HackerNewsController>>? _loggerMock;
        private HackerNewsController? _controller;

        [SetUp]
        public void Setup()
        {
            _hackerNewsServiceMock = new Mock<IHackerNewsService>();
            _loggerMock = new Mock<ILogger<HackerNewsController>>();
            _controller = new HackerNewsController(_hackerNewsServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetNewStories_ReturnsOkResult_WithStories()
        {
            // Arrange
            var stories = new List<HackerNewsItem>
            {
                new HackerNewsItem { Id = 1, Title = "Test Story 1" },
                new HackerNewsItem { Id = 2, Title = "Test Story 2" }
            };

            _hackerNewsServiceMock!.Setup(service => service.GetNewStoriesAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(stories);

            // Act
            var result = await _controller!.GetNewStories();

            // Assert
            NUnit.Framework.Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            NUnit.Framework.Assert.That(okResult?.Value, Is.EqualTo(stories));
        }

        [Test]
        public async Task SearchStories_ReturnsOkResult_WithStories()
        {
            // Arrange
            var stories = new List<HackerNewsItem>
            {
                new HackerNewsItem { Id = 1, Title = "C# Programming" },
                new HackerNewsItem { Id = 2, Title = "C# Advanced Concepts" }
            };

            _hackerNewsServiceMock!.Setup(service => service.SearchStoriesAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(stories);

            // Act
            var result = await _controller!.SearchStories("C#");

            // Assert
            NUnit.Framework.Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            NUnit.Framework.Assert.That(okResult?.Value, Is.EqualTo(stories));
        }

        [Test]
        public async Task SearchStories_ReturnsBadRequest_WhenQueryIsEmpty()
        {
            // Act
            var result = await _controller!.SearchStories("");

            // Assert
            NUnit.Framework.Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetNewStories_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _hackerNewsServiceMock!.Setup(service => service.GetNewStoriesAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller!.GetNewStories();

            // Assert
            NUnit.Framework.Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
            var objectResult = result.Result as ObjectResult;
            NUnit.Framework.Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task SearchStories_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _hackerNewsServiceMock!.Setup(service => service.SearchStoriesAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller!.SearchStories("test");

            // Assert
            NUnit.Framework.Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
            var objectResult = result.Result as ObjectResult;
            NUnit.Framework.Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
        }
    }
}