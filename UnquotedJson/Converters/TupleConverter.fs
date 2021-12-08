module UnquotedJson.Converters.TupleConverter

open UnquotedJson

open System
open Microsoft.FSharp.Reflection
open FSharp.Idioms

/// 读取元组的字段
let readTupleFields (loopRead:Type -> obj -> JsonValue) fields =
    fields
    |> List.ofArray
    |> List.map(fun(ftype,field)-> loopRead ftype field)
    |> JsonValue.Array

let tryRead (ty: Type) (value: obj) =
    if FSharpType.IsTuple ty then
        Some
            (fun loopRead ->
                let read = TupleType.readTuple ty
                read value |> readTupleFields loopRead)
    else
        None

let tryWrite (ty: Type) (json: JsonValue) =
    if FSharpType.IsTuple ty then
        Some
            (fun loopWrite ->
                match json with
                | JsonValue.Array elements ->
                    let elements = Array.ofList elements
                    let elementTypes = FSharpType.GetTupleElements(ty)

                    let values =
                        Array.zip elementTypes elements
                        |> Array.map (fun (tp, json) -> loopWrite tp json)

                    FSharpValue.MakeTuple(values, ty)
                | _ -> failwith "TupleWriter.write()")
    else
        None
