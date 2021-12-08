namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type Person = { name : string; age : int }

type RecordTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize record``() =
        let x = { name = "abcdefg"; age = 18 }
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y """{"name":"abcdefg","age":18}"""

    [<Fact>]
    member this.``deserialize record``() =
        let x = """{"age":18,"name":"abcdefg"}"""
        let y = deserialize<Person> x
        //output.WriteLine(Render.stringify y)
        Should.equal y { name = "abcdefg"; age = 18 }
        

    [<Fact>]
    member this.``read record``() =
        let x = { name = "abcdefg"; age = 18 }
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.Object ["name",JsonValue.String "abcdefg"; "age", JsonValue.Number 18.0]

    [<Fact>]
    member this.``write record``() =
        let x = JsonValue.Object ["name",JsonValue.String "abcdefg"; "age", JsonValue.Number 18.0]
        let y = JSON.write<Person> x
        //output.WriteLine(Render.stringify y)
        Should.equal y { name = "abcdefg"; age = 18 }

    [<Fact>]
    member this.``field items test``() =
        let x = JsonValue.Object ["name",JsonValue.String "abcdefg"; "age", JsonValue.Number 18.0]
        let y = x.["name"]
        Should.equal y <| JsonValue.String "abcdefg"

        
