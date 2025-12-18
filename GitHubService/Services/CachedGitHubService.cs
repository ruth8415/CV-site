using GitHubService.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GitHubService.Services;

public class CachedGitHubService : IGitHubService
{
    private readonly IGitHubService _innerService;
    private readonly IMemoryCache _cache;
    private const string PortfolioCacheKey = "Portfolio";
    private const string LastActivityCacheKey = "LastActivity";
    private DateTime? _cachedAtTime;

    public CachedGitHubService(IGitHubService innerService, IMemoryCache cache)
    {
        _innerService = innerService;
        _cache = cache;
    }

    public async Task<List<RepositoryInfo>> GetPortfolioAsync()
    {
        // Check if user has new activity since last cache
        var lastActivity = await GetLastUserActivityAsync();
        
        if (_cachedAtTime.HasValue && lastActivity.HasValue && lastActivity > _cachedAtTime)
        {
            // User has new activity, invalidate cache
            _cache.Remove(PortfolioCacheKey);
            _cachedAtTime = null;
        }

        if (_cache.TryGetValue(PortfolioCacheKey, out List<RepositoryInfo>? cachedPortfolio) && cachedPortfolio != null)
        {
            return cachedPortfolio;
        }

        var portfolio = await _innerService.GetPortfolioAsync();
        
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        _cache.Set(PortfolioCacheKey, portfolio, cacheOptions);
        _cachedAtTime = DateTime.UtcNow;

        return portfolio;
    }

    public async Task<List<RepositoryInfo>> SearchRepositoriesAsync(SearchRepositoryRequest request)
    {
        // Search is not cached because it's based on different parameters each time
        return await _innerService.SearchRepositoriesAsync(request);
    }

    public async Task<DateTime?> GetLastUserActivityAsync()
    {
        if (_cache.TryGetValue(LastActivityCacheKey, out DateTime? cachedActivity))
        {
            return cachedActivity;
        }

        var lastActivity = await _innerService.GetLastUserActivityAsync();
        
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1)); // Short cache for activity check

        _cache.Set(LastActivityCacheKey, lastActivity, cacheOptions);

        return lastActivity;
    }
}
