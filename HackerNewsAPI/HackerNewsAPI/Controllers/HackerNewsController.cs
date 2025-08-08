using Microsoft.AspNetCore.Mvc;
using HackerNewsAPI.Services;
using HackerNewsAPI.Models;

namespace HackerNewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;
        private readonly ILogger<HackerNewsController> _logger;

        public HackerNewsController(IHackerNewsService hackerNewsService, ILogger<HackerNewsController> logger)
        {
            _hackerNewsService = hackerNewsService;
            _logger = logger;
        }

        /// <summary>
        /// Get newest stories from Hacker News
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 20, max: 100)</param>
        /// <returns>List of newest stories</returns>
        [HttpGet("newstories")]
        public async Task<ActionResult<IEnumerable<HackerNewsItem>>> GetNewStories(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 20;
                if (pageSize > 100) pageSize = 100;

                var stories = await _hackerNewsService.GetNewStoriesAsync(page, pageSize);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching new stories");
                return StatusCode(500, "An error occurred while fetching stories");
            }
        }

        /// <summary>
        /// Search stories by title
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 20, max: 100)</param>
        /// <returns>List of matching stories</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<HackerNewsItem>>> SearchStories(
            [FromQuery] string query,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest("Query parameter is required");
                }

                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 20;
                if (pageSize > 100) pageSize = 100;

                var stories = await _hackerNewsService.SearchStoriesAsync(query, page, pageSize);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching stories");
                return StatusCode(500, "An error occurred while searching stories");
            }
        }
    }
}