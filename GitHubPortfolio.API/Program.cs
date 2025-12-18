using GitHubService.Models;
using GitHubService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure GitHub settings from configuration (secrets.json in development)
builder.Services.Configure<GitHubSettings>(
    builder.Configuration.GetSection("GitHub"));

// Add memory cache
builder.Services.AddMemoryCache();

// Register GitHubService with caching decorator using Scrutor
builder.Services.AddScoped<IGitHubService, GitHubPortfolioService>();
builder.Services.Decorate<IGitHubService, CachedGitHubService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
