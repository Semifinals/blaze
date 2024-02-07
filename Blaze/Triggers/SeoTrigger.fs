namespace Blaze.Triggers

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Semifinals.Apim.Attributes
open System.Net

module SeoTrigger =
    [<FunctionName("SeoTrigger")>]
    [<Operation(Summary = "Get robots.txt", Description = "Returns the robots.txt for the Blaze API")>]
    [<ResponseBody(HttpStatusCode.OK, "text/plain", typeof<string>, Summary = "Successful operation", Description = "Occurs when the request was successful")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "robots.txt")>] req: HttpRequest)
        : IActionResult =
            OkObjectResult("User-agent: *\nDisallow: /")
        