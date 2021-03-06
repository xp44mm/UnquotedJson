namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type TupleTest(output: ITestOutputHelper) =
        
    [<Fact>]
    member _.``serialize array``() =
        let x = (1,"x")
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y """[1,"x"]"""

    [<Fact>]
    member _.``deserialize array``() =
        let x = """[1,"x"]"""
        let y = deserialize<int*string> x
        //output.WriteLine(Render.stringify y)
        Should.equal y (1,"x")

    [<Fact>]
    member _.``read array``() =
        let x = (1,"x")
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.Array [JsonValue.Number 1.0;JsonValue.String "x"]

    [<Fact>]
    member _.``write array``() =
        let x = JsonValue.Array [JsonValue.Number 1.0;JsonValue.String "x"]
        let y = JSON.write<int*string> x
        //output.WriteLine(Render.stringify y)
        Should.equal y (1,"x")
