namespace UnquotedJson.Converters
open UnquotedJson
open UnquotedJson.Converters.Serialization

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type MutableVector2D() =
    let mutable currDX = 0.0
    let mutable currDY = 0.0
    member vec.DX with get() = currDX and set v = currDX <- v
    member vec.DY with get() = currDY and set v = currDY <- v
    member vec.Length
        with get () = sqrt (currDX * currDX + currDY * currDY)

    member vec.Angle
        with get () = atan2 currDY currDX

type ClassTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``JSON read``() =
        let x = MutableVector2D()
        let y = JSON.read x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| JsonValue.Object ["DX",JsonValue.Number 0.0;"DY",JsonValue.Number 0.0]

    [<Fact>]
    member this.``JSON write``() =
        let x = JsonValue.Object ["DX",JsonValue.Number 0.0;"DY",JsonValue.Number 0.0]
        let y = JSON.write<MutableVector2D> x
        //output.WriteLine(Render.stringify y)
        let z = MutableVector2D()
        Should.equal (y.DX,y.DY) (z.DX,z.DY)

    [<Fact>]
    member this.``is class``() =
        Should.equal typeof<bool>.IsClass       false
        Should.equal typeof<sbyte>.IsClass      false
        Should.equal typeof<byte>.IsClass       false
        Should.equal typeof<int16>.IsClass      false
        Should.equal typeof<uint16>.IsClass     false
        Should.equal typeof<int>.IsClass        false
        Should.equal typeof<uint32>.IsClass     false
        Should.equal typeof<int64>.IsClass      false
        Should.equal typeof<uint64>.IsClass     false
        Should.equal typeof<single>.IsClass     false
        Should.equal typeof<float>.IsClass      false
        Should.equal typeof<char>.IsClass       false
        Should.equal typeof<decimal>.IsClass    false
        Should.equal typeof<nativeint>.IsClass  false
        Should.equal typeof<unativeint>.IsClass false

        Should.equal typeof<string>.IsClass     true

