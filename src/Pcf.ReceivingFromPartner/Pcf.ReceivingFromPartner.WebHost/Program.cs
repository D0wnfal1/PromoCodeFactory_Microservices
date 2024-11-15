using MassTransit;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Abstractions.Repositories;
using Pcf.ReceivingFromPartner.Core.Services;
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
builder.Services.AddScoped<PromocodeService>();

// HTTP clients for integration
builder.Services.AddHttpClient<IGivingPromoCodeToCustomerGateway, GivingPromoCodeToCustomerGateway>(c =>
{
	c.BaseAddress = new Uri(builder.Configuration["IntegrationSettings:GivingToCustomerApiUrl"]);
});
builder.Services.AddHttpClient<IAdministrationGateway, AdministrationGateway>(c =>
{
	c.BaseAddress = new Uri(builder.Configuration["IntegrationSettings:AdministrationApiUrl"]);
});

// MongoDB settings
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));

// MassTransit for RabbitMQ
builder.Services.AddMassTransit(x =>
{
	x.UsingRabbitMq((context, cfg) =>
	{
		var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ");
		cfg.Host(new Uri($"amqp://{rabbitMqConfig["Username"]}:{rabbitMqConfig["Password"]}@{rabbitMqConfig["Host"]}:{rabbitMqConfig["Port"]}/{rabbitMqConfig["VirtualHost"]}"));
		cfg.ClearSerialization();
		cfg.UseRawJsonSerializer();
		cfg.ConfigureEndpoints(context);
	});
});

// OpenAPI/Swagger for API documentation
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

// Initialize the database
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
