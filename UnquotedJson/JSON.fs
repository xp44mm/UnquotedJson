module UnquotedJson.JSON

open System
open FSharp.Idioms.Literal
open UnquotedJson.Converters

let parser = JsonCompiler.parser

let parse = JsonCompiler.parse

///
let stringifyUnquotedJson (json:JsonValue) = JsonRender.stringifyUnquotedJson json

let stringifyNormalJson (json:JsonValue) = JsonRender.stringifyNormalJson json


let readDynamic (ty:Type) (value:obj) =
    FSharpConverter.readObj FSharpConverter.tryReaders ty value

/// convert from value to json
let read<'t> (value:'t) = readDynamic typeof<'t> value

let writeDynamic (ty:Type) (json:JsonValue) =
    FSharpConverter.writeObj FSharpConverter.tryWriters ty json

/// convert from json to value
let write<'t> (json:JsonValue) =
    writeDynamic typeof<'t> json :?> 't

