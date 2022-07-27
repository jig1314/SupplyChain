using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using InventoryMgmt.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Initializing CAP library to handle subscribing to message bus 
builder.Services.AddCap(x =>
{
    x.UseEntityFramework<ApplicationDbContext>();

    x.UseAzureServiceBus(opt =>
    {
        // Connecting to Azure Service Bus to handle message transmission
        opt.ConnectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnection");
        opt.EnableSessions = true;

        opt.CustomHeaders = message => new List<KeyValuePair<string, string>>()
        {
            new(DotNetCore.CAP.Messages.Headers.MessageId, SnowflakeId.Default().NextId().ToString()),
            new(DotNetCore.CAP.Messages.Headers.MessageName, message.Label)
        };
    });

    x.UseDashboard();
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCapDashboard();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
