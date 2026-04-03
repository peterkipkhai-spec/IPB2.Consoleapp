using IPB2.OnlineBookStoreSystem.Domain.Interfaces;
using IPB2.OnlineBookStoreSystem.Domain.Services;
using IPB2.OnlineBookStoreSystem.MinimalApi.Data;
using IPB2.OnlineBookStoreSystem.MinimalApi.Features.Authors;
using IPB2.OnlineBookStoreSystem.MinimalApi.Features.Books;
using IPB2.OnlineBookStoreSystem.MinimalApi.Features.Categories;
using IPB2.OnlineBookStoreSystem.MinimalApi.Features.Customers;
using IPB2.OnlineBookStoreSystem.MinimalApi.Features.Orders;
using IPB2.OnlineBookStoreSystem.MinimalApi.Features.Payments;
using IPB2.OnlineBookStoreSystem.MinimalApi.Features.Reports;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookStoreRepository, SqlBookStoreRepository>();
builder.Services.AddScoped<IBookStoreService, BookStoreDomainService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Ok(new { name = "OnlineBookStore Minimal API", version = "v1" }));
app.MapAuthorEndpoints();
app.MapCategoryEndpoints();
app.MapCustomerEndpoints();
app.MapBookEndpoints();
app.MapOrderEndpoints();
app.MapPaymentEndpoints();
app.MapReportEndpoints();

app.Run();
