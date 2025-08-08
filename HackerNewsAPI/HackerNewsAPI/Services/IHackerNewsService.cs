using HackerNewsAPI.Models;

namespace HackerNewsAPI.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<HackerNewsItem>> GetNewStoriesAsync(int page = 1, int pageSize = 20);
        Task<IEnumerable<HackerNewsItem>> SearchStoriesAsync(string query, int page = 1, int pageSize = 20);
    }
}