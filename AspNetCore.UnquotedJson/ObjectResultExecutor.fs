namespace AspNetCore.UnquotedJson

open System
open System.Text
open System.Threading.Tasks

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Infrastructure
open Microsoft.Net.Http.Headers

open FSharp.Idioms
open FSharp.Idioms.Jsons

/// <summary>
/// 自定义 ObjectResult 执行器，强制按以下规则处理响应：
/// 1. 简单类型（primitive/string/date）序列化为 text/plain
/// 2. 复杂类型序列化为 application/json
/// 3. 始终使用 UTF-8 编码
/// 4. 忽略客户端请求的 Accept 头
/// </summary>
/// <remarks>
/// 此实现会覆盖 ASP.NET Core 默认的 JSON/XML 内容协商逻辑。
/// </remarks>
[<Class>]
type ObjectResultExecutor(writerFactory: IHttpResponseStreamWriterFactory) =
    interface IActionResultExecutor<ObjectResult> with
        member this.ExecuteAsync(context: ActionContext, result: ObjectResult) =
            task {
                // 确定本执行器支持的内容类型和转换逻辑
                let executorContentType, text = 
                    match result.Value with
                    | null -> "text/plain", "null"
                    | :? DateTime as dt -> "text/plain", dt.ToString("O")
                    | :? DateTimeOffset as dto -> "text/plain", dto.ToString("O")
                    | :? string as s -> "text/plain", s
                    | :? char as c -> "text/plain", c.ToString()
                    | :? int as i -> "text/plain", i.ToString()
                    | :? float as f -> "text/plain", f.ToString()
                    | :? bool as b -> "text/plain", b.ToString()
                    | :? Guid as g -> "text/plain", g.ToString()

                    | :? Json as json -> "application/json", Json.print json
                    | value -> 
                        let json = Json.fromObj (value.GetType()) value
                        "application/json", Json.print json

                let response = context.HttpContext.Response
                response.ContentType <- 
                    let m = MediaTypeHeaderValue.Parse(executorContentType)
                    m.Encoding <- Encoding.UTF8
                    m.ToString()

                // 设置状态码
                if result.StatusCode.HasValue then
                    response.StatusCode <- result.StatusCode.Value

                // 写入响应体
                use writer = writerFactory.CreateWriter(response.Body, Encoding.UTF8)
                do! writer.WriteAsync(text)
                do! writer.FlushAsync()

            } :> Task
