namespace GitHubService.Models;

public class RepositoryInfo
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Homepage { get; set; }
    public List<string> Languages { get; set; } = new();
    public DateTime? LastCommitDate { get; set; }
    public int StargazersCount { get; set; }
    public int PullRequestsCount { get; set; }
    public int ForksCount { get; set; }
    public bool IsPrivate { get; set; }
}
