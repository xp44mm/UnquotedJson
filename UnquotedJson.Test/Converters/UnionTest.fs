namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type UionExample =
| Zero
| OnlyOne of int
| Pair of int * string

type UnionTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``demo``() =
        let x = [Zero;OnlyOne 1;Pair(2,"b")]
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "[\"Zero\",{\"OnlyOne\":1},{\"Pair\":[2,\"b\"]}]"

    [<Fact>]
    member this.``serialize zero union case``() =
        let x = Zero
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "\"Zero\""

    [<Fact>]
    member this.``deserialize zero union case``() =
        let x = "\"Zero\""
        let y = deserialize<UionExample> x
        //output.WriteLine(Render.stringify y)
        Should.equal y Zero

    [<Fact>]
    member this.``serialize only-one union case``() =
        let x = OnlyOne 1
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y """{"OnlyOne":1}"""

    [<Fact>]
    member this.``deserialize only-one union case``() =
        let x = """{"OnlyOne":1}"""
        let y = deserialize<UionExample> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| OnlyOne 1

    [<Fact>]
    member this.``serialize many params union case``() =
        let x = Pair(1,"")
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y """{"Pair":[1,""]}"""

    [<Fact>]
    member this.``deserialize many params union case``() =
        let x = """{"Pair":[1,""]}"""
        let y = deserialize<UionExample> x
        Should.equal y <| Pair(1,"")

    [<Fact>]
    member this.``read zero union case``() =
        let x = Zero
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.String "Zero"

    [<Fact>]
    member this.``write zero union case``() =
        let x = JsonValue.Object ["Zero",JsonValue.Null]
        let y = JSON.write<UionExample> x
        //output.WriteLine(Render.stringify y)
        Should.equal y Zero

    [<Fact>]
    member this.``read only-one union case``() =
        let x = OnlyOne 1
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.Object ["OnlyOne",JsonValue.Number 1.0]

    [<Fact>]
    member this.``write only-one union case``() =
        let x = JsonValue.Object ["OnlyOne",JsonValue.Number 1.0]
        let y = JSON.write<UionExample> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| OnlyOne 1

    [<Fact>]
    member this.``read many params union case``() =
        let x = Pair(1,"")
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.Object ["Pair",JsonValue.Array [JsonValue.Number 1.0;JsonValue.String ""]]

    [<Fact>]
    member this.``write many params union case``() =
        let x = JsonValue.Object ["Pair",JsonValue.Array [JsonValue.Number 1.0;JsonValue.String ""]]
        let y = JSON.write<UionExample> x
        Should.equal y <| Pair(1,"")
