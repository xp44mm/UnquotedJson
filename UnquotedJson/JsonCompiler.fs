module UnquotedJson.JsonCompiler

open FslexFsyacc
open System
open FSharp.Idioms.Literal

open UnquotedJson.JsonParseTable

let parser = app.getParser<Position<JsonToken>>(JsonTokenUtils.getTag,JsonTokenUtils.getLexeme)
let table = app.getTable parser

let parseTokens(tokens:seq<Position<JsonToken>>) =
    tokens
    |> parser.parse
    |> JsonParseTable.unboxRoot

///
let parse(text:string) =
    if String.IsNullOrWhiteSpace text then
        failwith "empty string is illeagal json string."
    let mutable tokens = []
    let mutable states = [(0,null)]

    let stringifyStates() =
        let symbols =
            states
            |> List.map(fun(i,_)-> $"{i}/{table.kernelSymbols.[i]}")
        stringify symbols

    text
    |> JsonTokenUtils.tokenize 0
    |> Seq.map(fun tok -> // trace
        tokens <- tok :: tokens
        tok
    )
    |> Seq.filter(fun tok ->
        match tok.value with
        | WS _ -> false
        | _ ->true
        )
    |> Seq.map(fun lookahead ->
        match parser.tryReduce(states,lookahead) with
        | Some reducedstates -> states <- reducedstates
        | None -> ()
        lookahead
    )
    |> Seq.iter(fun tok ->
        states <- parser.shift(states,tok)
    )

    match parser.tryReduce(states) with
    | Some reducedstates -> states <- reducedstates
    | None -> ()

    match states with
    |[(1,lxm);(0,null)] ->
        lxm
        |> JsonParseTable.unboxRoot
    | _ ->
        failwith $"states:{stringifyStates()}"

