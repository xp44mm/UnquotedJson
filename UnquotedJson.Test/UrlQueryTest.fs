namespace UnquotedJson

open Xunit
open Xunit.Abstractions

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.xUnit

type UrlQueryTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    [<Fact>]
    member this.``control char test``() =
        let x = @"[""\u0002""]"
        let y = UrlQuery.parseField<string[]> x

        //show y
        Should.equal y [|"\u0002"|]
        
    [<Fact>]
    member this.``parseField test``() =
        let x = "[xyz,\"123\"]"
        let y = UrlQuery.parseField<string[]> x
        let z = [|"xyz";"123"|]
        Should.equal y z
        
    [<Fact>]
    member this.``parseField dynamic test``() =
        let x = "[xyz,\"123\"]"
        let y = UrlQuery.parseFieldDynamic typeof<string[]> x
        let z = [|"xyz";"123"|] |> box
        Should.equal y z

    [<Fact>]
    member this.``parse test``() =
        let x = ["foo","bar";"abc","[xyz,\"123\"]"]
        let y = UrlQuery.parseQueryDynamic typeof<{|foo:string;abc:string[]|}> x
        let z = box {|abc=[|"xyz";"123"|];foo="bar"|}
        Should.equal y z
        
