module UnquotedJson.Program
open FSharp.Literals
open System

let [<EntryPoint>] main _ = 
    let x = "[`xyz`,`123`]"
    let y = UrlQuery.parseFieldDynamic typeof<string[]> x
    Console.WriteLine(Literal.stringify y)
    0
