module UnquotedJson.JsonTokenUtils

open System
open System.Text.RegularExpressions

open FSharp.Idioms
open FSharp.Idioms.RegularExpressions

let tokenize (inp:string) =
    let rec loop (pos:int) (inp:string) =
        seq {
            match inp with
            | "" -> ()
    
            | On(tryMatch(Regex @"^\s+")) (x,rest) -> 
                let pos = pos + x.Length
                yield! loop pos rest

            | On(tryFirst '{') rest ->
                yield pos,LBRACE
                yield! loop (pos+1) rest

            | On(tryFirst '}') rest ->
                yield pos,RBRACE
                yield! loop (pos+1) rest

            | On(tryFirst '[') rest ->
                yield pos,LBRACK
                yield! loop (pos+1) rest

            | On(tryFirst ']') rest ->
                yield pos,RBRACK
                yield! loop (pos+1) rest

            | On(tryFirst ',') rest ->
                yield pos,COMMA
                yield! loop (pos+1) rest

            | On(tryFirst ':') rest ->
                yield pos,COLON
                yield! loop (pos+1) rest

            | On(tryMatch(Regex @"^""(\\[^\u0000-\u001F\u007F]|[^\\""\u0000-\u001F\u007F])*""")) (lexeme,rest) ->
                yield pos,QUOTED(Quotation.unquote lexeme)
                let pos = pos + lexeme.Length
                yield! loop pos rest

            | On(tryMatch(Regex @"^[^,:{}[\]""]+(?<=\S)")) (lexeme,rest) ->
                yield pos,UNQUOTED lexeme
                let pos = pos + lexeme.Length
                yield! loop pos rest

            | _ -> failwithf "tokenizeWithPos:%A" (pos,inp)
        }
    
    loop 0 inp

let getTag(pos,token) = 
    match token with
    | COMMA       -> ","
    | COLON       -> ":"
    | LBRACK      -> "["
    | RBRACK      -> "]"
    | LBRACE      -> "{"
    | RBRACE      -> "}"
    | QUOTED   _  -> "QUOTED"
    | UNQUOTED _  -> "UNQUOTED"
    | WS _ -> "WS"

let getLexeme(pos,token) = 
    match token with
    | QUOTED x -> box x
    | UNQUOTED x -> box x
    | _ -> null

/// get value from unquoted
let fromUnquoted str = 
    //if str = "null" then
    //    JsonValue.Null
    //elif str = "false" then
    //    JsonValue.False
    //elif str = "true" then
    //    JsonValue.True
    //elif Regex.IsMatch(str, @"^[-+]?\d+(\.\d+)?([eE][-+]?\d+)?$") then
    //    JsonValue.Number(Double.Parse str)
    //else
    //    JsonValue.String str
    match str with
    | "null" -> JsonValue.Null
    | "true" -> JsonValue.True
    | "false" -> JsonValue.False
    | Search(Regex(@"^[-+]?\d+(\.\d+)?([eE][-+]?\d+)?$")) _ ->
        JsonValue.Number(Double.Parse str)
    | _ -> JsonValue.String str




























