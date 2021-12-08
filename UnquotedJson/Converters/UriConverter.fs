module UnquotedJson.Converters.UriConverter
open UnquotedJson

open System

let tryRead (ty: Type) (value: obj) =
    if ty = typeof<Uri> then
        Some(fun loopRead -> 
            (unbox<Uri> value).ToString() |> JsonValue.String
        )
    else
        None

let tryWrite (ty: Type) (json: JsonValue) =
    if ty = typeof<Uri> then
        Some(fun loopWrite -> 
            match json with
            | JsonValue.String uri -> Uri(uri)
            | x -> failwithf "%A" x
            |> box
        )
    else
        None


