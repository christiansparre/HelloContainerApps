using HelloContainerApps.Client.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleansClient(clientBuilder =>
{
    clientBuilder.UseAzureStorageClustering(options =>
        {
            options.ConfigureTableServiceClient(builder.Configuration["AzureStorageConnectionString"]);
            options.TableName = builder.Configuration["AzureStorageTableName"];
        })
        .Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "HelloWorld";
            options.ServiceId = "HelloWorld";
        });
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddLogging(options => options.AddApplicationInsights());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
