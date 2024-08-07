﻿module UnquotedJson.JsonTokenUtils

open System
open System.Text.RegularExpressions
open FSharp.Idioms

open FSharp.Idioms.Jsons
open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.RegularExpressions

open FslexFsyacc

let tokenize (pos:int) (inp:string) =
    let rec loop (i:int) =
        seq {
            match inp.[i-pos..] with
            | "" -> ()
    
            | First '{' capt ->

                let postok = {
                    index = i
                    length = 1
                    value = LBRACE
                }
                yield postok
                yield! loop postok.adjacent

            | First '}' _ ->
                let postok = {
                    index = i
                    length = 1
                    value = RBRACE
                }
                yield postok
                yield! loop postok.adjacent

            | First '[' _ ->
                let postok = {
                    index = i
                    length = 1
                    value = LBRACK
                }
                yield postok
                yield! loop postok.adjacent

            | First ']' _ ->
                let postok = {
                    index = i
                    length = 1
                    value = RBRACK
                }
                yield postok
                yield! loop postok.adjacent

            | First ',' _ ->
                let postok = {
                    index = i
                    length = 1
                    value = COMMA
                }
                yield postok
                yield! loop postok.adjacent

            | First ':' _ ->
                let postok = {
                    index = i
                    length = 1
                    value = COLON
                }
                yield postok
                yield! loop postok.adjacent

            | Rgx @"^\s+" x -> 
                let pos = i + x.Length
                yield! loop pos

            | Rgx @"^//[^\r\n]*" x ->
                let pos = i + x.Length
                yield! loop pos

            | Rgx @"^/\*(?!\s*\))[\s\S]*?\*/" x ->
                let pos = i + x.Length
                yield! loop pos

            | Rgx @"^""(\\[^\u0000-\u001F\u007F]|[^\\""\u0000-\u001F\u007F])*""" lexeme ->
                let postok = {
                    index = i
                    length = lexeme.Length
                    value = QUOTED(Json.unquote lexeme.Value)
                }
                yield postok
                yield! loop postok.adjacent

            | Rgx @"^[^,:{}[\]""]+(?<=\S)" lexeme ->

                let postok = {
                    index = i
                    length = lexeme.Length
                    value = UNQUOTED lexeme.Value
                }
                yield postok
                yield! loop postok.adjacent

            | rest -> failwith $"tokenize:{rest}"
        }
    
    loop pos

let getTag (postok:PositionWith<JsonToken>) = 
    match postok.value with
    | COMMA      -> ","
    | COLON      -> ":"
    | LBRACK     -> "["
    | RBRACK     -> "]"
    | LBRACE     -> "{"
    | RBRACE     -> "}"
    | QUOTED   _ -> "QUOTED"
    | UNQUOTED _ -> "UNQUOTED"
    | WS       _ -> "WS"
    | COMMENT  _ -> "COMMENT"

let getLexeme (postok:PositionWith<JsonToken>) = 
    match postok.value with
    | QUOTED   raw
    | UNQUOTED raw
    | WS       raw
    | COMMENT  raw -> box raw
    | _ -> null

/// get value from unquoted
let fromUnquoted str = 
    match str with
    | "null" -> Json.Null
    | "true" -> Json.True
    | "false" -> Json.False
    | Search(Regex(@"^[-+]?\d+(\.\d+)?([eE][-+]?\d+)?$")) _ ->
        Json.Number(Double.Parse str)
    | _ -> Json.String str




























