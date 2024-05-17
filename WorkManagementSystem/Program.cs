using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSwag;
using System.Reflection;
using WorkManagementSystem;
using WorkManagementSystem.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
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


app.Run();

