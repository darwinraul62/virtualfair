using Fair.Company.Api.HostedServices;
using Fair.Company.Api.Services;
using Fair.Company.Data;
using Fair.Company.Data.Repositories;
using Fair.Company.Infrastructure;
using Fair.Company.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the Entity Framework services to the container.
builder.Services.AddPersistenceService(builder.Configuration);
//Add Mapping Profiles
builder.Services.AddAutoMapper(typeof(Program));

//Initialize Data
builder.Services.AddTransient<InitializationService>();
builder.Services.AddHostedService<InitializeDataWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDataBaseUpdate();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();