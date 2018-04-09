using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using P7.Core.Scheduler.Scheduling;
using P7.HealthCheck.Core;

namespace TheApiApp.Core.Scheduler
{
    public class DummyHealthScheduledTask : IScheduledTask
    {
        private IHealthCheckStore HealthCheckStore { get; set; }
        public DummyHealthScheduledTask(
            IHealthCheckStore healthCheckStore)
        {
            HealthCheckStore = healthCheckStore;
        }
        public string Schedule => "*/1 * * * *";  // every 1 minute

        public async Task Invoke(CancellationToken cancellationToken)
        {
            var key = "dummy-health-task";
            var currentHealth = await HealthCheckStore.GetHealthAsync(key);
            if (currentHealth == null)
            {
                currentHealth = new HealthRecord()
                {
                    Healty = false,
                    State = null
                };
                await HealthCheckStore.SetHealthAsync(key, currentHealth);
            }

            // once loaded we are good
            currentHealth = new HealthRecord()
            {
                Healty = true,
                State = null
            };
            await HealthCheckStore.SetHealthAsync(key, currentHealth);

        }
    }
}
