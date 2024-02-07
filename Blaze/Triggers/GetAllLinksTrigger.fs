namespace Blaze.Triggers

open System.Net
open Blaze.DTOs
open Blaze.Types
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.Cosmos
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.OpenApi.Models
open Semifinals.Apim.Attributes
open System.Collections.Generic

module GetAllLinksTrigger =
    [<FunctionName("GetAllLinksTrigger")>]
    [<Authorize(1)>]
    [<Operation(Summary = "Get all links", Description = "Returns all links currently stored as a paginated list")>]
    [<Parameter("pageSize", In = ParameterLocation.Query, Required = false, Type = typeof<int>, Summary = "The page size parameter", Description = "Defines the number of links to fetch in a single page. Defaults to 10")>]
    [<Parameter("pageCount", In = ParameterLocation.Query, Required = false, Type = typeof<int>, Summary = "The page count parameter", Description = "Defines the page number to fetch. Defaults to 1 (first page)")>]
    [<ResponseBody(HttpStatusCode.OK, "application/json", typeof<LinkDto list>, Summary = "Successful request", Description = "Returns a the list of paginated links as requested by the query parameters")>]
    [<Response(HttpStatusCode.Unauthorized, Summary = "Unauthorized request", Description = "Occurs when the authentication header is missing or invalid")>]
    [<Response(HttpStatusCode.Forbidden, Summary = "Forbidden request", Description = "Occurs when authorization failed due to missing permissions")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = "/")>] req: HttpRequest)
        ([<CosmosDB(Connection = "CosmosConnectionString")>] cosmosClient: CosmosClient)
        : IActionResult =
            // Get pagination from the query string
            let parsed = req.GetQueryParameterDictionary()
            
            let pageSize =
                try Some(parsed["pageSize"] |> int)
                with | :? KeyNotFoundException as _ -> None
                
            let pageCount =
                try Some(parsed["pageCount"] |> int)
                with | :? KeyNotFoundException as _ -> None
            
            let page = new Pagination(pageSize, pageCount)
            
            // Create iterator to fetch links
            let offset = (page.PageNumber - 1) * page.PageSize
            let limit = page.PageSize
            let query = (QueryDefinition("SELECT * FROM c OFFSET @offset LIMIT @limit")
                .WithParameter("@offset", offset)
                .WithParameter("@limit", limit))
            let iterator = (cosmosClient
                .GetContainer("links-db", "links")
                .GetItemQueryIterator(query))
            
            // Get all links
            let mutable links: LinkDto list = []
            async {
                while iterator.HasMoreResults do
                    let! batch = iterator.ReadNextAsync() |> Async.AwaitTask
                    links <- links @ (batch.Resource
                        :?> Link array
                        |> Array.toList
                        |> List.map(fun link -> new LinkDto(link.ShortUrl, link.DestinationUrl)))
            } |> Async.RunSynchronously

            // Return requested links
            OkObjectResult(links)
            