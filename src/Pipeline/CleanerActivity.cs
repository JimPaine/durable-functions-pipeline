using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Pipeline.models;

namespace Pipeline
{
    public static class CleanerActivity
    {
        [FunctionName("CleanerActivity")]
        public static TransformedObject Run([ActivityTrigger]DurableActivityContext context, TraceWriter log)
        {
            log.Info($"CleanerActivity triggered at: {DateTime.UtcNow}");

            TransformedObject item = context.GetInput<TransformedObject>();

            // clean something

            return item;
        }
    }
}