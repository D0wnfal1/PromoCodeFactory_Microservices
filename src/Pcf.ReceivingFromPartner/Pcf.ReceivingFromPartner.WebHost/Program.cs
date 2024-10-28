using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Abstractions.Repositories;
using Pcf.ReceivingFromPartner.DataAccess;
using Pcf.ReceivingFromPartner.DataAccess.Data;
using Pcf.ReceivingFromPartner.DataAccess.Repositories;
using Pcf.ReceivingFromPartner.Integration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<INotificationGateway, NotificationGateway>();
builder.Services.AddScoped<IDbInitializer, EfDbInitializer>();

builder.Services.AddHttpClient<IGivingPromoCodeToCustomerGateway, GivingPromoCodeToCustomerGateway>(c =>
{
	c.BaseAddress = new Uri(builder.Configuration["IntegrationSettings:GivingToCustomerApiUrl"]);
});

builder.Services.AddHttpClient<IAdministrationGateway, AdministrationGateway>(c =>
{
	c.BaseAddress = new Uri(builder.Configuration["IntegrationSettings:AdministrationApiUrl"]);
});

builder.Services.AddDbContext<DataContext>(x =>
{
	//x.UseSqlite("Filename=PromocodeFactoryReceivingFromPartnerDb.sqlite");
	x.UseNpgsql(builder.Configuration.GetConnectionString("PromocodeFactoryReceivingFromPartnerDb"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(options =>
{
	options.Title = "PromoCode Factory Receiving From Partner API Doc";
	options.Version = "1.0";
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

app.Run();
