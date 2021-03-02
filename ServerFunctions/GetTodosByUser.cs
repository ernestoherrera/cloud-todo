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
    public class todo
    {
        public int userid {get; set;}        
        public Guid id {get; set;}
        public int todoid {get; set;}
        public string action {get; set;}
        public bool done {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public static class GetTodosByUser
    {
        internal static string functionName = "GetTodosByUser";
        internal static string database = "cosmosAppDb";
        internal static string containerName = "cloud-todo-db";
        internal static string key = "fpSvNKdbDvGArkDix2Jp45FA4FgyVmD7BEoZ1oTulmxj5gssCco3LXZL4SVM5OG9eFxYLvBAOhebyl8qO4jKjg==";
        internal static string endpointUrl = "https://az204cosmosaccount.documents.azure.com:443/";

        [FunctionName("GetTodosByUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# Function {functionName} HTTP trigger processed a request.");

            string userId = req.Query["userId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            userId = String.IsNullOrEmpty(userId) ?  (string)data?.userId : userId;  
            
            if (String.IsNullOrEmpty(userId))
                return new OkObjectResult("[]");

            var result = new List<todo>();

            using(var client = new CosmosClient(endpointUrl, key))
            {
                var db_conn = client.GetDatabase(database);
                var container_conn = db_conn.GetContainer(containerName);
                var selecttodos = $"select * from c where c.userid={userId}";
                QueryDefinition query = new QueryDefinition(selecttodos);
                FeedIterator<todo> iterator = container_conn.GetItemQueryIterator<todo>(query);
                
                while(iterator.HasMoreResults)
                {
                    FeedResponse<todo> todoObj = await iterator.ReadNextAsync();

                    result.AddRange(todoObj.ToList());
                }
            }
            return new JsonResult(result);
        }
    }
}
