module UnquotedJson.Json

open FSharp.Idioms.Jsons

let parser = JsonCompiler.parser

let parse:string->Json = JsonCompiler.parse
