namespace Blaze.Triggers

open Microsoft.Azure.WebJobs
open Microsoft.Extensions.Logging

module WarmupTrigger =
    [<FunctionName("WarmupTrigger")>]
    let Run
        ([<TimerTrigger("0 */10 * * * *")>]myTimer: TimerInfo)
        (logger: ILogger) =
            logger.LogInformation("Warmed up instance")
            // If above doesn't work, use the code below to ping the HTTP endpoint.
            // Need to find a good way to get the current domain (maybe just an env
            // variable).
            
            // task {
            //     use client = new HttpClient()
            //     let! response = client.GetAsync("")
            //     ()
            // }
            // |> Async.AwaitTask
            // |> Async.RunSynchronously
            