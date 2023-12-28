module UnquotedJson.JSON

open System
open FSharp.Idioms.Literal

let parser = JsonCompiler.parser

let parse = JsonCompiler.parse

/////
//let stringifyUnquotedJson (json:Json) = JsonRender.stringifyUnquotedJson json

//let stringifyNormalJson (json:Json) = JsonRender.stringifyNormalJson json


//let readDynamic (ty:Type) (value:obj) =
//    FSharpConverter.readObj FSharpConverter.tryReaders ty value

///// convert from value to json
//let read<'t> (value:'t) = readDynamic typeof<'t> value

//let writeDynamic (ty:Type) (json:Json) =
//    FSharpConverter.writeObj FSharpConverter.tryWriters ty json

///// convert from json to value
//let write<'t> (json:Json) =
//    writeDynamic typeof<'t> json :?> 't

