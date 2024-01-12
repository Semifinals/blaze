namespace Blaze.Triggers

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.Azure.Cosmos

module DeleteLinkTrigger =
    [<FunctionName("DeleteLinkTrigger")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = "{shortUrl}")>] req: HttpRequest)
        ([<CosmosDB("%CosmosConnectionString%", "links", DatabaseName = "links-db")>] container: Container)
        (shortUrl: string) =
            container.DeleteItemAsync(shortUrl, PartitionKey(shortUrl))
                |> Async.AwaitTask
                |> Async.RunSynchronously
                |> ignore
                
            NoContentResult() :> IActionResult
            