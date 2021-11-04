using System.Net;
using HelloContainerApps.Silo.Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .UseOrleans((ctx, siloBuilder) =>
        siloBuilder
        .UseAzureStorageClustering(options =>
        {
            options.ConnectionString = ctx.Configuration["AzureStorageConnectionString"];
            options.TableName = ctx.Configuration["AzureStorageTableName"];
        })
        .Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "HelloWorld";
            options.ServiceId = "HelloWorld";
        })
        .ConfigureEndpoints(Dns.GetHostName(), ctx.Configuration.GetValue<int>("SiloPort"), ctx.Configuration.GetValue<int>("GatewayPort"))
        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloWorldGrain).Assembly).WithReferences()
        ))
    .ConfigureServices(services =>
    {

    })
    .Build();

await host.RunAsync();