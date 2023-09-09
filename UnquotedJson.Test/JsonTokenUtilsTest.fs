namespace UnquotedJson

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open UnquotedJson

type JsonTokenUtilsTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member _.``empty object``() =
        let x = "{}"
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList
        show y
        Should.equal y [{index= 0;length= 1;value= LBRACE};{index= 1;length= 1;value= RBRACE}]

    [<Fact>]
    member _.``empty array``() =
        let x = "[]"
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 0;length= 1;value= LBRACK};{index= 1;length= 1;value= RBRACK}]

    [<Fact>]
    member _.``null``() =
        let x = "null"
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 0;length= 4;value= UNQUOTED "null"}]

    [<Fact>]
    member _.``false``() =
        let x = "false"
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 0;length= 5;value= UNQUOTED "false"}]

    [<Fact>]
    member _.``true``() =
        let x = "true"
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 0;length= 4;value= UNQUOTED "true"}]

    [<Fact>]
    member _.``empty string``() =
        let x = "\"\""
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 0;length= 2;value= QUOTED ""}]

    [<Fact>]
    member _.``number``() =
        let x = "0"
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 0;length= 1;value= UNQUOTED "0"}]

    [<Fact>]
    member _.``single field object``() =
        let x = """{"a":0}"""
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 0;length= 1;value= LBRACE};{index= 1;length= 3;value= QUOTED "a"};{index= 4;length= 1;value= COLON};{index= 5;length= 1;value= UNQUOTED "0"};{index= 6;length= 1;value= RBRACE}]

    [<Fact>]
    member _.``many fields object``() =
        let x = """{"a":0,"b":null}"""
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 0;length= 1;value= LBRACE};{index= 1;length= 3;value= QUOTED "a"};{index= 4;length= 1;value= COLON};{index= 5;length= 1;value= UNQUOTED "0"};{index= 6;length= 1;value= COMMA};{index= 7;length= 3;value= QUOTED "b"};{index= 10;length= 1;value= COLON};{index= 11;length= 4;value= UNQUOTED "null"};{index= 15;length= 1;value= RBRACE}]

    [<Fact>]
    member _.``singleton array``() =
        let x = "[0]"
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 0;length= 1;value= LBRACK};{index= 1;length= 1;value= UNQUOTED "0"};{index= 2;length= 1;value= RBRACK}]

    [<Fact>]
    member _.``many elements array``() =
        let x = "[0,1]"
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 0;length= 1;value= LBRACK};{index= 1;length= 1;value= UNQUOTED "0"};{index= 2;length= 1;value= COMMA};{index= 3;length= 1;value= UNQUOTED "1"};{index= 4;length= 1;value= RBRACK}]

    [<Fact>]
    member _.``unquoted json``() =
        let x = """
        {0:{index:0, license:t, nameSID:n, image:"img:left", descriptionSID:t, category:r}}
        """
        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y
        Should.equal y [{index= 10;length= 1;value= LBRACE};{index= 11;length= 1;value= UNQUOTED "0"};{index= 12;length= 1;value= COLON};{index= 13;length= 1;value= LBRACE};{index= 14;length= 5;value= UNQUOTED "index"};{index= 19;length= 1;value= COLON};{index= 20;length= 1;value= UNQUOTED "0"};{index= 21;length= 1;value= COMMA};{index= 23;length= 7;value= UNQUOTED "license"};{index= 30;length= 1;value= COLON};{index= 31;length= 1;value= UNQUOTED "t"};{index= 32;length= 1;value= COMMA};{index= 34;length= 7;value= UNQUOTED "nameSID"};{index= 41;length= 1;value= COLON};{index= 42;length= 1;value= UNQUOTED "n"};{index= 43;length= 1;value= COMMA};{index= 45;length= 5;value= UNQUOTED "image"};{index= 50;length= 1;value= COLON};{index= 51;length= 10;value= QUOTED "img:left"};{index= 61;length= 1;value= COMMA};{index= 63;length= 14;value= UNQUOTED "descriptionSID"};{index= 77;length= 1;value= COLON};{index= 78;length= 1;value= UNQUOTED "t"};{index= 79;length= 1;value= COMMA};{index= 81;length= 8;value= UNQUOTED "category"};{index= 89;length= 1;value= COLON};{index= 90;length= 1;value= UNQUOTED "r"};{index= 91;length= 1;value= RBRACE};{index= 92;length= 1;value= RBRACE}]
    
    [<Fact>]
    member _.``parse normal json``() =
        let x = """{"0":{"index":0,"license":"t","nameSID":"n","image":"img:left","descriptionSID":"t","category":"r"}}"""

        let y = 
            JsonTokenUtils.tokenize 0 x
            |> Seq.toList

        show y

        Should.equal y [{index= 0;length= 1;value= LBRACE};{index= 1;length= 3;value= QUOTED "0"};{index= 4;length= 1;value= COLON};{index= 5;length= 1;value= LBRACE};{index= 6;length= 7;value= QUOTED "index"};{index= 13;length= 1;value= COLON};{index= 14;length= 1;value= UNQUOTED "0"};{index= 15;length= 1;value= COMMA};{index= 16;length= 9;value= QUOTED "license"};{index= 25;length= 1;value= COLON};{index= 26;length= 3;value= QUOTED "t"};{index= 29;length= 1;value= COMMA};{index= 30;length= 9;value= QUOTED "nameSID"};{index= 39;length= 1;value= COLON};{index= 40;length= 3;value= QUOTED "n"};{index= 43;length= 1;value= COMMA};{index= 44;length= 7;value= QUOTED "image"};{index= 51;length= 1;value= COLON};{index= 52;length= 10;value= QUOTED "img:left"};{index= 62;length= 1;value= COMMA};{index= 63;length= 16;value= QUOTED "descriptionSID"};{index= 79;length= 1;value= COLON};{index= 80;length= 3;value= QUOTED "t"};{index= 83;length= 1;value= COMMA};{index= 84;length= 10;value= QUOTED "category"};{index= 94;length= 1;value= COLON};{index= 95;length= 3;value= QUOTED "r"};{index= 98;length= 1;value= RBRACE};{index= 99;length= 1;value= RBRACE}]
