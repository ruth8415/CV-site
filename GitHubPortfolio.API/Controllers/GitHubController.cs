using GitHubService.Models;
using GitHubService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitHubPortfolio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GitHubController : ControllerBase
{
    private readonly IGitHubService _gitHubService;

    public GitHubController(IGitHubService gitHubService)
    {
        _gitHubService = gitHubService;
    }

    /// <summary>
    /// Returns the portfolio repositories for the configured GitHub user
    /// </summary>
    [HttpGet("portfolio")]
    public async Task<ActionResult<List<RepositoryInfo>>> GetPortfolio()
    {
        try
        {
            var repositories = await _gitHubService.GetPortfolioAsync();
            return Ok(repositories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching portfolio: {ex.Message}");
        }
    }

    /// <summary>
    /// Search public repositories on GitHub
    /// </summary>
    /// <param name="name">Repository name to search for (optional)</param>
    /// <param name="language">Programming language filter (optional)</param>
    /// <param name="userName">GitHub username filter (optional)</param>
    [HttpGet("search")]
    public async Task<ActionResult<List<RepositoryInfo>>> SearchRepositories(
        [FromQuery] string? name = null,
        [FromQuery] string? language = null,
        [FromQuery] string? userName = null)
    {
        try
        {
            var request = new SearchRepositoryRequest
            {
                Name = name,
                Language = language,
                UserName = userName
            };

            var repositories = await _gitHubService.SearchRepositoriesAsync(request);
            return Ok(repositories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error searching repositories: {ex.Message}");
        }
    }
}
