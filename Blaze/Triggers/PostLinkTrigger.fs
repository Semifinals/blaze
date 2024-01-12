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
        ([<HttpTrigger(AuthorizationLevel.Function, "post", Route = null)>] dto: LinkDto)
        ([<CosmosDB("%CosmosConnectionString%", "links", DatabaseName = "links-db")>] db: Link outref) =
            match LinkMapper.ToDomain(dto) with
            | Ok link ->
                db <- link
                CreatedResult($"/{link.ShortUrl}", LinkMapper.FromDomain(link)) :> IActionResult
            | Error errors ->
                BadRequestObjectResult(errors) :> IActionResult
                