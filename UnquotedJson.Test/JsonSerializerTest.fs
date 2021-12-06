namespace UnquotedJson

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open UnquotedJson

type JsonSerializerTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member this.``empty object``() =
        let x = "{}"
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.Object []

    [<Fact>]
    member this.``empty array``() =
        let x = "[]"
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.Array []

    [<Fact>]
    member this.``null``() =
        let x = "null"
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.Null

    [<Fact>]
    member this.``false``() =
        let x = "false"
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.False

    [<Fact>]
    member this.``true``() =
        let x = "true"
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.True

    [<Fact>]
    member this.``empty string``() =
        let x = String.replicate 2 "\""
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.String ""

    [<Fact>]
    member this.``number``() =
        let x = "0"
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.Number 0.0

    [<Fact>]
    member this.``single field object``() =
        let x = """{"a":0}"""
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.Object["a",JsonValue.Number 0.0]

    [<Fact>]
    member this.``many fields object``() =
        let x = """{"a":0,"b":null}"""
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.Object ["a",JsonValue.Number 0.0;"b",JsonValue.Null;]

    [<Fact>]
    member this.``singleton array``() =
        let x = "[0]"
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.Array [JsonValue.Number 0.0]

    [<Fact>]
    member this.``many elements array``() =
        let x = "[0,1]"
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.Array [JsonValue.Number 0.0;JsonValue.Number 1.0]

    [<Fact>]
    member this.``unquoted json``() =
        let x = """
        {0:{index:0, license:t, nameSID:n, image:"img:left", descriptionSID:t, category:r}}
        """
        let y = JSON.parse x
        //show y
        Should.equal y 
        <| JsonValue.Object ["basic",JsonValue.Object ["0",JsonValue.Object ["index",JsonValue.Number 0.0;"license",JsonValue.String "t";"nameSID",JsonValue.String "n";"image",JsonValue.String "img_left";"descriptionSID",JsonValue.String "t";"category",JsonValue.String "r"]]]
        
    [<Fact>]
    member this.``convert unquoted json``() =
        let x = """{0:{index:0, license:t, nameSID:n, image:"img:left", descriptionSID:t, category:r}}"""
        let n = """{"0":{"index":0,"license":"t","nameSID":"n","image":"img:left","descriptionSID":"t","category":"r"}}"""

        let y = 
            x
            |> JSON.parse
            |> JSON.stringifyNormalJson

        show y

    [<Fact>]
    member this.``parse normal json``() =
        let n = """{"0":{"index":0,"license":"t","nameSID":"n","image":"img:left","descriptionSID":"t","category":"r"}}"""

        let y = 
            n
            |> JSON.parse
            |> JSON.stringifyUnquotedJson

        show y

