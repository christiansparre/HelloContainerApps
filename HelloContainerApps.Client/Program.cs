using HelloContainerApps.Client.Data;
using HelloContainerApps.Client.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services
    .AddSingleton<ClusterClientHostedService>()
    .AddSingleton<IHostedService>(_ => _.GetRequiredService<ClusterClientHostedService>())
    .AddSingleton<IClusterClient>(_ => _.GetRequiredService<ClusterClientHostedService>().Client);

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
