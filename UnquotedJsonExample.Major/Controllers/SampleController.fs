namespace UnquotedJsonExample.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

open FSharp.Idioms
open AspNetCore

[<ApiController>]
[<Route("[controller]/[action]")>]
type SampleController(_logger:ILogger<SampleController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Pairs() = 
        let req = this.Request
        let kvps = 
            req.Query 
            |> Query.toPairs 
            |> List.ofSeq

        _logger.LogInformation("{0}{1}",req.QueryString.Value,Literal.stringify(kvps))

        JsonResult kvps

    [<HttpGet>]
    member this.percentEncodeTest() = 
        let req = this.Request;
        //这三个值是相同的，都是escaped
        _logger.LogInformation("Value:{0}",req.QueryString.Value)
        _logger.LogInformation("ToString:{0}",req.QueryString.ToString())
        _logger.LogInformation("ToUriComponent:{0}",req.QueryString.ToUriComponent())

        let kvps = 
            req.Query 
            |> Query.toPairs
        _logger.LogInformation(Literal.stringify (List.ofSeq kvps))
