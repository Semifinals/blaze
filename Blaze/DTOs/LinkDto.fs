namespace Blaze.DTOs

open Newtonsoft.Json
open Blaze.Errors.AccumulativeValidatorBuilder
open Blaze.Primitives

type LinkDto(shortUrl: string, destinationUrl: string) =
    [<JsonProperty("shortUrl")>]
    member val ShortUrl = shortUrl with get, set

    [<JsonProperty("destinationUrl")>]
    member val DestinationUrl = destinationUrl with get, set

    member this.IsValid =
        validate {
            let! shortUrl = ShortUrl.create this.ShortUrl
            and! destinationUrl = DestinationUrl.create this.DestinationUrl
            return this
        }
