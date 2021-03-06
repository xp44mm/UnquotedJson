namespace UnquotedJson

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open UnquotedJson

type JsonTokenizerUtilsTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``empty object``() =
        let x = "{}"
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList
        show y
        //Should.equal y 
        //<| JsonValue.Object []

    [<Fact>]
    member _.``empty array``() =
        let x = "[]"
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.Array []

    [<Fact>]
    member _.``null``() =
        let x = "null"
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.Null

    [<Fact>]
    member _.``false``() =
        let x = "false"
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.False

    [<Fact>]
    member _.``true``() =
        let x = "true"
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.True

    [<Fact>]
    member _.``empty string``() =
        let x = String.replicate 2 "\""
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.String ""

    [<Fact>]
    member _.``number``() =
        let x = "0"
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.Number 0.0

    [<Fact>]
    member _.``single field object``() =
        let x = """{"a":0}"""
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.Object["a",JsonValue.Number 0.0]

    [<Fact>]
    member _.``many fields object``() =
        let x = """{"a":0,"b":null}"""
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.Object ["a",JsonValue.Number 0.0;"b",JsonValue.Null;]

    [<Fact>]
    member _.``singleton array``() =
        let x = "[0]"
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.Array [JsonValue.Number 0.0]

    [<Fact>]
    member _.``many elements array``() =
        let x = "[0,1]"
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.Array [JsonValue.Number 0.0;JsonValue.Number 1.0]

    [<Fact>]
    member _.``unquoted json``() =
        let x = """
        {0:{index:0, license:t, nameSID:n, image:"img:left", descriptionSID:t, category:r}}
        """
        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y
        //Should.equal y 
        //<| JsonValue.Object ["0",JsonValue.Object ["index",JsonValue.Number 0.0;"license",JsonValue.String "t";"nameSID",JsonValue.String "n";"image",JsonValue.String "img:left";"descriptionSID",JsonValue.String "t";"category",JsonValue.String "r"]]
    
    [<Fact>]
    member _.``convert unquoted json``() =
        let x = """{0:{index:0, license:t, nameSID:n, image:"img:left", descriptionSID:t, category:r}}"""
        let n = """{"0":{"index":0,"license":"t","nameSID":"n","image":"img:left","descriptionSID":"t","category":"r"}}"""

        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList

        show y

    [<Fact>]
    member _.``parse normal json``() =
        let x = """{"0":{"index":0,"license":"t","nameSID":"n","image":"img:left","descriptionSID":"t","category":"r"}}"""

        let y = 
            JsonTokenizerUtils.tokenize x
            |> Seq.toList


        show y

