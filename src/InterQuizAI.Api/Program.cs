using InterQuizAI.Api.Features.Configuration;
using InterQuizAI.Api.Features.History;
using InterQuizAI.Api.Features.Languages;
using InterQuizAI.Api.Features.Quiz;
using InterQuizAI.Api.Infrastructure.Persistence;
using InterQuizAI.Api.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<InterQuizDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=interquiz.db"));

// Security
builder.Services.AddSingleton<IEncryptionService, AesEncryptionService>();

// HTTP Client for OpenAI
builder.Services.AddHttpClient();

// Services
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<ILanguagesService, LanguagesService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InterQuizDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors();
app.UseStaticFiles();

// Map API endpoints
app.MapGroup("/api/config").MapConfigurationEndpoints();
app.MapGroup("/api/languages").MapLanguagesEndpoints();
app.MapGroup("/api/quiz").MapQuizEndpoints();
app.MapGroup("/api/history").MapHistoryEndpoints();

// Fallback to index.html for SPA-like behavior
app.MapFallbackToFile("index.html");

app.Run();
