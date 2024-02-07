namespace Blaze.Triggers

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.Cosmos
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.OpenApi.Models
open Semifinals.Apim.Attributes
open System.Net

module DeleteLinkTrigger =
    [<FunctionName("DeleteLinkTrigger")>]
    [<Authorize(1)>]
    [<Operation(Summary = "Delete a link", Description = "Deletes a short link")>]
    [<Parameter("shortUrl", In = ParameterLocation.Path, Required = true, Type = typeof<string>, Summary = "The short URL", Description = "Defines the short URL for the short link to delete")>]
    [<Response(HttpStatusCode.NoContent, Summary = "Successful request", Description = "Indicates the short link was successfully deleted")>]
    [<Response(HttpStatusCode.Unauthorized, Summary = "Unauthorized request", Description = "Occurs when the authentication header is missing or invalid")>]
    [<Response(HttpStatusCode.Forbidden, Summary = "Forbidden request", Description = "Occurs when authorization failed due to missing permissions")>]
    [<Response(HttpStatusCode.NotFound, Summary = "Short URL not found", Description = "Occurs when the requested short URL doesn't exist")>]
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
