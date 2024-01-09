module Blaze.Errors.AccumulativeValidatorBuilder

type AccumulativeValidatorBuilder() =
    member _.BindReturn(result, f) =
        result |> Result.map f

    member _.MergeSources(result1, result2) =
        match result1, result2 with
        | Ok ok1, Ok ok2 -> Ok (ok1, ok2)
        | Error errs1, Ok _ -> Error errs1
        | Ok _, Error errs2 -> Error errs2
        | Error errs1, Error errs2 -> Error (errs1 @ errs2)

let validate = AccumulativeValidatorBuilder()
