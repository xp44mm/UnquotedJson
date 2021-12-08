module UnquotedJson.Converters.NullableConverter
open UnquotedJson

open System

let tryRead (ty:Type) (value:obj) = 
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Nullable<_>> then
        Some(fun loopRead -> 
            if value = null then
                JsonValue.Null
            else
                let underlyingType = ty.GenericTypeArguments.[0]
                loopRead underlyingType value
        )
    else None

let tryWrite (ty:Type) (json:JsonValue) = 
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Nullable<_>> then
        Some(fun loopWrite -> 
            match json with
            | JsonValue.Null -> null
            | _ ->
                let underlyingType = ty.GenericTypeArguments.[0]
                loopWrite underlyingType json
        )
    else None

