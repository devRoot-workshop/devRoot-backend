using System;
using devRoot.Server;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<devRootContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("DEVROOTCONNECTIONSTRING", EnvironmentVariableTarget.Machine)));
builder.Services.AddScoped<Utilites>();

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
    Credential = GoogleCredential.FromFile("./devRoot.json")
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(myAllowSpecificOrigins);
app.UseHttpsRedirection();



app.UseAuthorization();
app.UseMiddleware<FirebaseAuthMiddleware>();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllers();

app.Run();
