using GitHubService.Models;
using Microsoft.Extensions.Options;
using Octokit;

namespace GitHubService.Services;

public class GitHubPortfolioService : IGitHubService
{
    private readonly GitHubClient _client;
    private readonly GitHubSettings _options;

    public GitHubPortfolioService(IOptions<GitHubSettings> options)
    {
        _options = options.Value;
        _client = new GitHubClient(new ProductHeaderValue("my-github-app"));
        
        // Authenticate if token is provided
        if (!string.IsNullOrEmpty(_options.Token))
        {
            _client.Credentials = new Credentials(_options.Token);
        }
    }

    public async Task<List<RepositoryInfo>> GetPortfolioAsync()
    {
        var repositories = await _client.Repository.GetAllForUser(_options.UserName);
        var result = new List<RepositoryInfo>();

        foreach (var repo in repositories)
        {
            var repoInfo = new RepositoryInfo
            {
                Name = repo.Name,
                Description = repo.Description,
                Url = repo.HtmlUrl,
                Homepage = repo.Homepage,
                StargazersCount = repo.StargazersCount,
                ForksCount = repo.ForksCount,
                IsPrivate = repo.Private
            };

            // Get languages for this repository
            try
            {
                var languages = await _client.Repository.GetAllLanguages(_options.UserName, repo.Name);
                repoInfo.Languages = languages.Select(l => l.Name).ToList();
            }
            catch
            {
                repoInfo.Languages = new List<string>();
            }

            // Get last commit date
            try
            {
                var commits = await _client.Repository.Commit.GetAll(_options.UserName, repo.Name, new CommitRequest(), new ApiOptions { PageCount = 1, PageSize = 1 });
                if (commits.Any())
                {
                    repoInfo.LastCommitDate = commits.First().Commit.Author.Date.DateTime;
                }
            }
            catch
            {
                repoInfo.LastCommitDate = null;
            }

            // Get pull requests count
            try
            {
                var pullRequests = await _client.PullRequest.GetAllForRepository(_options.UserName, repo.Name, new PullRequestRequest { State = ItemStateFilter.All });
                repoInfo.PullRequestsCount = pullRequests.Count;
            }
            catch
            {
                repoInfo.PullRequestsCount = 0;
            }

            result.Add(repoInfo);
        }

        return result;
    }

    public async Task<List<RepositoryInfo>> SearchRepositoriesAsync(SearchRepositoryRequest request)
    {
        var searchRequest = new SearchRepositoriesRequest();

        // Build search term
        var searchTerm = request.Name ?? "";
        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchRequest.Term = searchTerm;
        }

        // Add language filter
        if (!string.IsNullOrEmpty(request.Language))
        {
            var language = GetLanguageEnum(request.Language);
            if (language.HasValue)
            {
                searchRequest.Language = language.Value;
            }
        }

        // Add user filter
        if (!string.IsNullOrEmpty(request.UserName))
        {
            searchRequest.User = request.UserName;
        }

        var result = await _client.Search.SearchRepo(searchRequest);

        return result.Items.Select(repo => new RepositoryInfo
        {
            Name = repo.Name,
            Description = repo.Description,
            Url = repo.HtmlUrl,
            Homepage = repo.Homepage,
            StargazersCount = repo.StargazersCount,
            ForksCount = repo.ForksCount,
            IsPrivate = repo.Private,
            Languages = new List<string>()
        }).ToList();
    }

    public async Task<DateTime?> GetLastUserActivityAsync()
    {
        try
        {
            var events = await _client.Activity.Events.GetAllUserPerformed(_options.UserName);
            if (events.Any())
            {
                return events.First().CreatedAt.DateTime;
            }
        }
        catch
        {
            // Ignore errors
        }

        return null;
    }

    private Language? GetLanguageEnum(string language)
    {
        return language.ToLower() switch
        {
            "csharp" or "c#" => Language.CSharp,
            "javascript" or "js" => Language.JavaScript,
            "typescript" or "ts" => Language.TypeScript,
            "python" => Language.Python,
            "java" => Language.Java,
            "go" => Language.Go,
            "rust" => Language.Rust,
            "ruby" => Language.Ruby,
            "php" => Language.Php,
            "cpp" or "c++" => Language.Cpp,
            "c" => Language.C,
            "html" => Language.Html,
            "css" => Language.Css,
            "swift" => Language.Swift,
            "kotlin" => Language.Kotlin,
            _ => null
        };
    }
}
