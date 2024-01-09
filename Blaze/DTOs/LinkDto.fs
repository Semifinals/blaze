namespace Blaze.DTOs

open Newtonsoft.Json

type LinkDto(shortUrl: string, destinationUrl: string) =
    [<JsonProperty("shortUrl")>]
    member val ShortUrl = shortUrl with get, set

    [<JsonProperty("destinationUrl")>]
    member val DestinationUrl = destinationUrl with get, set
