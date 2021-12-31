namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type DBNullConverterTest(output: ITestOutputHelper) =
    [<Fact>]
    member _.``serialize DBNull``() =
        let x = DBNull.Value
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "null"

    [<Fact>]
    member _.``deserialize DBNull``() =
        let x = "null"
        let y = deserialize<DBNull> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| DBNull.Value


    [<Fact>]
    member _.``read DBNull``() =
        let x = DBNull.Value
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.Null

    [<Fact>]
    member _.``DBNull instantiate``() =
        let x = JsonValue.Null
        let y = JSON.write<DBNull> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| DBNull.Value

