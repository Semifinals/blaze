namespace Blaze

open Microsoft.Azure.Functions.Extensions.DependencyInjection
open Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
open Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
open Microsoft.Extensions.DependencyInjection
open Microsoft.OpenApi.Models
open System.Threading.Tasks

type Startup() =
    inherit FunctionsStartup()

    override _.Configure(builder: IFunctionsHostBuilder) =
        builder.Services.AddSingleton<IOpenApiConfigurationOptions>(fun _ -> 
            OpenApiConfigurationOptions(
                Info = OpenApiInfo(
                    Version = DefaultOpenApiConfigurationOptions.GetOpenApiDocVersion(),
                    Title = DefaultOpenApiConfigurationOptions.GetOpenApiDocTitle(),
                    Description = DefaultOpenApiConfigurationOptions.GetOpenApiDocDescription()),
                OpenApiVersion = DefaultOpenApiConfigurationOptions.GetOpenApiVersion(),
                ForceHttps = DefaultOpenApiConfigurationOptions.IsHttpsForced(),
                ForceHttp = DefaultOpenApiConfigurationOptions.IsHttpForced()
            ) :> IOpenApiConfigurationOptions
        ) |> ignore
        
        builder.Services.AddSingleton<IOpenApiHttpTriggerAuthorization>(fun _ ->
            OpenApiHttpTriggerAuthorization(fun req ->
                Task.FromResult(Unchecked.defaultof<OpenApiAuthorizationResult>)
            ) :> IOpenApiHttpTriggerAuthorization
        ) |> ignore
        
[<assembly: FunctionsStartup(typeof<Startup>)>]
do()
