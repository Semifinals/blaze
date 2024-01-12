namespace Blaze.Triggers

open Microsoft.Azure.WebJobs
open Blaze.Types
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Extensions.Http

module GetAllLinksTrigger =
    [<FunctionName("GetAllLinksTrigger")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = null)>] req: HttpRequest)
        ([<CosmosDB("%CosmosConnectionString%", "links", DatabaseName = "links-db", Id = "{shortUrl}")>] link: Link) =
            // TODO: Change above to create client or read all links
            NotFoundResult() :> IActionResult
