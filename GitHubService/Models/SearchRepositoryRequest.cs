namespace GitHubService.Models;

public class SearchRepositoryRequest
{
    public string? Name { get; set; }
    public string? Language { get; set; }
    public string? UserName { get; set; }
}
