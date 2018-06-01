using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Pipeline.models;

namespace Pipeline
{
    public static class EgressActivity
    {
        [FunctionName("EgressActivity")]
        public static async Task Run([ActivityTrigger]DurableActivityContext context, TraceWriter log)
        {
            log.Info($"EgressActivity triggered at: {DateTime.UtcNow}");

            List<TransformedObject> items = context.GetInput<List<TransformedObject>>();

            await Task.Run(() => {});
        }
    }
}