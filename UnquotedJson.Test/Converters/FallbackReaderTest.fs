namespace UnquotedJson.Converters

open Xunit
open Xunit.Abstractions
open System
open FSharp.xUnit
open UnquotedJson

type FallbackReaderTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``covert from sbyte test``() =
        let x = 0y
        let y = JSON.read x
        Assert.Equal(y, JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from byte test``() =
        let x = 0uy
        let y = JSON.read x
        Assert.Equal(y,JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from int16 test``() =
        let x = 0s
        let y = JSON.read x
        Assert.Equal(y,JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from uint16 test``() =
        let x = 0us
        let y = JSON.read x
        Assert.Equal(y,JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from int test``() =
        let x = 0
        let y = JSON.read x
        Assert.Equal(y,JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from uint32 test``() =
        let x = 0u
        let y = JSON.read x
        Assert.Equal(y,JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from int64 test``() =
        let x = 0L
        let y = JSON.read x
        Assert.Equal(y,JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from uint64 test``() =
        let x = 0UL
        let y = JSON.read x
        Assert.Equal(y,JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from single test``() =
        let x = 0.1f
        let y = JSON.read x 
        Assert.Equal(y,JsonValue.Number 0.1)

    [<Fact>]
    member this.``covert from decimal test``() =
        let x = 0M
        let y = JSON.read x
        Assert.Equal(y,JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from nativeint test``() =
        let x = 0n
        let y = JSON.read x
        Assert.Equal(y, JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from unativeint test``() =
        let x = 0un
        let y = JSON.read x
        Assert.Equal(y,JsonValue.Number 0.0)

    [<Fact>]
    member this.``covert from nullable test``() =
        let x0 = Nullable()
        let y0 = JSON.read x0
        Assert.Equal(y0,JsonValue.Null)

        let x = Nullable(3)
        let y = JSON.read x
        Assert.Equal(y,JsonValue.Number 3.0)

    [<Fact>]
    member this.``covert from null test``() =
        let ls = null
        let res = JSON.read ls
        Should.equal res JsonValue.Null

    [<Fact>]
    member this.``covert from char test``() =
        let x = '\t'
        let y = JSON.read x
        Assert.Equal(y, JsonValue.String "\t")

    [<Fact>]
    member this.``covert from string test``() =
        let x = ""
        let y = JSON.read x
        Assert.Equal(y, JsonValue.String "")
