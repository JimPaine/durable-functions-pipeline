using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Pipeline.models;

namespace Pipeline
{
    public static class HttpIngestFunction
    {
         [FunctionName("HttpIngestFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
            HttpRequestMessage request, [OrchestrationClient] DurableOrchestrationClient starter, TraceWriter log)
        {
            log.Info($"HttpIngestFunction triggered at: {DateTime.UtcNow}");
            // pull from body
            //tranform
            List<IncomingObjectOne> items = new List<IncomingObjectOne>
            {
                new IncomingObjectOne { Name = "Hello"},
                new IncomingObjectOne { Name = "Hello1"},
                new IncomingObjectOne { Name = "Hello2"}
            };

            // transform
            List<TransformedObject> transformed = new List<TransformedObject>();
            await starter.StartNewAsync("Orchestrator", transformed);

            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}