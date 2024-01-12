namespace Blaze.Triggers

open Microsoft.Azure.WebJobs
open Blaze.Types
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.Azure.Cosmos
open Blaze.DTOs

module GetAllLinksTrigger =
    [<FunctionName("GetAllLinksTrigger")>]
    let Run
        ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = null)>] req: HttpRequest)
        ([<CosmosDB("%CosmosConnectionString%", "links", DatabaseName = "links-db")>] container: Container)
        ([<FromQuery>] page: Pagination) =
            // Create iterator to fetch links
            let offset = (page.PageNumber - 1) * page.PageSize
            let limit = page.PageSize
            let query = QueryDefinition("SELECT * FROM c OFFSET @offset LIMIT @limit").WithParameter("@offset", offset).WithParameter("@limit", limit)
            let iterator = container.GetItemQueryIterator(query)
            
            // Get all links
            let mutable links: Link list = []
            async {
                while iterator.HasMoreResults do
                    let! batch = iterator.ReadNextAsync() |> Async.AwaitTask
                    links <- links @ (batch.Resource :?> Link list)
            } |> Async.RunSynchronously

            // Return requested links
            OkObjectResult(links) :> IActionResult
            