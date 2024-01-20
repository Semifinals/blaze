namespace Blaze.Triggers

open Blaze.DTOs
open Blaze.Mappers
open Blaze.Types
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http

module PostLinkTrigger =
    [<FunctionName("PostLinkTrigger")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Function, "put", Route = "{shortUrl}")>] destinationUrl: string)
        ([<CosmosDB(containerName = "links", databaseName = "links-db", Connection = "CosmosConnectionString")>] db: Link outref)
        (shortUrl: string) =
            let dto = new LinkDto(shortUrl, destinationUrl)
            
            match LinkMapper.ToDomain(dto) with
            | Ok link ->
                db <- link
                OkObjectResult(LinkMapper.FromDomain(link)) :> IActionResult
            | Error errors ->
                BadRequestObjectResult(errors) :> IActionResult
                