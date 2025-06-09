namespace UnquotedJsonExample.Controllers

open UnquotedJsonExample

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

open UnquotedJson
open AspNetCore
open AspNetCore.UnquotedJson

[<ApiController>]
[<Route("[controller]/[action]")>]
type UnquotedJsonController(log:ILogger<UnquotedJsonController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.emptyParam() = 
        let req = this.Request
        let pairs =
            req.Query
            |> Query.toPairs
            |> Seq.toArray

        JsonResult(pairs)
        
    [<HttpGet>]
    member this.apostrophe() = 
        let req = this.Request
        let pairs =
            req.Query
            |> Query.toPairs
            |> Seq.toArray

        let res =
            pairs
            |> Array.map(fun(k,v)-> k, UrlQuery.parseField<string[]> v)
            |> Map.ofArray

        log.LogInformation(sprintf "%A" res)

        JsonResult(res)

    [<HttpGet>]
    member this.parseField() =
        let req = this.Request
        let mp =
            req.Query
            |> Query.toPairs
            |> Map.ofSeq

        let res = {|
            section = UrlQuery.parseField<SectionInput> mp.["section"]
            panel = UrlQuery.parseField<PanelInput> mp.["panel"]
        |}

        JsonResult(res)

    [<HttpGet>]
    member this.SectionInput() =
        let req = this.Request
        let pairs =
            req.Query
            |> Query.toPairs
            |> Seq.toArray

        let res = UrlQuery.parseQuery<SectionInput> pairs
        log.LogInformation(sprintf "%A" res)
        JsonResult(res)

    [<HttpGet>]
    member this.HorizDuctInput() =
        let req = this.Request
        let pairs =
            req.Query
            |> Query.toPairs
            |> Seq.toArray

        let res = UrlQuery.parseQuery<HorizDuctInput> pairs
        log.LogInformation(sprintf "%A" res)
        JsonResult(res)

    [<HttpGet>]
    member this.tuple() =
        let res = ("1",1)
        JsonResult(res)
