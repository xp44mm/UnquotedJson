module UnquotedJson.Converters.ListConverter
open UnquotedJson

open System
open FSharp.Idioms

let tryRead (ty:Type) (value:obj) = 
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<List<_>> then
        Some(fun loopRead -> 
            let elemType, elements = ListType.readList ty value
            ArrayConverter.readArrayElements loopRead elemType elements
        )
    else None


let tryWrite (ty:Type) (json:JsonValue) = 
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<List<_>> then
        Some(fun loopWrite -> 
            let elementType = ListType.getElementType ty
            let arrayType = elementType.MakeArrayType()
            let arr = loopWrite arrayType json
            let mOfArray = ListType.getOfArray ty
            mOfArray.Invoke(null, Array.singleton arr)
        )
    else None

