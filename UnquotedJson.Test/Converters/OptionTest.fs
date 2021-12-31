namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type OptionTest(output: ITestOutputHelper) =
    [<Fact>]
    member _.``serialize none``() =
        let x = None
        let y = serialize x
        Should.equal y "null"

    [<Fact>]
    member _.``deserialize none``() =
        let x = "null"
        let y = deserialize<_ option> x
        //output.WriteLine(Render.stringify y)
        Should.equal y None

    [<Fact>]
    member _.``serialize some``() =
        let x = Some 1
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "1"

    [<Fact>]
    member _.``deserialize some``() =
        let x = "1"
        let y = deserialize<int option> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| Some 1

    [<Fact>]
    member _.``read none``() =
        let x = None
        let y = JSON.read<int option> x
        Should.equal y JsonValue.Null

    [<Fact>]
    member _.``write none``() =
        let x = JsonValue.Null
        let y = JSON.write<int option> x

        //output.WriteLine(Render.stringify y)
        Should.equal y None

    [<Fact>]
    member _.``read some``() =
        let x = Some 1
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.Number 1.0

    [<Fact>]
    member _.``write some``() =
        let x = JsonValue.Number 1.0
        let y = JSON.write<int option> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| Some 1




