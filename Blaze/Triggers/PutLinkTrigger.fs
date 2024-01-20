namespace Blaze.Triggers

open Blaze.DTOs
open Blaze.Mappers
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
        (shortUrl: string) =
            try
                // Attempt to shortcircuit if the link already exists
                if existing.DestinationUrl = destinationUrl then
                    OkObjectResult(LinkMapper.FromDomain(existing)) :> IActionResult
                else
                    raise (KeyNotFoundException())
            with | _ ->
                // Create and validate dto
                let dto = new LinkDto(shortUrl, destinationUrl)
            
                match LinkMapper.ToDomain(dto) with
                | Ok link ->
                    // Add to database
                    db <- link
                    CreatedResult($"/{link.ShortUrl}", LinkMapper.FromDomain(link)) :> IActionResult
                | Error errors ->
                    // Return errors
                    BadRequestObjectResult(errors) :> IActionResult
                