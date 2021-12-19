module UnquotedJson.JsonTokenUtils

open System.Text.RegularExpressions
open FSharp.Idioms.StringOps

let tokenize(inp:string) =
    let rec loop (inp:string) =
        seq {
            match inp with
            | "" -> ()
    
            | Prefix @"\s+" (_,rest) -> 
                yield! loop rest

            | PrefixChar '{' rest ->
                yield LBRACE
                yield! loop rest

            | PrefixChar '}' rest ->
                yield RBRACE
                yield! loop rest

            | PrefixChar '[' rest ->
                yield LBRACK
                yield! loop rest

            | PrefixChar ']' rest ->
                yield RBRACK
                yield! loop rest

            | PrefixChar ',' rest ->
                yield COMMA
                yield! loop rest

            | PrefixChar ':' rest ->
                yield COLON
                yield! loop rest

            | Prefix """(?:"(\\[\\"bfnrt]|\\u[0-9A-Fa-f]{4}|[^\\"\r\n])*")""" (lexeme,rest) ->
                yield  QUOTED(unquote lexeme)
                yield! loop rest

            | Prefix @"[^,:{}[\]""]+(?<=\S)" (lexeme,rest) ->
                yield  UNQUOTED lexeme
                yield! loop rest

            | _ -> failwith "never"
        }
    
    loop inp

let tokenizeWithPos (inp:string) =
    let rec loop (pos:int) (inp:string) =
        seq {
            match inp with
            | "" -> ()
    
            | Prefix @"\s+" (x,rest) -> 
                let pos = pos + x.Length
                yield! loop pos rest

            | PrefixChar '{' rest ->
                yield pos,LBRACE
                yield! loop (pos+1) rest

            | PrefixChar '}' rest ->
                yield pos,RBRACE
                yield! loop (pos+1) rest

            | PrefixChar '[' rest ->
                yield pos,LBRACK
                yield! loop (pos+1) rest

            | PrefixChar ']' rest ->
                yield pos,RBRACK
                yield! loop (pos+1) rest

            | PrefixChar ',' rest ->
                yield pos,COMMA
                yield! loop (pos+1) rest

            | PrefixChar ':' rest ->
                yield pos,COLON
                yield! loop (pos+1) rest

            | Prefix """(?:"(\\[\\"bfnrt]|\\u[0-9A-Fa-f]{4}|[^\\"\r\n])*")""" (lexeme,rest) ->
                yield pos,QUOTED(unquote lexeme)
                let pos = pos + lexeme.Length
                yield! loop pos rest

            | Prefix @"[^,:{}[\]""]+(?<=\S)" (lexeme,rest) ->
                yield pos,UNQUOTED lexeme
                let pos = pos + lexeme.Length
                yield! loop pos rest

            | _ -> failwith "never"
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

let getLexeme(pos,token) = 
    match token with
    | QUOTED x -> box x
    | UNQUOTED x -> box x
    | _ -> null

// get value from unquoted
let fromUnquoted str = 
    if str = "null" then
        JsonValue.Null
    elif str = "false" then
        JsonValue.False
    elif str = "true" then
        JsonValue.True
    elif Regex.IsMatch(str, @"[-+]?\d+(\.\d+)?([eE][-+]?\d+)?") then
        JsonValue.Number(System.Double.Parse str)
    else
        JsonValue.String str































