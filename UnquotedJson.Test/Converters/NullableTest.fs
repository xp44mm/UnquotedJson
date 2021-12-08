namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit


type NullableTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize nullable``() =
        let x = Nullable 3
        let y = serialize x
        Should.equal y "3"

    [<Fact>]
    member this.``serialize nullable null``() =
        let x = Nullable ()
        let y = serialize x
        Should.equal y "null"

    [<Fact>]
    member this.``deserialize nullable``() =
        let x = "3" 
        let y = deserialize<Nullable<int>> x
        Should.equal y <| Nullable 3

    [<Fact>]
    member this.``deserialize nullable null``() =
        let x = "null"
        let y = deserialize<Nullable<_>> x
        Should.equal y <| Nullable ()

    [<Fact>]
    member this.``read nullable``() =
        let x = Nullable 3
        let y = JSON.read x
        Should.equal y <| JsonValue.Number 3.0

    [<Fact>]
    member this.``read nullable null``() =
        let x = Nullable()
        let y = JSON.read x
        Should.equal y <| JsonValue.Null


    [<Fact>]
    member this.``write nullable``() =
        let x = JsonValue.Number 3.0
        let y = JSON.write<Nullable<int>> x

        Should.equal y <| Nullable 3

    [<Fact>]
    member this.``write nullable null``() =
        let x = JsonValue.Null
        let y = JSON.write<Nullable<_>> x

        Should.equal y <| Nullable ()

    [<Fact>]
    member this.``nullable equality``() =

        Assert.True(Nullable()=Nullable())
        Assert.True(Nullable 3=Nullable 3)

