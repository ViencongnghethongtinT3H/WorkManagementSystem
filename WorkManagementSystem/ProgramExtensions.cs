//using Microsoft.EntityFrameworkCore;
//using Serilog;

//namespace WorkManagementSystem;
//public static class ProgramExtensions
//{
//    private const string CorsName = "api";
//    private const string appName = "Configuration";

//    public static IServiceCollection AddCoreServices(this IServiceCollection services,
//            IConfiguration config, IWebHostEnvironment env, Type apiType)
//    {
//        services.AddCors(options =>
//        //{
//            options.AddPolicy(CorsName, policy =>
//            {
//                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
//            });
//        });
       
       
//        Log.Error("------ End creation AddCoreServices------------");
//        return services;
//    }

    
//    public static async Task<IApplicationBuilder> UseCoreApplication(this WebApplication app, IWebHostEnvironment env)
//    {
//        if (env.IsDevelopment())
//        {
//            app.UseDeveloperExceptionPage();
//        }

//        app.UseSwagger();
//        app.UseCors(CorsName);
//        app.UseRouting();
//        app.MapGet("/", () => Results.LocalRedirect("~/swagger"));
//        app.UseAuthentication();
//        app.UseAuthorization();
//        SetupHttpContext(app);
//        app.UseMiddleware(typeof(ErrorHandleMiddleware));
//        app.UseMiddleware(typeof(AntiXssMiddleware));
//        app.UseEndpoints(endpoints =>
//        {
//            endpoints.MapControllers();
//        });


//        try
//        {
//            app.Logger.LogInformation("Applying database migration ({ApplicationName})...", appName);
//            //  app.ApplyDatabaseMigration();
//            app.Logger.LogInformation("Applying seed data into database");
//            await SeedData<IdentityDataSeeder>(app);
//            await SeedData<DataSeeder>(app);

//            app.Logger.LogInformation("Done apply seed data into database ({ApplicationName})...", appName);
//        }
//        catch (Exception ex)
//        {
//            app.Logger.LogCritical(ex, "Host terminated unexpectedly ({ApplicationName})...", appName);
//        }
//        finally
//        {
//            Log.CloseAndFlush();
//        }

//        return app;
//    }

//    //public static void ApplyDatabaseMigration(this WebApplication app)
//    //{
//    //    try
//    //    {
//    //        using var scope = app.Services.CreateScope();
//    //        var context = scope.ServiceProvider.GetRequiredService<ConfigurationCmsDataContext>();
//    //        context.Database.Migrate();
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        Log.Error("ApplyDatabaseMigration fails");
//    //        throw;
//    //    }

//    //}

//    //public static async Task SeedData<T>(IHost app) where T : IDataSeeder
//    //{
//    //    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
//    //    if (scopedFactory is not null)
//    //    {
//    //        using var scope = scopedFactory.CreateScope();
//    //        var service = scope.ServiceProvider.GetService<T>();
//    //        if (service is not null)
//    //        {
//    //            await service.SeedAllAsync();
//    //        }
//    //        else
//    //        {
//    //            Log.Error("Can't seed data because services of DataSeeder not create");
//    //        }
//    //    }
//    //}

//    //public static void SetupHttpContext(IHost app)
//    //{
//    //    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
//    //    if (scopedFactory is not null)
//    //    {
//    //        using var scope = scopedFactory.CreateScope();
//    //        var context = scope.ServiceProvider.GetService<IHttpContextAccessor>();
//    //        if (context is not null)
//    //        {
//    //            DIS.Core.HttpUtils.HttpContext.Configure(context);
//    //        }
//    //        else
//    //        {
//    //            Log.Error("SetupHttpContext -- context is null");
//    //        }
//    //    }
//    //}


//    public static IServiceCollection AddSqlServerDbContext<TDbContext>(this IServiceCollection services, IConfiguration configuration)
//      where TDbContext : DbContext
//    {
//        services.AddDbContext<TDbContext>(options =>
//        {
//            options.UseSqlServer(configuration.GetConnectionString("SqlServerConnectionString"),
//                sqlServerOptionsAction: sqlOptions =>
//                {
//                    sqlOptions.MigrationsAssembly(typeof(TDbContext).GetTypeInfo().Assembly.GetName().Name);
//                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
//                });
//        }, ServiceLifetime.Scoped);
//        return services;
//    }

//}

