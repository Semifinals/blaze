namespace Blaze.Primitives

open Blaze.Errors
open System

module DestinationUrl =
    let nameof = "destinationUrl"

    let create (s: string) =
        match s with
        | null -> StringErrors.Null nameof
        | _ when s.Length > 512 -> StringErrors.LongerThan nameof 512
        | _ when not <| Uri.IsWellFormedUriString(s, UriKind.Absolute) -> StringErrors.Uri nameof
        | _ -> Ok s
        