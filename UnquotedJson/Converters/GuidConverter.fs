module UnquotedJson.Converters.GuidConverter
open UnquotedJson

open System
open FSharp.Literals

let tryRead(ty:Type) (value:obj) = 
    if ty = typeof<Guid> then
        Some(fun loopRead -> 
            value.ToString() |> JsonValue.String
        )
    else None

let tryWrite(ty:Type) (json:JsonValue) = 
    if ty = typeof<Guid> then
        Some(fun loopWrite -> 
            match json with
            | JsonValue.String guid -> box <| Guid.Parse(guid)
            | _ -> failwith (Render.stringify json)
        )
    else None

