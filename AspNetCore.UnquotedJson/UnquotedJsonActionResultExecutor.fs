namespace AspNetCore

open System.Text
open System.Threading.Tasks

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Infrastructure
open Microsoft.AspNetCore.Mvc.Formatters
open Microsoft.Extensions.Primitives
open Microsoft.Net.Http.Headers

open FSharp.Idioms
open FSharp.Idioms.Jsons

[<System.Obsolete("")>]
type UnquotedJsonActionResultExecutor
    (
        writerFactory:IHttpResponseStreamWriterFactory
        //logger:ILogger<UnquotedJsonActionResultExecutor>
    ) =
    interface IActionResultExecutor<JsonResult> with
        member this.ExecuteAsync(context:ActionContext, result:JsonResult) =
            // 如果结果已经是Json类型不变，否则将其他类型转化为Json类型的数据
            let json = 
                match result.Value with
                | :? Json as json -> json
                | value -> 
                    Json.fromObj (value.GetType()) value

            let text = Json.print json

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
