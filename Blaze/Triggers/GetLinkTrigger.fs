namespace Blaze.Triggers

open Microsoft.Azure.WebJobs
open Blaze.Types
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Extensions.Http

module GetLinkTrigger =
    [<FunctionName("GetLinkTrigger")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = "{shortUrl}")>] req: HttpRequest)
        ([<CosmosDB("%CosmosConnectionString%", "links", DatabaseName = "links-db", Id = "{shortUrl}")>] link: Link) =
            RedirectResult(link.DestinationUrl) :> IActionResult
