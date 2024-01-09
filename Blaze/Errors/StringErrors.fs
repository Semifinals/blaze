module Blaze.Errors.StringErrors

let Null (nameof: string) =
    Error [ $"The '{nameof}' field cannot be null" ]

let ShorterThan (nameof: string) (limit: int) =
    Error [ $"The '{nameof}' field cannot be shorter than {limit} characters" ]

let LongerThan (nameof: string) (limit: int) =
    Error [ $"The '{nameof}' field cannot be longer than {limit} characters" ]
