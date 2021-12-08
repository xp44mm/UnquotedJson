namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type MapTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``serialize map``() =
        let x = Map [1,"1";2,"2"]
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y """[[1,"1"],[2,"2"]]"""

    [<Fact>]
    member this.``deserialize map``() =
        let x = """[[1,"1"],[2,"2"]]"""
        let y = deserialize<Map<int,string>> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| Map.ofList [1,"1";2,"2"]

    [<Fact>]
    member this.``read map``() =
        let x = Map.ofList [1,"1";2,"2"]
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y 
        <| JsonValue.Array [JsonValue.Array [JsonValue.Number 1.0;JsonValue.String "1"];JsonValue.Array [JsonValue.Number 2.0;JsonValue.String "2"]]

    [<Fact>]
    member this.``write map``() =
        let x = JsonValue.Array [JsonValue.Array [JsonValue.Number 1.0;JsonValue.String "1"];JsonValue.Array [JsonValue.Number 2.0;JsonValue.String "2"]]
        let y = JSON.write<Map<int,string>> x
        //output.WriteLine(Render.stringify y)
        Should.equal y (Map.ofList [1,"1";2,"2"])


