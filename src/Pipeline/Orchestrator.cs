using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Pipeline.models;

namespace Pipeline
{
    public static class Orchestrator
    {
        [FunctionName("Orchestrator")]
        public static async Task Run([OrchestrationTrigger]DurableOrchestrationContext context, TraceWriter log)
        {
            log.Info($"Orchestrator triggered at: {DateTime.UtcNow}");

            // items after initial transformation
            List<TransformedObject> items = context.GetInput<List<TransformedObject>>();            

            // call cleaner
            foreach(TransformedObject item in items)
            {
                await context.CallActivityAsync("CleanerActivity", item);
            }
            log.Info("Completed cleaner activities");

            // call enricher
            foreach(TransformedObject item in items)
            {
                await context.CallActivityAsync("EnricherActivity", item);
            }
            log.Info("Completed enricher activities");

            // call egress
            await context.CallActivityAsync("EgressActivity", items);
        }
    }
}
