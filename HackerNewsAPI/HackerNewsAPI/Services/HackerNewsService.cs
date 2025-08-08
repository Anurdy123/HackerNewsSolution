using System.Text.Json;
using HackerNewsAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HackerNewsService> _logger;
        private const string HackerNewsApiBaseUrl = "https://hacker-news.firebaseio.com/v0";
        private const string NewStoriesCacheKey = "NewStories";
        private const int CacheExpirationMinutes = 5;

        public HackerNewsService(HttpClient httpClient, IMemoryCache cache, ILogger<HackerNewsService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<HackerNewsItem>> GetNewStoriesAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                var stories = await GetNewStoriesFromCacheOrApiAsync();
                
                // Apply pagination
                var startIndex = (page - 1) * pageSize;
                return stories.Skip(startIndex).Take(pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching new stories");
                return Enumerable.Empty<HackerNewsItem>();
            }
        }

        public async Task<IEnumerable<HackerNewsItem>> SearchStoriesAsync(string query, int page = 1, int pageSize = 20)
        {
            try
            {
                var stories = await GetNewStoriesFromCacheOrApiAsync();
                
                // Filter stories based on query
                var filteredStories = stories.Where(s => 
                    !string.IsNullOrEmpty(s.Title) && 
                    s.Title.Contains(query, StringComparison.OrdinalIgnoreCase));
                
                // Apply pagination
                var startIndex = (page - 1) * pageSize;
                return filteredStories.Skip(startIndex).Take(pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching stories");
                return Enumerable.Empty<HackerNewsItem>();
            }
        }

        private async Task<IEnumerable<HackerNewsItem>> GetNewStoriesFromCacheOrApiAsync()
        {
            if (!_cache.TryGetValue(NewStoriesCacheKey, out IEnumerable<HackerNewsItem> cachedStories))
            {
                _logger.LogInformation("Fetching new stories from Hacker News API");
                cachedStories = await FetchNewStoriesFromApiAsync();
                
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(CacheExpirationMinutes));
                
                _cache.Set(NewStoriesCacheKey, cachedStories, cacheEntryOptions);
            }
            else
            {
                _logger.LogInformation("Fetching new stories from cache");
            }
            
            return cachedStories ?? Enumerable.Empty<HackerNewsItem>();
        }

        private async Task<IEnumerable<HackerNewsItem>> FetchNewStoriesFromApiAsync()
        {
            try
            {
                // Get the list of new story IDs
                var newStoriesResponse = await _httpClient.GetAsync($"{HackerNewsApiBaseUrl}/newstories.json");
                newStoriesResponse.EnsureSuccessStatusCode();
                
                var newStoryIds = await JsonSerializer.DeserializeAsync<int[]>(
                    await newStoriesResponse.Content.ReadAsStreamAsync());
                
                if (newStoryIds == null || !newStoryIds.Any())
                {
                    return Enumerable.Empty<HackerNewsItem>();
                }
                
                // Take only the first 200 stories to avoid overwhelming the API
                var limitedStoryIds = newStoryIds.Take(200);
                
                // Fetch details for each story
                var stories = new List<HackerNewsItem>();
                
                foreach (var storyId in limitedStoryIds)
                {
                    try
                    {
                        var storyResponse = await _httpClient.GetAsync($"{HackerNewsApiBaseUrl}/item/{storyId}.json");
                        storyResponse.EnsureSuccessStatusCode();
                        
                        var story = await JsonSerializer.DeserializeAsync<HackerNewsItem>(
                            await storyResponse.Content.ReadAsStreamAsync());
                        
                        if (story != null && story.Type == "story")
                        {
                            stories.Add(story);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error fetching story with ID {StoryId}", storyId);
                        // Continue with other stories even if one fails
                    }
                }
                
                return stories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching new stories from API");
                throw;
            }
        }
    }
}