using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;


var resourceList = new List<KeyValuePair<string, object>>();
            resourceList.Add(new KeyValuePair<string, object>
                ("application", "tacocat"));


var appConfig = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
             //         .AddYamlFile("app.yaml", optional: false)
                      .Build();

var builder = WebApplication.CreateBuilder( args);
            builder.Logging.AddFilter("System", LogLevel.Trace);
            builder.Logging.AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Trace);
            builder.Logging.AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Trace);
            builder.Logging.SetMinimumLevel(LogLevel.Trace);
            builder.Services.AddControllers();

/*
            // Build a config object, using env vars and JSON providers.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddYamlFile("payments-app.yml")
                .AddEnvironmentVariables()
                .Build();

            // Load file listed in payments-app.yml
            config.AddYamlFile($"{config["applicationTagsYamlFile"]});
            resourceList.Add(new KeyValuePair<string, object>
                ("application", $"{config["application"]}"));
            resourceList.Add(new KeyValuePair<string, object>
                ("cluster", $"{config["cluster"]}"));
             resourceList.Add(new KeyValuePair<string, object>
                 ("service", $"{config["payments"]}"));
              resourceList.Add(new KeyValuePair<string, object>
                  ("shard", $"{config["shard"]}"));
 */
             builder.Configuration.AddJsonFile("appsettings.json");
             builder.WebHost.UseUrls("http://*:50063");
  //          builder.WebHost.UseUrls($"http://*:{appConfig["port"]}");
            builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder.AddAspNetCoreInstrumentation();
                tracerProviderBuilder.AddHttpClientInstrumentation();
                tracerProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService("payments").AddAttributes(resourceList));
                tracerProviderBuilder.AddSource("BackEnd");
                tracerProviderBuilder.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri("http://tacocat-wavefront-proxy:4317");
   //                 options.Endpoint = new Uri($"{config["proxy"]}");
                    options.ExportProcessorType = ExportProcessorType.Simple;
                });
            });
            using var app = builder.Build();
     //        app.UseStaticFiles();
     //        app.UseRouting();
               app.MapControllers();

            app.Run("http://*:50063");
