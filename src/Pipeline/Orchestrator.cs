using System;
using System.Linq;
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
            List<Task<TransformedObject>> tasks = new List<Task<TransformedObject>>();
            foreach(TransformedObject item in items)
            {
                tasks.Add(context.CallActivityAsync<TransformedObject>("CleanerActivity", item));
            }
            await Task.WhenAll(tasks);
            log.Info("Completed cleaner activities");

            // call enricher
            items = tasks.Select(x => x.Result).ToList();
            tasks = new List<Task<TransformedObject>>();
            foreach(TransformedObject item in items)
            {
                tasks.Add(context.CallActivityAsync<TransformedObject>("EnricherActivity", item));
            }
            await Task.WhenAll(tasks);
            log.Info("Completed enricher activities");

            // call egress
            await context.CallActivityAsync("EgressActivity", tasks.Select(x => x.Result).ToList());
        }
    }
}
