using Microsoft.EntityFrameworkCore;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.DataAccess;
using Pcf.GivingToCustomer.DataAccess.Data;
using Pcf.GivingToCustomer.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IDbInitializer, EfDbInitializer>();
builder.Services.AddDbContext<DataContext>(x =>
{
	//x.UseSqlite("Filename=PromocodeFactoryAdministrationDb.sqlite");
	x.UseNpgsql(builder.Configuration.GetConnectionString("promocode_factory_givingToCustomer_db"));
});

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
