namespace Blaze.Triggers

open Blaze.DTOs
open Blaze.Types
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.OpenApi.Models
open Semifinals.Apim.Attributes
open System.Collections.Generic
open System.Net

module PostLinkTrigger =
    [<FunctionName("PostLinkTrigger")>]
    [<Authorize(1)>]
    [<Operation(Summary = "Create a link", Description = "Creates a new short link, or replaces an existing one")>]
    [<Parameter("shortUrl", In = ParameterLocation.Path, Required = true, Type = typeof<string>, Summary = "The short URL", Description = "Defines the short URL for the short link to create")>]
    [<RequestBody("text/plain", typeof<string>, Required = true, Description = "The destination URL the short URL should redirect to")>]
    [<ResponseBody(HttpStatusCode.OK, "application/json", typeof<LinkDto>, Summary = "Successful request, already exists", Description = "Indicates the request was successful but that the existing value is identical")>]
    [<ResponseBody(HttpStatusCode.Created, "application/json", typeof<LinkDto>, Summary = "Successful request, created or updated", Description = "Indicates the short URL was successfully created or updated")>]
    [<ResponseBody(HttpStatusCode.BadRequest, "application/json", typeof<string list>, Summary = "Bad request", Description = "Occurs when either the short URL or destination URL are not valid")>]
    [<Response(HttpStatusCode.Unauthorized, Summary = "Unauthorized request", Description = "Occurs when the authentication header is missing or invalid")>]
    [<Response(HttpStatusCode.Forbidden, Summary = "Forbidden request", Description = "Occurs when authorization failed due to missing permissions")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Function, "put", Route = "{shortUrl}")>] destinationUrl: string)
        ([<CosmosDB(containerName = "links", databaseName = "links-db", Connection = "CosmosConnectionString", Id = "{shortUrl}", PartitionKey = "{shortUrl}")>] existing: Link)
        ([<CosmosDB(containerName = "links", databaseName = "links-db", Connection = "CosmosConnectionString")>] db: Link outref)
        (shortUrl: string)
        : IActionResult =
            try
                // Attempt to short-circuit if the link already exists
                if existing.DestinationUrl = destinationUrl then
                    let dto = new LinkDto(existing.ShortUrl, existing.DestinationUrl)
                    OkObjectResult(dto)
                else
                    raise (KeyNotFoundException())
            with | _ ->
                // Create and validate dto
                match (new LinkDto(shortUrl, destinationUrl)).IsValid with
                | Ok dto ->
                    db <- new Link(dto.ShortUrl, dto.DestinationUrl)
                    CreatedResult($"/{dto.ShortUrl}", dto)
                | Error errors ->
                    BadRequestObjectResult(errors)
                