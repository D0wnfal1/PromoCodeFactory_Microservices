using Microsoft.EntityFrameworkCore;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.DataAccess;
using Pcf.Administration.DataAccess.Data;
using Pcf.Administration.DataAccess.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IDbInitializer, EfDbInitializer>();
builder.Services.AddDbContext<DataContext>(x =>
{
	//x.UseSqlite("Filename=PromocodeFactoryAdministrationDb.sqlite");
	x.UseNpgsql(builder.Configuration.GetConnectionString("PromocodeFactoryAdministrationDb"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(options =>
{
	options.Title = "PromoCode Factory Administration API Doc";
	options.Version = "1.0";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
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

app.Run();
