using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pcf.GivingToCustomer.Core.Abstractions.Gateways;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Services;
using Pcf.GivingToCustomer.DataAccess;
using Pcf.GivingToCustomer.DataAccess.Data;
using Pcf.GivingToCustomer.DataAccess.Repositories;
using Pcf.GivingToCustomer.Integration;
using Pcf.GivingToCustomer.WebHost.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<INotificationGateWay, NotificationGateway>();
builder.Services.AddScoped<IDbInitializer, EfDbInitializer>();
builder.Services.AddScoped<GivingPromocodesService>();
builder.Services.AddDbContext<DataContext>(x =>
{
	//x.UseSqlite("Filename=PromocodeFactoryAdministrationDb.sqlite");
	x.UseNpgsql(builder.Configuration.GetConnectionString("PromocodeFactoryGivingToCustomerDb"));
	x.UseSnakeCaseNamingConvention();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(options =>
{
	options.Title = "PromoCode Factory Giving To Customer API Doc";
	options.Version = "1.0";
});
builder.Services.AddMassTransit(x =>
{
	x.AddConsumer<PromocodesConsumer>();

	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host("rabbitmq://localhost", c =>
		{
			c.Username("guest");
			c.Password("guest");
		});

		cfg.ReceiveEndpoint("PromocodesConsumerQueue", e =>
		{
			e.ConfigureConsumer<PromocodesConsumer>(context);
		});

		cfg.ClearSerialization();
		cfg.UseRawJsonSerializer();
		cfg.ConfigureEndpoints(context);
	});
});
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}
app.UseOpenApi();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//	var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
//	dbInitializer.InitializeDb();
//}
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
