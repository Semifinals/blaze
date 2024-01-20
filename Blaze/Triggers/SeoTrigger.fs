namespace Blaze.Triggers

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http

module SeoTrigger =
    [<FunctionName("SeoTrigger")>]
    let Run ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "robots.txt")>] req: HttpRequest) =
        OkObjectResult("User-agent: *\nDisallow: /")
        