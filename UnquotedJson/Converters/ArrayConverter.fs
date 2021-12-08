module UnquotedJson.Converters.ArrayConverter
open UnquotedJson

open System
open FSharp.Idioms

/// 读取数组的元素
let readArrayElements (loopRead: Type -> obj -> JsonValue) (elemType: Type) (elements: obj[]) =
    let ls =
        elements
        |> List.ofArray
        |> List.map(loopRead elemType)

    JsonValue.Array ls

let tryRead (ty:Type) (value:obj) = 
    if ty.IsArray && ty.GetArrayRank() = 1 then
        Some(fun loop -> 
            let elemType,elements = ArrayType.readArray ty value
            readArrayElements loop elemType elements)
    else None

let tryWrite (ty:Type) (json:JsonValue) = 
    if ty.IsArray && ty.GetArrayRank() = 1 then
        Some(fun loopWrite -> 
            match json with
            | JsonValue.Array elements ->
                let elementType = ArrayType.getElementType ty
                let arr = (Array.CreateInstance:Type*int->Array)(elementType, elements.Length)
                elements
                |> List.map(fun e -> loopWrite elementType e)
                |> List.iteri(fun i v -> arr.SetValue(v, i))
                box arr
            | _ -> failwith "ArrayWriter.write()"
        )
    else None
