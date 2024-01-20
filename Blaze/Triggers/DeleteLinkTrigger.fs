namespace Blaze.Triggers

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.Azure.Cosmos

module DeleteLinkTrigger =
    [<FunctionName("DeleteLinkTrigger")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Function, "delete", Route = "{shortUrl}")>] req: HttpRequest)
        ([<CosmosDB(Connection = "CosmosConnectionString")>] cosmosClient: CosmosClient)
        (shortUrl: string) =
            cosmosClient
                .GetContainer("links-db", "links")
                .DeleteItemAsync(shortUrl, PartitionKey(shortUrl))
                    |> Async.AwaitTask
                    |> Async.RunSynchronously
                    |> ignore
                
            NoContentResult() :> IActionResult
            // return 404 if already deleted?
            