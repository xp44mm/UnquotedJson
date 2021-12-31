namespace UnquotedJson.Converters

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open UnquotedJson

type FallbackWriterTest(output: ITestOutputHelper) =

    [<Fact>]
    member _.``null``() =
        let json = JsonValue.Null
        let y = JSON.write<_> json
        Should.equal y null

    [<Fact>]
    member _.``false``() =
        let json = JsonValue.False
        let y = JSON.write<_> json
        Should.equal y false

    [<Fact>]
    member _.``true``() =
        let json = JsonValue.True
        let y = JSON.write<_> json
        Should.equal y true

    [<Fact>]
    member _.``string``() =
        let json = JsonValue.String ""
        let y = JSON.write<string> json
        Should.equal y ""

    [<Fact>]
    member _.``char``() =
        let json = JsonValue.String "0"
        let y = JSON.write<char> json
        Should.equal y '0'

    [<Fact>]
    member _.``number sbyte``() =
        let json = JsonValue.Number 0.0
        let y = JSON.write<sbyte> json
        Should.equal y 0y

    [<Fact>]
    member _.``number byte``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0uy)

    [<Fact>]
    member _.``number int16``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0s)

    [<Fact>]
    member _.``number uint16``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0us)

    [<Fact>]
    member _.``number int``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0)

    [<Fact>]
    member _.``number uint32``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0u)

    [<Fact>]
    member _.``number int64``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0L)

    [<Fact>]
    member _.``number uint64``() =
        let x =JsonValue.Number 0.0 
        let y = JSON.write<_> x
        Assert.Equal(y,0UL)

    [<Fact>]
    member _.``number single``() =
        let x =JsonValue.Number 0.1
        let y = JSON.write<_> x 
        Assert.Equal(y, 0.1f)

    [<Fact>]
    member _.``number decimal``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0M)

    [<Fact>]
    member _.``number nativeint``() =
        let x = JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0n)

    [<Fact>]
    member _.``number unativeint``() =
        let x =JsonValue.Number 0.0
        let y = JSON.write<_> x
        Assert.Equal(y, 0un)
