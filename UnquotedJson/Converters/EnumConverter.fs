module UnquotedJson.Converters.EnumConverter
open UnquotedJson

open System
open FSharp.Idioms.Literal
open FSharp.Idioms

let private _read (ty:Type) (value:obj) =
    if ty.IsDefined(typeof<FlagsAttribute>,false) then
        let reader = EnumType.readFlags ty
        reader value
        |> Array.map(JsonValue.String)
        |> Array.toList
        |> JsonValue.Array
    else
        JsonValue.String <| Enum.GetName(ty,value)

let private _write (ty:Type) (json:JsonValue) =
    let enumUnderlyingType = EnumType.getEnumUnderlyingType ty
    let values = EnumType.getValues ty

    if ty.IsDefined(typeof<FlagsAttribute>,false) then
        match json with
        | JsonValue.Array flags ->
            flags
            |> List.map(function
                | JsonValue.String flag -> values.[flag] 
                | json -> failwith (stringify json)
            )
            |> List.reduce(|||)
        | _ -> failwith (stringify json)
    else
        match json with
        | JsonValue.String enum -> values.[enum]
        | _ -> failwith (stringify json)
    |> EnumType.fromUInt64 enumUnderlyingType


let tryRead (ty:Type) (value:obj) = 
    if ty.IsEnum then
        Some(fun loopRead -> 
            _read ty value
        )
    else None

let tryWrite (ty:Type) (json:JsonValue) = 
    if ty.IsEnum then
        Some(fun loopWrite -> 
            _write ty json
        )
    else None

