namespace UnquotedJsonExample.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Primitives

[<ApiController>]
[<Route("[controller]/[action]")>]
type LoggerController(_logger:ILogger<LoggerController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Test1() =   
        _logger.LogInformation("first message in controller")
        0
