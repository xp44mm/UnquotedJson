namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type SetTest(output: ITestOutputHelper) =

    [<Fact>]
    member _.``serialize set``() =
        let x = set [1;2;3]
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "[1,2,3]"

    [<Fact>]
    member _.``deserialize set``() =
        let x = "[1,2,3]"
        let y = deserialize<Set<int>> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| set[1;2;3]

    [<Fact>]
    member _.``read set``() =
        let x = set [1;2;3]
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.Array [JsonValue.Number 1.0;JsonValue.Number 2.0;JsonValue.Number 3.0]

    [<Fact>]
    member _.``write set``() =
        let x = JsonValue.Array [JsonValue.Number 1.0;JsonValue.Number 2.0;JsonValue.Number 3.0]
        let y = JSON.write<Set<int>> x
        //output.WriteLine(Render.stringify y)
        Should.equal y (set[1;2;3])
