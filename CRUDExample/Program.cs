using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.ConfigureLogging(loggingProvider =>
//{
//	loggingProvider.ClearProviders();	
//	loggingProvider.AddConsole();
//	loggingProvider.AddDebug();
//	loggingProvider.AddEventLog();
//});

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
	loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
});

builder.Services.AddControllersWithViews();

//add services into IoC container
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

//thông báo add db sử dụng với sqlserver
builder.Services.AddDbContext<PersonsDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefautConnection"));
});
//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False
var app = builder.Build();

//app.Logger.LogDebug("LogDebug");
//app.Logger.LogInformation("LogInformation");
//app.Logger.LogWarning("LogWarning");
//app.Logger.LogError("LogError");
//app.Logger.LogCritical("LogCritical");

if (builder.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
