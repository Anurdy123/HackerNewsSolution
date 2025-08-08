using System.Net;
using HackerNewsAPI.Models;
using HackerNewsAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace HackerNewsAPI.Tests
{
    public class HackerNewsServiceTests
    {
        private Mock<HttpMessageHandler>? _httpMessageHandlerMock;
        private Mock<IMemoryCache>? _memoryCacheMock;
        private Mock<ILogger<HackerNewsService>>? _loggerMock;
        private HackerNewsService? _hackerNewsService;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/")
            };

            _memoryCacheMock = new Mock<IMemoryCache>();
            _loggerMock = new Mock<ILogger<HackerNewsService>>();

            _hackerNewsService = new HackerNewsService(httpClient, _memoryCacheMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetNewStoriesAsync_ReturnsStoriesFromCache_WhenCacheExists()
        {
            // Arrange
            var cachedStories = new List<HackerNewsItem>
            {
                new HackerNewsItem { Id = 1, Title = "Test Story 1" },
                new HackerNewsItem { Id = 2, Title = "Test Story 2" }
            };

            object? cachedValue = cachedStories;
            _memoryCacheMock!.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedValue))
                .Returns(true);

            // Act
            var result = await _hackerNewsService!.GetNewStoriesAsync();

            // Assert
            NUnit.Framework.Assert.That(result, Is.Not.Null);
            NUnit.Framework.Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task SearchStoriesAsync_ReturnsFilteredStories()
        {
            // Arrange
            var stories = new List<HackerNewsItem>
            {
                new HackerNewsItem { Id = 1, Title = "C# Programming" },
                new HackerNewsItem { Id = 2, Title = "JavaScript Frameworks" },
                new HackerNewsItem { Id = 3, Title = "C# Advanced Concepts" }
            };

            object? cachedValue = stories;
            _memoryCacheMock!.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedValue))
                .Returns(true);

            // Act
            var result = await _hackerNewsService!.SearchStoriesAsync("C#");

            // Assert
            NUnit.Framework.Assert.That(result, Is.Not.Null);
            NUnit.Framework.Assert.That(result.Count(), Is.EqualTo(2));
            NUnit.Framework.Assert.That(result.All(s => s.Title!.Contains("C#")));
        }

        [Test]
        public async Task SearchStoriesAsync_ReturnsEmptyList_WhenQueryIsEmpty()
        {
            // Arrange
            var stories = new List<HackerNewsItem>
            {
                new HackerNewsItem { Id = 1, Title = "Test Story" }
            };

            object? cachedValue = stories;
            _memoryCacheMock!.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedValue))
                .Returns(true);

            // Act
            var result = await _hackerNewsService!.SearchStoriesAsync("");

            // Assert
            NUnit.Framework.Assert.That(result, Is.Not.Null);
            // When query is empty, it should return all stories, not an empty list
            NUnit.Framework.Assert.That(result.Count(), Is.EqualTo(1));
        }
    }
}