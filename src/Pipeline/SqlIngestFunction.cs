using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Pipeline.models;

namespace Pipeline
{
    public static class SqlIngestFunction
    {
         [FunctionName("SqlIngestFunction")]
        public static async Task Run(
            [TimerTrigger("0 0 18 * * *")]TimerInfo timer,
            [OrchestrationClient] DurableOrchestrationClient starter, TraceWriter log)
        {
            log.Info($"SqlIngestFunction triggered at: {DateTime.UtcNow}");
            // Read from SQL
            List<IncomingObjectTwo> items = new List<IncomingObjectTwo>
            {
                new IncomingObjectTwo { Name = "Hello"},
                new IncomingObjectTwo { Name = "Hello1"},
                new IncomingObjectTwo { Name = "Hello2"}
            };

            // Transform
            List<TransformedObject> transformed = new List<TransformedObject>();
            await starter.StartNewAsync("Orchestrator", transformed);
        }
    }
}