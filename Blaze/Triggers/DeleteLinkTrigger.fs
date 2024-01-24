namespace Blaze.Triggers

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.Cosmos
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http

module DeleteLinkTrigger =
    [<FunctionName("DeleteLinkTrigger")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Function, "delete", Route = "{shortUrl}")>] req: HttpRequest)
        ([<CosmosDB(Connection = "CosmosConnectionString")>] cosmosClient: CosmosClient)
        (shortUrl: string)
        : IActionResult =
            let res =
                try
                    cosmosClient
                        .GetContainer("links-db", "links")
                        .DeleteItemAsync(shortUrl, PartitionKey(shortUrl))
                            |> Async.AwaitTask
                            |> Async.RunSynchronously
                            |> ignore
                    Ok()
                with
                | _ -> Error()

            match res with
            | Ok _ -> NoContentResult()
            | Error _ -> NotFoundResult()
                                
            