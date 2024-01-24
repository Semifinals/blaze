namespace Blaze.Triggers

open Blaze.DTOs
open Blaze.Types
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open System.Collections.Generic

module PostLinkTrigger =
    [<FunctionName("PostLinkTrigger")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Function, "put", Route = "{shortUrl}")>] destinationUrl: string)
        ([<CosmosDB(containerName = "links", databaseName = "links-db", Connection = "CosmosConnectionString", Id = "{shortUrl}", PartitionKey = "{shortUrl}")>] existing: Link)
        ([<CosmosDB(containerName = "links", databaseName = "links-db", Connection = "CosmosConnectionString")>] db: Link outref)
        (shortUrl: string)
        : IActionResult =
            try
                // Attempt to shortcircuit if the link already exists
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
                