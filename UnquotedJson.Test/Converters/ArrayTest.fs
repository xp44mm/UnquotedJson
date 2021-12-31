namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type ArrayTest(output: ITestOutputHelper) =

    [<Fact>]
    member _.``serialize array``() =
        let x = [|1;2;3|]
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "[1,2,3]"

    [<Fact>]
    member _.``deserialize array``() =
        let x = "[1,2,3]"
        let y = deserialize<int[]> x
        //output.WriteLine(Render.stringify y)
        Should.equal y [|1;2;3|]

    [<Fact>]
    member _.``read array``() =
        let x = [|1;2;3|]
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.Array [JsonValue.Number 1.0;JsonValue.Number 2.0;JsonValue.Number 3.0]

    [<Fact>]
    member _.``write array``() =
        let x = JsonValue.Array [JsonValue.Number 1.0;JsonValue.Number 2.0;JsonValue.Number 3.0]
        let y = JSON.write<int[]> x
        //output.WriteLine(Render.stringify y)
        Should.equal y [|1;2;3|]

    [<Fact>]
    member _.``index array``() =
        let x = JsonValue.Array [JsonValue.Number 1.0;JsonValue.Number 2.0;JsonValue.Number 3.0]
        let y = x.[1]
        Should.equal y <| JsonValue.Number 2.0


