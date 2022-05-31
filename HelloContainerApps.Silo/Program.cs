using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseOrleans(silo =>
    {
        silo
            .UseAzureStorageClustering(options =>
            {
                options.ConfigureTableServiceClient(builder.Configuration["AzureStorageConnectionString"]);
                options.TableName = builder.Configuration["AzureStorageTableName"];
            })
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "HelloWorld";
                options.ServiceId = "HelloWorld";
            })
            .ConfigureEndpoints(builder.Configuration["SiloHostName"] ?? Dns.GetHostName(), builder.Configuration.GetValue<int>("SiloPort"), builder.Configuration.GetValue<int>("GatewayPort"))
            .ConfigureLogging(loggingBuilder => { loggingBuilder.AddApplicationInsights(); });
    });

var app = builder.Build();

app.MapGet("/", context => context.Response.WriteAsync("Hello World"));

app.Run();