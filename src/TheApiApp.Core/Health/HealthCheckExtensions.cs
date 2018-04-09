﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Newtonsoft.Json;
using P7.HealthCheck.Core;

namespace TheApiApp.Core.Health
{
    public  class HealthCheckOptions
    {
        [JsonProperty("groups")]
        public HealthCheckGroup[] Groups { get; set; }
    }

    public  class HealthCheckGroup
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("urls")]
        public string[] Urls { get; set; }
    }

    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddMyHealthCheck(this IServiceCollection services, IConfiguration config)
        {
            var appConfig = new HealthCheckOptions();
            config.GetSection("healthCheck").Bind(appConfig);

            services.AddSingleton<CustomHealthCheck>();
            services.AddHealthChecks(checks =>
            {
                foreach (var group in appConfig.Groups)
                {
                    if(String.Equals(group.Type, "url",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        checks.AddHealthCheckGroup(group.Name, g =>
                        {
                            foreach (var url in group.Urls)
                            {
                                g.AddUrlCheck(url);
                            }
                        });
                    }
                }
                checks.AddCheck<AggregateHealthCheck>("aggregate-health");
                /*
                checks.AddUrlCheck("https://github.com")
                    .AddHealthCheckGroup(
                        "servers",
                        group => group.AddUrlCheck("https://google.com")
                            .AddUrlCheck("https://twitddter.com")
                    )
                    .AddHealthCheckGroup(
                        "memory",
                        group => group.AddPrivateMemorySizeCheck(1)
                            .AddVirtualMemorySizeCheck(2)
                            .AddWorkingSetCheck(1),
                        CheckStatus.Unhealthy
                    )
                    .AddCheck("thrower", (Func<IHealthCheckResult>)(() => { throw new DivideByZeroException(); }))
                    .AddCheck("long-running", async cancellationToken => { await Task.Delay(10000, cancellationToken); return HealthCheckResult.Healthy("I ran too long"); })
                    .AddCheck<CustomHealthCheck>("custom");
                    */
                /*
                // add valid storage account credentials first
                checks.AddAzureBlobStorageCheck("accountName", "accountKey");
                checks.AddAzureBlobStorageCheck("accountName", "accountKey", "containerName");
                checks.AddAzureTableStorageCheck("accountName", "accountKey");
                checks.AddAzureTableStorageCheck("accountName", "accountKey", "tableName");
                checks.AddAzureFileStorageCheck("accountName", "accountKey");
                checks.AddAzureFileStorageCheck("accountName", "accountKey", "shareName");
                checks.AddAzureQueueStorageCheck("accountName", "accountKey");
                checks.AddAzureQueueStorageCheck("accountName", "accountKey", "queueName");
                */

            });
            return services;
        }
    }
}