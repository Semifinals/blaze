namespace Blaze.Types

open Newtonsoft.Json

type Link(shortUrl: string, destinationUrl: string) =
    [<JsonProperty("id")>]
    member val ShortUrl = shortUrl with get, set

    [<JsonProperty("destinationUrl")>]
    member val DestinationUrl = destinationUrl with get, set
