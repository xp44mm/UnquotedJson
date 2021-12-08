namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open System.Reflection
open System.Text.RegularExpressions
open System

type EnumTest(output: ITestOutputHelper) =
    
    [<Fact>]
    member this.``serialize enum``() =
        let x = DateTimeKind.Local
        let y = serialize x
        
        //output.WriteLine(Render.stringify y)
        Should.equal y "\"Local\""

    [<Fact>]
    member this.``deserialize enum``() =
        let x = "\"Utc\""
        let y = deserialize<DateTimeKind> x

        //output.WriteLine(Render.stringify y)
        Should.equal y DateTimeKind.Utc

    [<Fact>]
    member this.``serialize flags``() =
        let x = BindingFlags.Public ||| BindingFlags.NonPublic
        let y = serialize x

        //output.WriteLine(Render.stringify y)
        Should.equal y """["Public","NonPublic"]"""

    [<Fact>]
    member this.``deserialize flags``() =
        let x = """["Public","NonPublic"]"""
        let y = deserialize<BindingFlags> x

        //output.WriteLine(Render.stringify y)
        Should.equal y (BindingFlags.Public ||| BindingFlags.NonPublic)

    [<Fact>]
    member this.``serialize zero flags``() =
        let x = RegexOptions.None
        let y = serialize x

        //output.WriteLine(Render.stringify y)
        Should.equal y """["None"]"""

    [<Fact>]
    member this.``deserialize zero flags``() =
        let x = """["None"]"""
        let y = deserialize<RegexOptions> x

        //output.WriteLine(Render.stringify y)
        Should.equal y RegexOptions.None

    [<Fact>]
    member this.``read enum``() =
        let x = DateTimeKind.Local
        let y = JSON.read x

        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.String "Local"
    [<Fact>]
    member this.``enum instantiate``() =
        let x = JsonValue.String "Local"
        let y = JSON.write<DateTimeKind> x

        //output.WriteLine(Render.stringify y)
        Should.equal y DateTimeKind.Local
         
    [<Fact>]
    member this.``read flags``() =
        let x = BindingFlags.Public ||| BindingFlags.NonPublic
        let y = JSON.read x
        //output.WriteLine(Render.stringify res)
        Should.equal y <| JsonValue.Array [JsonValue.String "Public"; JsonValue.String "NonPublic" ]

    [<Fact>]
    member this.``flags instantiate``() =
        let x = JsonValue.Array [JsonValue.String "Public"; JsonValue.String "NonPublic" ]
        let y = JSON.write<BindingFlags> x

        //output.WriteLine(Render.stringify y)
        Should.equal y (BindingFlags.Public ||| BindingFlags.NonPublic)

    [<Fact>]
    member this.``read zero flags``() =
        let x = RegexOptions.None
        let y = JSON.read x
        //output.WriteLine(Render.stringify res)
        Should.equal y <| JsonValue.Array [JsonValue.String "None"]

    [<Fact>]
    member this.``zero flags instantiate``() =
        let x = JsonValue.Array [JsonValue.String "None"]
        let y = JSON.write<RegexOptions> x

        //output.WriteLine(Render.stringify y)
        Should.equal y RegexOptions.None

