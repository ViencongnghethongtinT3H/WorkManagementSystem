using Microsoft.EntityFrameworkCore;
using Serilog;
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

}

