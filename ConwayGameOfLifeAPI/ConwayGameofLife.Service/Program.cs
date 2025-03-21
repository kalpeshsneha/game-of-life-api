using ConwayGameOfLife.Core.Services;
using ConwayGameOfLife.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using ConwayGameOfLife.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add API Versioning 
builder.Services.AddApiVersioning(options =>
{
	options.ReportApiVersions = true;
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddDbContext<GameOfLifeDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("GameOfLifeDB")));

// Core Service Registration
builder.Services.AddScoped<IBoardService, BoardService>();

// Infra Repository Registration 
builder.Services.AddScoped<IBoardRepository, BoardRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/exceptions/logs");
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
