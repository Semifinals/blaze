namespace Blaze.Triggers

open Microsoft.Azure.WebJobs
open Blaze.Types
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Extensions.Http
open System

module GetLinkTrigger =
    [<FunctionName("GetLinkTrigger")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{shortUrl:regex(^(?!robots.txt$).*)}")>] req: HttpRequest)
        ([<CosmosDB(containerName = "links", databaseName = "links-db", Connection = "CosmosConnectionString", Id = "{shortUrl}", PartitionKey = "{shortUrl}")>] link: Link) =            
            try RedirectResult(link.DestinationUrl) :> IActionResult
            with | _ -> NotFoundResult() :> IActionResult
            