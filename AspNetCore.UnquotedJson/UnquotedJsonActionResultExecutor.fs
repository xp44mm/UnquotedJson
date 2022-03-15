namespace AspNetCore

open System.Text
open System.Threading.Tasks

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Infrastructure
open Microsoft.AspNetCore.Mvc.Formatters
open Microsoft.Extensions.Primitives
open Microsoft.Net.Http.Headers

open UnquotedJson
open FSharp.Control.Tasks.V2

type UnquotedJsonActionResultExecutor
    (
        writerFactory:IHttpResponseStreamWriterFactory
        //logger:ILogger<UnquotedJsonActionResultExecutor>
    ) =
    interface IActionResultExecutor<JsonResult> with
        member this.ExecuteAsync(context:ActionContext, result:JsonResult) =
            let json = 
                match result.Value with
                | :? JsonValue as json -> json
                | value -> JSON.readDynamic (value.GetType()) value

            let text = JSON.stringifyNormalJson json

            let response = context.HttpContext.Response
            response.ContentType <- 
                match result.ContentType with
                | null | "" -> 
                    let x = MediaTypeHeaderValue(StringSegment "application/json")
                    x.Encoding <- Encoding.UTF8
                    x.ToString()
                | x -> x

            if result.StatusCode.HasValue then
                response.StatusCode <- result.StatusCode.Value

            task {
                use writer = writerFactory.CreateWriter(response.Body, MediaType.GetEncoding(response.ContentType))
                do! writer.WriteAsync(text)
                do! writer.FlushAsync()
            }
            :> Task