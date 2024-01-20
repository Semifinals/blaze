namespace Blaze.DTOs

open Newtonsoft.Json

type Pagination(pageSize: int option, pageNumber: int option) =
    [<JsonProperty("pageSize")>]
    member val PageSize =
        if pageSize.IsSome
        then pageSize.Value
        else 10
        with get, set

    [<JsonProperty("pageCount")>]
    member val PageNumber =
        if pageNumber.IsSome
        then pageNumber.Value
        else 1
        with get, set
    