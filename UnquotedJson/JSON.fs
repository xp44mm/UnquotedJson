module UnquotedJson.JSON
open UnquotedJson

open System
open FSharp.Literals.Literal

open FslexFsyacc.Runtime


let parser = 
    Parser<int*JsonToken>(
        JsonParseTable.rules,
        JsonParseTable.actions,
        JsonParseTable.closures,JsonTokenUtils.getTag,JsonTokenUtils.getLexeme)

//let unboxRoot = unbox<JsonValue>

let parseTokens(tokens:seq<int*JsonToken>) =
    tokens
    |> parser.parse
    |> JsonParseTable.unboxRoot


///
let parse(text:string) = 
    if String.IsNullOrWhiteSpace text then
        failwith "empty string is illeagal json string."
    let mutable states = [0,null]

    let stringifyStates() =
        let symbols = 
            states
            |> List.map(fun(i,_)-> $"{i}/{parser.getSymbol i}")
        stringify symbols

    text
    //|> JsonTokenUtils.tokenize
    |> JsonTokenizerUtils.tokenize
    |> Seq.filter(fun p -> 
        match p.value with
        | WS _ -> false
        | _ ->true
        )
    |> Seq.map(fun p -> p.index,p.value)
    //|> JsonParseTable.parse
    |> Seq.iteri(fun i tok ->
        let nextStates = parser.shift(states,tok)
        states <- nextStates
        Console.WriteLine(stringifyStates())
    )

    match parser.tryReduce(states) with
    | Some nextStates ->
        states <- nextStates
        Console.WriteLine(stringifyStates())
    | None -> ()

    match states with
    |[(1,lxm);0,null] -> lxm |> JsonParseTable.unboxRoot
    | _ -> failwith $"states:{stringifyStates()}\r\ntok:EOF"

///
let stringifyUnquotedJson (json:JsonValue) = JsonRender.stringifyUnquotedJson json

let stringifyNormalJson (json:JsonValue) = JsonRender.stringifyNormalJson json

open UnquotedJson.Converters

let readDynamic (ty:Type) (value:obj) = 
    FSharpConverter.readObj FSharpConverter.tryReaders ty value

/// convert from value to json
let read<'t> (value:'t) = readDynamic typeof<'t> value

let writeDynamic (ty:Type) (json:JsonValue) = 
    FSharpConverter.writeObj FSharpConverter.tryWriters ty json

/// convert from json to value
let write<'t> (json:JsonValue) = 
    writeDynamic typeof<'t> json :?> 't

