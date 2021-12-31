namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type GuidTest(output: ITestOutputHelper) =
    [<Fact>]
    member _.``serialize``() =
        let x = Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")
        let y = serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "\"936da01f-9abd-4d9d-80c7-02af85c822a8\""

    [<Fact>]
    member _.``deserialize``() =
        let x = "\"936da01f-9abd-4d9d-80c7-02af85c822a8\""
        let y = deserialize<Guid> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")


    [<Fact>]
    member _.``read``() =
        let x = Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y 
        <| JsonValue.String "936da01f-9abd-4d9d-80c7-02af85c822a8"

    [<Fact>]
    member _.``instantiate``() =
        let x = JsonValue.String "936da01f-9abd-4d9d-80c7-02af85c822a8"
        let y = JSON.write<Guid> x

        //output.WriteLine(Render.stringify y)
        Should.equal y <| Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")


