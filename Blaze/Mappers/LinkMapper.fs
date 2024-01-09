namespace Blaze.Mappers

open Blaze.DTOs
open Blaze.Types
open Blaze.Primitives
open Blaze.Errors.AccumulativeValidatorBuilder

module LinkMapper =
    let ToDomain(dto: LinkDto): Result<Link, string list> =
        validate {
            let! shortUrl = ShortUrl.create dto.ShortUrl
            and! destinationUrl = DestinationUrl.create dto.DestinationUrl
            return Link(shortUrl, destinationUrl)
        }

    let FromDomain(link: Link): LinkDto =
        let shortUrl = link.ShortUrl
        let destinationUrl = link.DestinationUrl
        LinkDto(shortUrl, destinationUrl)
