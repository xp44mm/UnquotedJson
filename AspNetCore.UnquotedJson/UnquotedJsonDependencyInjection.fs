module AspNetCore.UnquotedJsonDependencyInjection

open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Infrastructure

[<System.Obsolete("")>]
let AddUnquotedJson(services:IServiceCollection) =
    let jsonResultExecutor = 
        services
        |> Seq.filter(fun f -> 
            f.ServiceType = typeof<IActionResultExecutor<JsonResult>> &&
            f.ImplementationType.Assembly = typeof<JsonResult>.Assembly
            )
        //|> Seq.filter(fun f -> )

    if Seq.isEmpty jsonResultExecutor then
        ()
    else
        let e = Seq.exactlyOne jsonResultExecutor
        services.Remove(e) |> ignore

    services.AddSingleton<IActionResultExecutor<JsonResult>, UnquotedJsonActionResultExecutor>()
