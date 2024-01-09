namespace Blaze.Primitives

open Blaze.Errors

module ShortUrl =
    let nameof = "shortUrl"

    let create (s: string) =
        match s with
        | null -> StringErrors.Null nameof
        | _ when s.Length > 512 -> StringErrors.LongerThan nameof 512
        | _ -> Ok s
