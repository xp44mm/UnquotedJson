namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type TimeSpanConverterTest(output: ITestOutputHelper) =
    [<Fact>]
    member _.``serialize DateTimeOffset``() =
        let x = TimeSpan(2, 14, 18)
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| "\"02:14:18\""

    [<Fact>]
    member _.``deserialize DateTimeOffset``() =
        let x = "\"02:14:18\""
        let y = deserialize<TimeSpan> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| TimeSpan(2, 14, 18)


    [<Fact>]
    member _.``read DateTimeOffset``() =
        let x = TimeSpan(2, 14, 18)
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.String "02:14:18"

    [<Fact>]
    member _.``DateTimeOffset instantiate``() =
        let x = JsonValue.String "02:14:18"
        let y = JSON.write<TimeSpan> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| TimeSpan(2, 14, 18)

