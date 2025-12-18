# GitHub Portfolio API

CV Site - Portfolio application that connects to GitHub to display developer's projects.

## Project Structure

- **GitHubService** - Class Library with GitHub API integration using Octokit
- **GitHubPortfolio.API** - Web API that provides endpoints for portfolio and search

## Setup Instructions

### 1. Create a GitHub Personal Access Token

1. Go to GitHub Settings > Developer settings > Personal access tokens
2. Click "Generate new token (classic)"
3. Give it a name and select scopes: `repo`, `read:user`
4. Copy the generated token

### 2. Configure User Secrets

Run the following commands in the `GitHubPortfolio.API` folder:

```powershell
cd GitHubPortfolio.API
dotnet user-secrets init
dotnet user-secrets set "GitHub:UserName" "YOUR_GITHUB_USERNAME"
dotnet user-secrets set "GitHub:Token" "YOUR_GITHUB_TOKEN"
```

### 3. Run the Application

```powershell
cd GitHubPortfolio.API
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000`

## API Endpoints

### GET /api/github/portfolio
Returns the portfolio repositories for the configured GitHub user.

**Response:**
```json
[
  {
    "name": "repo-name",
    "description": "Repository description",
    "url": "https://github.com/username/repo-name",
    "homepage": "https://example.com",
    "languages": ["C#", "JavaScript"],
    "lastCommitDate": "2024-01-15T10:30:00Z",
    "stargazersCount": 10,
    "pullRequestsCount": 5,
    "forksCount": 2,
    "isPrivate": false
  }
]
```

### GET /api/github/search
Search public repositories on GitHub.

**Query Parameters:**
- `name` (optional) - Repository name to search for
- `language` (optional) - Programming language filter (e.g., "csharp", "javascript", "python")
- `userName` (optional) - GitHub username filter

**Example:**
```
GET /api/github/search?name=dotnet&language=csharp&userName=microsoft
```

## Features

- **Caching**: Portfolio data is cached for better performance
- **Smart Cache Invalidation**: Cache is invalidated when user has new GitHub activity
- **Decorator Pattern**: Caching is implemented using the Decorator pattern with Scrutor

## Technologies

- .NET 8
- Octokit (GitHub API client)
- Scrutor (Decorator pattern for DI)
- In-Memory Caching
"# CV-site" 
