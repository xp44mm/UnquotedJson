module UnquotedJson.JSON

open System

///
let parse(text:string) = 
    if System.String.IsNullOrWhiteSpace text then
        failwith "empty string is illeagal json string."
    else
        text
        |> JsonTokenUtils.tokenize
        |> JsonParseTable.parse

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
let write<'t> (json:JsonValue) = writeDynamic typeof<'t> json :?> 't

