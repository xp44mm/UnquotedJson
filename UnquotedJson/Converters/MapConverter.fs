module UnquotedJson.Converters.MapConverter
open UnquotedJson

open System
open FSharp.Idioms

let tryRead (ty:Type) (value:obj) = 
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Map<_,_>> then
        Some(fun loopRead -> 
            let tupleType, elements = MapType.readMap ty value
            ArrayConverter.readArrayElements loopRead tupleType elements
        )
    else None


let tryWrite (ty:Type) (json:JsonValue) = 
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Map<_,_>> then
        Some(fun loopWrite -> 
            let arrayType = MapType.makeArrayType(ty)
            let mOfArray = MapType.getOfArray(ty)
            let arr = loopWrite arrayType json
            mOfArray.Invoke(null, Array.singleton arr)
        )
    else None


