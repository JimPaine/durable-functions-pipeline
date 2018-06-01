using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Pipeline.models;

namespace Pipeline
{
    public static class EnricherActivity
    {
        [FunctionName("EnricherActivity")]
        public static TransformedObject Run([ActivityTrigger]DurableActivityContext context, TraceWriter log)
        {
            log.Info($"EnricherActivity triggered at: {DateTime.UtcNow}");

            TransformedObject item = context.GetInput<TransformedObject>();

            // enrich something

            return item;
        }
    }
}