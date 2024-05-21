using FastEndpoints.Swagger;
using System.Reflection;
using WorkManagementSystem;
using WorkManagementSystem.Infrastructure.DataSeeder;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAuthorization();
//builder.Services.AddAuthentication();
builder.Services.AddFastEndpoints().SwaggerDocument();
//(o =>
//{
//    o.DocumentSettings = s =>
//    {
//        s.Title = "WMS API";
//        s.Description = "SyNV";
//        s.Version = "1";
//        s.AddAuth("Bearer", new()
//        {
//            Type = OpenApiSecuritySchemeType.Http,
//            Scheme = JwtBearerDefaults.AuthenticationScheme,
//            BearerFormat = "JWT",
//        });
//    };
//});

builder.Services.AddDbContext<MainDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(MainDbContext).GetTypeInfo().Assembly.GetName().Name);
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<DataSeeder>();

//builder.Services.SwaggerDocument(o =>
//{
//    o.DocumentSettings = s =>
//    {
//        s.Title = "WMS API";
//       // s.Description = "SyNV";
//        //s.Version = "v1";
//        s.AddAuth("Bearer", new()
//        {
//            Type = OpenApiSecuritySchemeType.Http,
//            Scheme = JwtBearerDefaults.AuthenticationScheme,
//            BearerFormat = "JWT",
//        });
//    };

//});

builder.Services.AddInjections();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseFastEndpoints().UseSwaggerGen();
app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();
try
{
    app.Logger.LogInformation("Applying database migration ({ApplicationName})...", "WMS");
    app.ApplyDatabaseMigration();
    // HttpContextCustom.SetupHttpContext(app);
    app.Logger.LogInformation("Applying seed data into database");
    await SeedData(app);
    app.Logger.LogInformation("Starting web host (WMS)...");
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Host terminated unexpectedly (WMS)...");
}

async Task SeedData(IHost app)
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

app.Run();

