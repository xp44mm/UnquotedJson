namespace UnquotedJson

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal
open FSharp.Idioms.OrdinalIgnoreCase
open FSharp.xUnit

open System.IO
open System.Text

type ComponentRecordJsonTest(output:ITestOutputHelper) =
    let filePath = @"D:\Application Data\GitHub\xp44mm\SwCSharpAddin1\CommandData\model records.json"

    [<Fact>]
    member this.``get all path test``() =
        let rec loop json =
            [
                match json with
                | Json.Object entries -> 
                    for (k,v)in entries do
                        match k,v with
                        | "path", Json.String path -> yield path
                        | "children", arr -> yield! loop arr // 注释掉，结果是一样的
                        | _ -> ()
                | Json.Array elems ->
                    for elem in elems do
                        yield! loop elem
                | _ -> ()
            ]
        let txt = File.ReadAllText(filePath, Encoding.UTF8)
        let json = Json.parse txt

        loop json
        |> Set.ofList
        |> Set.toList
        |> String.concat "\r\n"
        |> output.WriteLine

    [<Fact>]
    member this.``打印所有模型信息``() =
        let txt = File.ReadAllText(filePath, Encoding.UTF8)
        let json = Json.parse txt
        let arr =
            match json with
            | Json.Array elems -> elems
            | _ -> failwith ""
        for json in arr do
            output.WriteLine(Json.print json)

    [<Fact>]
    member this.``展开为``() =
        let txt = File.ReadAllText(filePath,Encoding.UTF8)
        let json = Json.parse txt
        ///
        let elems =
            match json with
            | Json.Array elems -> elems
            | _ -> failwith ""

        let root = elems |> List.last

        /// 分成两部分
        let expanded,original =
            elems
            |> List.partition(fun e ->
                let path = e.["path"].stringText
                Path.GetExtension path == ".SLDPRT"
            )

        let json = 
            ComponentRecordJson.toExpand expanded original
            |> List.find(fun json -> json.["path"].stringText = root.["path"].stringText)

        /// 对于original中的每个json对象执行替换，
        /// 如果json对象的全部children可以替换则替换为expanded
        let folder = Path.GetDirectoryName(filePath)
        let treePath = Path.Combine(folder,"tree.json")
        File.WriteAllText(treePath,Json.print json)
        output.WriteLine(treePath)

