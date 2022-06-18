module UnquotedJson.Program
open FSharp.Literals
open System

let x = """{0:{index:0, license:t, nameSID:n, image:"img:left", descriptionSID:t, category:r}}"""

let [<EntryPoint>] main _ = 
    let y = JSON.parse x
    Console.WriteLine(Literal.stringify y)
    0
