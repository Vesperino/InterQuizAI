using InterQuizAI.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InterQuizAI.Api.Infrastructure.Persistence;

public class InterQuizDbContext : DbContext
{
    public InterQuizDbContext(DbContextOptions<InterQuizDbContext> options) : base(options)
    {
    }

    public DbSet<AppSettings> AppSettings => Set<AppSettings>();
    public DbSet<ApiConfiguration> ApiConfigurations => Set<ApiConfiguration>();
    public DbSet<TechnologyType> TechnologyTypes => Set<TechnologyType>();
    public DbSet<ProgrammingLanguage> ProgrammingLanguages => Set<ProgrammingLanguage>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<DifficultyLevel> DifficultyLevels => Set<DifficultyLevel>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<QuizSession> QuizSessions => Set<QuizSession>();
    public DbSet<QuizSessionQuestion> QuizSessionQuestions => Set<QuizSessionQuestion>();
    public DbSet<QuizResult> QuizResults => Set<QuizResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // AppSettings
        modelBuilder.Entity<AppSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // ApiConfiguration
        modelBuilder.Entity<ApiConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // TechnologyType
        modelBuilder.Entity<TechnologyType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // ProgrammingLanguage
        modelBuilder.Entity<ProgrammingLanguage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasOne(e => e.TechnologyType)
                .WithMany(t => t.Languages)
                .HasForeignKey(e => e.TechnologyTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.Name, e.TechnologyTypeId }).IsUnique();
            entity.HasOne(e => e.TechnologyType)
                .WithMany(t => t.Categories)
                .HasForeignKey(e => e.TechnologyTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // DifficultyLevel
        modelBuilder.Entity<DifficultyLevel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Question
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ExternalId).IsUnique();
            entity.HasIndex(e => new { e.LanguageId, e.CategoryId, e.DifficultyLevelId });

            entity.HasOne(e => e.Language)
                .WithMany(l => l.Questions)
                .HasForeignKey(e => e.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Questions)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DifficultyLevel)
                .WithMany(d => d.Questions)
                .HasForeignKey(e => e.DifficultyLevelId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Answer
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.QuestionId);

            entity.HasOne(e => e.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // QuizSession
        modelBuilder.Entity<QuizSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SessionGuid).IsUnique();

            entity.HasOne(e => e.Language)
                .WithMany(l => l.QuizSessions)
                .HasForeignKey(e => e.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.QuizSessions)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DifficultyLevel)
                .WithMany(d => d.QuizSessions)
                .HasForeignKey(e => e.DifficultyLevelId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // QuizSessionQuestion
        modelBuilder.Entity<QuizSessionQuestion>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Session)
                .WithMany(s => s.SessionQuestions)
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Question)
                .WithMany(q => q.SessionQuestions)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // QuizResult
        modelBuilder.Entity<QuizResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SessionId);

            entity.HasOne(e => e.Session)
                .WithMany(s => s.Results)
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Question)
                .WithMany(q => q.Results)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.SelectedAnswer)
                .WithMany(a => a.Results)
                .HasForeignKey(e => e.SelectedAnswerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Seed Data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Technology Types
        modelBuilder.Entity<TechnologyType>().HasData(
            new TechnologyType { Id = 1, Name = "backend", DisplayName = "Backend" },
            new TechnologyType { Id = 2, Name = "frontend", DisplayName = "Frontend" }
        );

        // Difficulty Levels
        modelBuilder.Entity<DifficultyLevel>().HasData(
            new DifficultyLevel { Id = 1, Name = "junior", DisplayName = "Junior", Description = "0-2 lata doświadczenia, podstawy, składnia", SortOrder = 1 },
            new DifficultyLevel { Id = 2, Name = "mid", DisplayName = "Mid", Description = "2-5 lat doświadczenia, praktyczne scenariusze, best practices", SortOrder = 2 },
            new DifficultyLevel { Id = 3, Name = "senior", DisplayName = "Senior", Description = "5-8 lat doświadczenia, zaawansowane koncepty, optymalizacja", SortOrder = 3 },
            new DifficultyLevel { Id = 4, Name = "tech_lead", DisplayName = "Tech Lead", Description = "8+ lat doświadczenia, leadership techniczny, projektowanie systemów", SortOrder = 4 },
            new DifficultyLevel { Id = 5, Name = "architect", DisplayName = "Architect IT", Description = "10+ lat doświadczenia, architektura enterprise, skalowalność", SortOrder = 5 }
        );

        // Backend Languages
        modelBuilder.Entity<ProgrammingLanguage>().HasData(
            new ProgrammingLanguage { Id = 1, Name = "csharp", DisplayName = "C# / .NET", TechnologyTypeId = 1 },
            new ProgrammingLanguage { Id = 2, Name = "java", DisplayName = "Java / Spring", TechnologyTypeId = 1 },
            new ProgrammingLanguage { Id = 3, Name = "python", DisplayName = "Python", TechnologyTypeId = 1 },
            new ProgrammingLanguage { Id = 4, Name = "nodejs", DisplayName = "Node.js / Express", TechnologyTypeId = 1 },
            new ProgrammingLanguage { Id = 5, Name = "go", DisplayName = "Go", TechnologyTypeId = 1 },
            new ProgrammingLanguage { Id = 6, Name = "rust", DisplayName = "Rust", TechnologyTypeId = 1 },
            new ProgrammingLanguage { Id = 7, Name = "php", DisplayName = "PHP", TechnologyTypeId = 1 },
            new ProgrammingLanguage { Id = 8, Name = "ruby", DisplayName = "Ruby", TechnologyTypeId = 1 }
        );

        // Frontend Languages
        modelBuilder.Entity<ProgrammingLanguage>().HasData(
            new ProgrammingLanguage { Id = 9, Name = "vue3", DisplayName = "Vue 3", TechnologyTypeId = 2 },
            new ProgrammingLanguage { Id = 10, Name = "react", DisplayName = "React", TechnologyTypeId = 2 },
            new ProgrammingLanguage { Id = 11, Name = "angular", DisplayName = "Angular", TechnologyTypeId = 2 },
            new ProgrammingLanguage { Id = 12, Name = "svelte", DisplayName = "Svelte", TechnologyTypeId = 2 },
            new ProgrammingLanguage { Id = 13, Name = "typescript", DisplayName = "TypeScript (ogólne)", TechnologyTypeId = 2 },
            new ProgrammingLanguage { Id = 14, Name = "javascript", DisplayName = "JavaScript (ogólne)", TechnologyTypeId = 2 }
        );

        // Backend Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "fundamentals", DisplayName = "Fundamenty języka", Description = "Typy danych, OOP, memory management, async/await, generics, LINQ", TechnologyTypeId = 1, AllowsHint = false },
            new Category { Id = 2, Name = "architecture", DisplayName = "Architektura & Wzorce", Description = "SOLID, Design Patterns, Clean Architecture, DDD, CQRS", TechnologyTypeId = 1, AllowsHint = false },
            new Category { Id = 3, Name = "databases", DisplayName = "Bazy danych", Description = "SQL, ORM, optymalizacja, transakcje, indeksy", TechnologyTypeId = 1, AllowsHint = true },
            new Category { Id = 4, Name = "api", DisplayName = "API & Komunikacja", Description = "REST, HTTP, WebSockets, gRPC, serializacja JSON/XML", TechnologyTypeId = 1, AllowsHint = false },
            new Category { Id = 5, Name = "testing", DisplayName = "Jakość & Testy", Description = "Unit tests, integration tests, TDD, mocking, refaktoryzacja", TechnologyTypeId = 1, AllowsHint = false },
            new Category { Id = 6, Name = "security", DisplayName = "Bezpieczeństwo", Description = "OWASP Top 10, autentykacja, autoryzacja, JWT, szyfrowanie", TechnologyTypeId = 1, AllowsHint = false },
            new Category { Id = 7, Name = "devops", DisplayName = "DevOps & Narzędzia", Description = "CI/CD, Docker, Kubernetes, Git, monitoring, logging", TechnologyTypeId = 1, AllowsHint = true }
        );

        // Frontend Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 8, Name = "framework_fundamentals", DisplayName = "Fundamenty frameworka", Description = "Komponenty, lifecycle, reactivity, props, events, slots", TechnologyTypeId = 2, AllowsHint = false },
            new Category { Id = 9, Name = "state_management", DisplayName = "State Management", Description = "Zarządzanie stanem (Pinia, Vuex, Redux, Zustand)", TechnologyTypeId = 2, AllowsHint = true },
            new Category { Id = 10, Name = "routing", DisplayName = "Routing & Navigation", Description = "Router, guards, lazy loading, dynamic routes", TechnologyTypeId = 2, AllowsHint = false },
            new Category { Id = 11, Name = "frontend_testing", DisplayName = "Testy frontendowe", Description = "Unit tests, component tests, E2E (Vitest, Jest, Cypress)", TechnologyTypeId = 2, AllowsHint = true },
            new Category { Id = 12, Name = "performance", DisplayName = "Performance & Optymalizacja", Description = "Bundle size, code splitting, memoization, virtual DOM", TechnologyTypeId = 2, AllowsHint = false },
            new Category { Id = 13, Name = "typescript_frontend", DisplayName = "TypeScript & Typowanie", Description = "Typy w kontekście frameworka, generics, utility types", TechnologyTypeId = 2, AllowsHint = false }
        );
    }
}
