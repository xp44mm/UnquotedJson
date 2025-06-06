namespace UnquotedJson

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.xUnit


open System.Collections.Generic

type UrlQueryTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member this.``control char test``() =
        let x = @"[""\u0002""]"
        let y = UrlQuery.parseField<string[]> x

        //show y
        Should.equal y [|"\u0002"|]

    [<Fact>]
    member this.``parseField test``() =
        let x = "[xyz,\"123\"]"
        let y = UrlQuery.parseField<string[]> x
        let z = [|"xyz";"123"|]
        Should.equal y z

    [<Fact>]
    member this.``parseField dynamic test``() =
        let x = "[xyz,\"123\"]"
        let y = UrlQuery.parseFieldDynamic typeof<string[]> x
        let z = [|"xyz";"123"|] |> box
        Should.equal y z

    [<Fact>]
    member this.``parse test``() =
        let x = ["foo","bar";"abc","[xyz,\"123\"]"]
        let y = UrlQuery.parseQueryDynamic typeof<{|foo:string;abc:string[]|}> x
        let z = box {|abc=[|"xyz";"123"|];foo="bar"|}
        Should.equal y z

    [<Fact>]
    member this.``现代浏览器url的query部分可以不转义的字符``() =
        // 非字母非数字 in ascii
        let nw = set [
            for i in 33..126 do
                let c = char i
                if System.Char.IsAsciiLetterOrDigit c then
                    ()
                else c
        ]
        // 必须 percent code 转义
        let st = set [ '=';'&';'#' ]

        let ls1 = [
            for c in st do
            let x = sprintf "%X" (int c)
            $"{x}=%%{x}"
        ]
        output.WriteLine(String.concat "&" ls1)

        // 保持原样
        let ls2 = [
            for c in nw - st do
            let x = sprintf "%X" (int c)
            $"{x}={c}"
        ]

        output.WriteLine(String.concat "&" ls2)

