namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type DateTimeOffsetConverterTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize DateTimeOffset``() =
        let x = DateTimeOffset(2021,2,11,9,2,18,0,TimeSpan(0,8,0,0,0))
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| "\"2021/2/11 9:02:18 +08:00\""

    [<Fact>]
    member this.``deserialize DateTimeOffset``() =
        let x = "\"2021/2/11 9:02:18 +08:00\""
        let y = deserialize<DateTimeOffset> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| DateTimeOffset(2021,2,11,9,2,18,0,TimeSpan(0,8,0,0,0))

    [<Fact>]
    member this.``read DateTimeOffset``() =
        let x = DateTimeOffset(2021,2,11,9,2,18,0,TimeSpan(0,8,0,0,0))
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.String "2021/2/11 9:02:18 +08:00"

    [<Fact>]
    member this.``write DateTimeOffset``() =
        let x = JsonValue.String "2021/2/11 9:02:18 +08:00"
        let y = JSON.write<DateTimeOffset> x

        //output.WriteLine(Render.stringify y)
        Should.equal y <| DateTimeOffset(2021,2,11,9,2,18,0,TimeSpan(0,8,0,0,0))

