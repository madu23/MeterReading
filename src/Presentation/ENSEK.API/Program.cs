using ENSEK.API.Extensions;
using ENSEK.Application.Interfaces;
using ENSEK.Application.Mappers;
using ENSEK.Application.Models;
using ENSEK.Database.Context;
using ENSEK.Domain.Interfaces;
using ENSEK.Service.Implementation;
using ENSEK.Service.Implementation.Repositories;
using ENSEK.Service.Implementation.Services;
using ENSEK.Tasks.StartupTasks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("DbConn");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<IUploadService, UploadService>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IMeterReadingRepository, MeterReadingRepository>();
builder.Services.AddTransient<IStartupTask, SeedDbTask>();
builder.Services.AddDbContext<EnsekContext>(opt => opt.UseSqlServer(connString));
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("VisitorManagerPolicy",
    policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});
// configure automapper section
var AutoMapperConfig = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});
var mapper = AutoMapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddLogging(l => l.AddConsole());
var app = builder.Build();
await app.RunWithTasksAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
