namespace Blaze.DTOs

open Newtonsoft.Json

type Pagination(?pageSize: int, ?pageNumber: int) =
    [<JsonProperty("pageSize")>]
    member val PageSize = pageSize |> defaultArg <| 10 with get, set

    [<JsonProperty("pageCount")>]
    member val PageNumber = pageNumber |> defaultArg <| 1 with get, set
