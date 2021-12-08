namespace UnquotedJson.Converters

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open UnquotedJson

type FallbackWriterTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``null``() =
        let json = JsonValue.Null
        let y = JSON.write<_> json
        Should.equal y null

    [<Fact>]
    member this.``false``() =
        let json = JsonValue.False
        let y = JSON.write<_> json
        Should.equal y false

    [<Fact>]
    member this.``true``() =
        let json = JsonValue.True
        let y = JSON.write<_> json
        Should.equal y true

    [<Fact>]
    member this.``string``() =
        let json = JsonValue.String ""
        let y = JSON.write<string> json
        Should.equal y ""

    [<Fact>]
    member this.``char``() =
        let json = JsonValue.String "0"
        let y = JSON.write<char> json
        Should.equal y '0'

    [<Fact>]
    member this.``number sbyte``() =
        let json = JsonValue.Number 0.0
        let y = JSON.write<sbyte> json
        Should.equal y 0y

    [<Fact>]
    member this.``number byte``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0uy)

    [<Fact>]
    member this.``number int16``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0s)

    [<Fact>]
    member this.``number uint16``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0us)

    [<Fact>]
    member this.``number int``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0)

    [<Fact>]
    member this.``number uint32``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0u)

    [<Fact>]
    member this.``number int64``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0L)

    [<Fact>]
    member this.``number uint64``() =
        let x =JsonValue.Number 0.0 
        let y = JSON.write<_> x
        Assert.Equal(y,0UL)

    [<Fact>]
    member this.``number single``() =
        let x =JsonValue.Number 0.1
        let y = JSON.write<_> x 
        Assert.Equal(y, 0.1f)

    [<Fact>]
    member this.``number decimal``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0M)

    [<Fact>]
    member this.``number nativeint``() =
        let x = JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0n)

    [<Fact>]
    member this.``number unativeint``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0un)
