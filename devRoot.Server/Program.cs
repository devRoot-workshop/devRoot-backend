using System;
using devRoot.Server;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    public static void Main(string[] args)
    {
        const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<Utilites>();

        // // Connection string from system environment | Used in production
        builder.Services.AddDbContextPool<devRootContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("DEVROOTCONNECTIONSTRING", EnvironmentVariableTarget.Machine)));

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
                    policy.WithOrigins("http://localhost:3000",
                            "http://127.0.0.1:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("DEVROOTFIREBASESTRING", EnvironmentVariableTarget.Machine))
        });
        builder.Services.AddSingleton<FirebaseService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(myAllowSpecificOrigins);
        }

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
