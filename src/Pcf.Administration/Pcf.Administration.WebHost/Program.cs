using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Services;
using Pcf.Administration.DataAccess;
using Pcf.Administration.DataAccess.Data;
using Pcf.Administration.DataAccess.Repositories;
using Pcf.Administration.WebHost.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IDbInitializer, EfDbInitializer>();
builder.Services.AddScoped<AdministrationPromocodeService>();

// PostgreSQL settings
builder.Services.AddDbContext<DataContext>(x =>
{
	x.UseNpgsql(builder.Configuration.GetConnectionString("PromocodeFactoryAdministrationDb"));
	x.UseSnakeCaseNamingConvention();
});

// Redis cache setup
builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration.GetConnectionString("Redis");
	options.InstanceName = "PromoCodeFactoryAdministration_";
});

// MassTransit for RabbitMQ
builder.Services.AddMassTransit(x =>
{
	x.AddConsumer<AdministationPromocodesConsumer>();
	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host("amqp://guest:guest@localhost:5672");
		cfg.ReceiveEndpoint("PromocodesConsumerQueue", e =>
		{
			e.ConfigureConsumer<AdministationPromocodesConsumer>(context);
		});
		cfg.ClearSerialization();
		cfg.UseRawJsonSerializer();
		cfg.ConfigureEndpoints(context);
	});
});

// OpenAPI/Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(options =>
{
	options.Title = "PromoCode Factory Administration API Doc";
	options.Version = "1.0";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOpenApi();
app.UseSwaggerUI();
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
