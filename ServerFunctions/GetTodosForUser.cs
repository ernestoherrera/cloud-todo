using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;

using Newtonsoft.Json;

namespace SpartanSystems.TodoFuncs
{
    internal class todoItem
    {
        public int userid {get; set;}
        public int todoid {get; set;}      
        public Guid id {get; set;}
        public int priority {get; set;}
        public string action {get; set;}
        public bool done {get; set;}
        public DateTime date {get; set;}
        public DateTime duedate {get; set;}
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    internal class Configuration
    {
        internal static string database = "cosmosAppDb";
        internal static string containerName = "todo";
        internal static string key = "fpSvNKdbDvGArkDix2Jp45FA4FgyVmD7BEoZ1oTulmxj5gssCco3LXZL4SVM5OG9eFxYLvBAOhebyl8qO4jKjg==";
        internal static string endpointUrl = "https://az204cosmosaccount.documents.azure.com:443/";
    }
    public static class GetTodosForUser
    {
        [FunctionName("GetTodosForUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP GetTododsForUser trigger function processed a request.");

            string userId = req.Query["userId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            userId = String.IsNullOrEmpty(userId) ?  (string)data?.userId : userId;  
            
            if (String.IsNullOrEmpty(userId))
                return new OkObjectResult("[]");

            var result = new List<todoItem>();

            using(CosmosClient client = new CosmosClient(Configuration.endpointUrl, Configuration.key))
            {
                Database db_conn = client.GetDatabase(Configuration.database);
                Container container_conn = db_conn.GetContainer(Configuration.containerName);
                string selecttodos = $"select * from c where c.userid={userId}";
                QueryDefinition query = new QueryDefinition(selecttodos);
                FeedIterator<todoItem> iterator = container_conn.GetItemQueryIterator<todoItem>(query);
                
                while(iterator.HasMoreResults)
                {
                    Microsoft.Azure.Cosmos.FeedResponse<todoItem> todoObj = await iterator.ReadNextAsync();

                    result.AddRange(todoObj.ToList());
                }
            }
            return new JsonResult(result);
        }
    }
}
