using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration.Json;

namespace Payments
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var resourceList = new List<KeyValuePair<string, object>>();
            resourceList.Add(new KeyValuePair<string, object>
                ("application", "tacocat"));
            var appConfig = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .Build();
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls($"http://*:50063");
            builder.Configuration.AddJsonFile("appsettings.json");

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
                    options.ExportProcessorType = ExportProcessorType.Simple;
                });
            });
            builder.Build().Run();
        }


    }
}
