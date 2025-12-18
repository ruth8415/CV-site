using GitHubService.Models;

namespace GitHubService.Services;

public interface IGitHubService
{
    Task<List<RepositoryInfo>> GetPortfolioAsync();
    Task<List<RepositoryInfo>> SearchRepositoriesAsync(SearchRepositoryRequest request);
    Task<DateTime?> GetLastUserActivityAsync();
}
