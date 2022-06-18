module UnquotedJson.JsonTokenizerUtils

open System
open System.Text.RegularExpressions

open FSharp.Idioms
open FslexFsyacc.Runtime

let tokenize (inp:string) =
    let mutable pos = 0
    let rec loop 
        //(pos:int) 
        (inp:string) =
        seq {
            match inp with
            | "" -> ()
    
            | On(tryMatch(Regex @"^\s+")) (x,rest) ->
                yield WS x
                //let pos = pos + x.Length
                yield! loop (*pos*) rest

            | On(tryFirst '{') rest ->
                yield (*pos,*)LBRACE
                yield! loop (*(pos+1)*) rest

            | On(tryFirst '}') rest ->
                yield (*pos,*)RBRACE
                yield! loop (*(pos+1)*) rest

            | On(tryFirst '[') rest ->
                yield (*pos,*)LBRACK
                yield! loop (*(pos+1)*) rest

            | On(tryFirst ']') rest ->
                yield (*pos,*)RBRACK
                yield! loop (*(pos+1)*) rest

            | On(tryFirst ',') rest ->
                yield (*pos,*)COMMA
                yield! loop (*(pos+1)*) rest

            | On(tryFirst ':') rest ->
                yield (*pos,*)COLON
                yield! loop (*(pos+1)*) rest

            | On(tryMatch(Regex @"^""(\\[^\u0000-\u001F\u007F]|[^\\""\u0000-\u001F\u007F])*""")) (lexeme,rest) ->
                yield (*pos,*)QUOTED(Quotation.unquote lexeme)
                //let pos = pos + lexeme.Length
                yield! loop (*pos*) rest

            | On(tryMatch(Regex @"^[^,:{}[\]""]+(?<=\S)")) (lexeme,rest) ->
                yield (*pos,*)UNQUOTED lexeme
                //let pos = pos + lexeme.Length
                yield! loop (*pos*) rest

            | _ -> failwithf "tokenizeWithPos:%A" ((*pos,*)inp)
        }
    
    loop (*0*) inp
    |> Seq.map(fun tok ->
        let oldPos = pos
        pos <- pos + tok.raw.Length
        {
            index = oldPos
            length = tok.raw.Length
            value = tok
        }
    )
