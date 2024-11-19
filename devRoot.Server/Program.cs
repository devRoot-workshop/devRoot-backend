using System;
using devRoot.Server;
using devRoot.Server.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Utilites>();

// // Connection string from system environment | Used in production
// builder.Services.AddDbContextPool<devRootContext>(options =>
//     options.UseNpgsql(Environment.GetEnvironmentVariable("DEVROOTCONNECTIONSTRING", EnvironmentVariableTarget.Machine)));

/*
    ---------- | DEVELOPMENT ONLY | ----------
*/
// Connection string for local postgresql container | Docker compose file provided
builder.Services.AddDbContextPool<devRootContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=efdb;Username=efuser;Password=efpassword"));
    
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
/*
    ---------- | DEVELOPMENT ONLY | ----------
*/

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(myAllowSpecificOrigins);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
