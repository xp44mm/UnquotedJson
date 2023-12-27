module UnquotedJson.Converters.RecordConverter
open UnquotedJson

open System
open FSharp.Idioms
open Microsoft.FSharp.Reflection
open FSharp.Idioms.Literal

let tryRead (ty:Type) (value:obj) = 
    if FSharpType.IsRecord ty then
        Some(fun loopRead -> 
            let read = RecordType.readRecord ty
            read value
            |> Array.map(fun(pi,value) -> pi.Name, loopRead pi.PropertyType value)
            |> List.ofArray
            |> JsonValue.Object
        )
    else None

let tryWrite (ty:Type) (json:JsonValue) = 
    if FSharpType.IsRecord ty then
        Some(fun loopWrite -> 
            match json with
            | (JsonValue.Object _) as job ->
                let values =
                    RecordType.getRecordFields(ty)
                    |> Array.map(fun pi -> 
                        if job.ContainsKey pi.Name then
                            loopWrite pi.PropertyType job.[pi.Name]
                        else Literal.zeroDynamic pi.PropertyType
                    )
                FSharpValue.MakeRecord(ty,values)
            | _ -> failwith "RecordWriter.write()"
        )
    else None

