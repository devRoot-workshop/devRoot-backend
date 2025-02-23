using System;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using devRoot.Server;
using devRoot.Server.Auth;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal class Program
{
    public static void Main(string[] args)
    {
        const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<Utilities>();

        //Connection string from system environment | Used in production

        var connstring = Environment.GetEnvironmentVariable("DEVROOTCONNECTIONSTRING");

        if (string.IsNullOrEmpty(connstring))
        {
            connstring = Utilities.EnvRead("./.env")["DEVROOTCONNECTIONSTRING"];
        }

        builder.Services.AddDbContextPool<devRootContext>(options =>
            options.UseNpgsql(connstring));
        /*
            ---------- | DEVELOPMENT ONLY | ----------
        */
        // Connection string for local postgresql container | Docker compose file provided
        //builder.Services.AddDbContextPool<devRootContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=efdb;Username=efuser;Password=efpassword"));

        // CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: myAllowSpecificOrigins,
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        builder.Services.AddSwaggerGen(config =>
        {

            config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "devRoot ASP.NET Core REST API",
                Description = "This is the Swagger documentation for the backend endpoints of the devRoot app",
                Version = "v1"
            });

            config.UseInlineDefinitionsForEnums();

            // Add the security definition for Bearer token
            config.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter the Bearer token in the format 'Bearer {token}'"
            });

            // Add a global security requirement so it applies to all endpoints
            config.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
        });


        var firebasestring = Environment.GetEnvironmentVariable("DEVROOTFIREBASESTRING");

        if (string.IsNullOrEmpty(connstring))
        {
            firebasestring = Utilities.EnvRead("./.env")["DEVROOTFIREBASESTRING"];
        }

        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromJson(firebasestring)
        });
        builder.Services.AddSingleton<FirebaseService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(myAllowSpecificOrigins);
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.MapControllers();

        app.Run();
    }
}
