namespace Blaze.Triggers

open Blaze.Types
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.OpenApi.Models
open Semifinals.Apim.Attributes
open System.Net

module GetLinkTrigger =
    [<FunctionName("GetLinkTrigger")>]
    [<Operation(Summary = "Redirect to a destination", Description = "Uses a short URL to redirect to a different destination")>]
    [<Parameter("shortUrl", In = ParameterLocation.Path, Required = true, Type = typeof<string>, Summary = "The short URL", Description = "Defines the short URL for the desination to go to")>]
    [<Response(HttpStatusCode.Redirect, Summary = "Redirect to destination", Description = "Indicates the request was successful and redirects to the intended destination")>]
    [<Response(HttpStatusCode.NotFound, Summary = "Short URL not found", Description = "Occurs when no destination URL has been set for the given short URL")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{shortUrl:regex(^(?!robots.txt$)(?!swagger\..+$).*)}")>] req: HttpRequest)
        ([<CosmosDB(containerName = "links", databaseName = "links-db", Connection = "CosmosConnectionString", Id = "{shortUrl}", PartitionKey = "{shortUrl}")>] link: Link)
        : IActionResult =            
            try RedirectResult(link.DestinationUrl)
            with | _ -> NotFoundResult()
            