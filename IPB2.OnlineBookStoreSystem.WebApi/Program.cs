using IPB2.OnlineBookStoreSystem.Domain.Interfaces;
using IPB2.OnlineBookStoreSystem.Domain.Services;
using IPB2.OnlineBookStoreSystem.MinimalApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookStoreRepository, SqlBookStoreRepository>();
builder.Services.AddScoped<IBookStoreService, BookStoreDomainService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
