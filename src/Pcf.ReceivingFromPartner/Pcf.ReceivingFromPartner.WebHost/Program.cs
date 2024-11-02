using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Abstractions.Repositories;
using Pcf.ReceivingFromPartner.DataAccess;
using Pcf.ReceivingFromPartner.DataAccess.Data;
using Pcf.ReceivingFromPartner.Integration;
using Pcf.ReceivingFromPartner.WebHost.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
builder.Services.AddScoped<INotificationGateway, NotificationGateway>();
builder.Services.AddScoped<IDbInitializer, MongoDbInitializer>();


builder.Services.AddHttpClient<IGivingPromoCodeToCustomerGateway, GivingPromoCodeToCustomerGateway>(c =>
{
	c.BaseAddress = new Uri(builder.Configuration["IntegrationSettings:GivingToCustomerApiUrl"]);
});

builder.Services.AddHttpClient<IAdministrationGateway, AdministrationGateway>(c =>
{
	c.BaseAddress = new Uri(builder.Configuration["IntegrationSettings:AdministrationApiUrl"]);
});

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
void SeedDatabase()
{
	using (var scope = app.Services.CreateScope())
	{
		var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
		dbInitializer.InitializeDb();
	}
}

SeedDatabase();



app.Run();
