using Microsoft.EntityFrameworkCore;
using Serilog;
using WorkManagementSystem.Infrastructure.DataSeeder;
using WorkManagementSystem.Infrastructure.Factory;
using WorkManagementSystem.Infrastructure.Persistence;
using WorkManagementSystem.Infrastructure.Repository;
using WorkManagementSystem.Infrastructure.UnitOfWork;

namespace WorkManagementSystem;
public static class ProgramExtensions
{
    internal static void AddInjections(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseContext, MainDbContext>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        //services.AddTransient(typeof(IMovieServices), typeof(MovieServices));
        services.AddTransient<IContextFactory, ContextFactory>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }

    public static async Task SeedData(IHost app)
    {
        var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
        if (scopedFactory is not null)
        {
            using var scope = scopedFactory.CreateScope();
            var service = scope.ServiceProvider.GetService<DataSeeder>();
            if (service is not null)
            {
                await service.SeedAllAsync();
            }
        }
    }
    public static void ApplyDatabaseMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
        context.Database.Migrate();
    }
}

