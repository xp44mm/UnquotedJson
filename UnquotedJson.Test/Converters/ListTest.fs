namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type ListTest(output: ITestOutputHelper) =
    
    [<Fact>]
    member this.``serialize list``() =
        let x = [1;2;3]
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "[1,2,3]"

    [<Fact>]
    member this.``deserialize list``() =
        let x = "[1,2,3]"
        let y = deserialize<List<int>> x
        //output.WriteLine(Render.stringify y)
        Should.equal y [1;2;3]


    [<Fact>]
    member this.``read list``() =
        let x = [1;2;3]
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.Array [JsonValue.Number 1.0;JsonValue.Number 2.0;JsonValue.Number 3.0]

    [<Fact>]
    member this.``write list``() =
        let x = JsonValue.Array [JsonValue.Number 1.0;JsonValue.Number 2.0;JsonValue.Number 3.0]
        let y = JSON.write<List<int>> x
        //output.WriteLine(Render.stringify y)
        Should.equal y [1;2;3]
