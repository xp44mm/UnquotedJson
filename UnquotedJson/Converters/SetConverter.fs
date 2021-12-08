module UnquotedJson.Converters.SetConverter
open UnquotedJson

open System
open System.Reflection
open FSharp.Idioms

let tryRead (ty:Type) (value:obj) = 
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Set<_>> then
        Some(fun loopRead -> 
            let elementType, elements = SetType.readSet ty value
            ArrayConverter.readArrayElements loopRead elementType elements
        )
    else None

let tryWrite (ty:Type) (json:JsonValue) = 
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Set<_>> then
        Some(fun loopWrite -> 
            let arrayType = SetType.makeArrayType ty
            let arr = loopWrite arrayType json

            let mOfArray = SetType.getOfArray ty
            mOfArray.Invoke(null, Array.singleton arr)
        )
    else None

