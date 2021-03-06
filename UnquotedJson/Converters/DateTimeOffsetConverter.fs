module UnquotedJson.Converters.DateTimeOffsetConverter
open UnquotedJson

open System
open FSharp.Literals

let tryRead (ty:Type) (value:obj) = 
    if ty = typeof<DateTimeOffset> then
        Some(fun loopRead -> 
            value.ToString()
            |> JsonValue.String)
    else None

let tryWrite (ty:Type) (json:JsonValue) = 
    if ty = typeof<DateTimeOffset> then
        Some(fun loopWrite -> 
            match json with
            | JsonValue.String s -> box <| DateTimeOffset.Parse(s)
            | _ -> failwith (Render.stringify json))
    else None
