module UnquotedJson.Program
open FSharp.Idioms
open FSharp.Idioms.Jsons
open System

let x = """/*test*/{0:{index:0, license:t, nameSID:n, image:"img:left", descriptionSID:t, category:r,}}"""

let [<EntryPoint>] main _ = 
    let y = Json.parse x
    Console.WriteLine(Literal.stringify y)
    0
