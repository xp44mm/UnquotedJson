module UnquotedJson.Converters.UnionConverter
open UnquotedJson

open FSharp.Idioms
open Microsoft.FSharp.Reflection
open System

let tryRead (ty: Type) (value: obj) =
    if FSharpType.IsUnion ty then
        Some(fun loopRead -> 
            let reader = UnionType.readUnion ty
            let name,fields = reader value
            //简化            
            if Array.isEmpty fields then 
                // union case is paramless
                JsonValue.String name
            elif fields.Length = 1 then
                // union case is single param
                let unionFields = loopRead <|| fields.[0]
                JsonValue.Object [name,unionFields]
            else 
                // union case is tuple
                let unionFields = TupleConverter.readTupleFields loopRead fields
                JsonValue.Object [name,unionFields]
        )
    else
        None

let tryWrite (ty: Type) (json: JsonValue) =

    if FSharpType.IsUnion ty then
        Some(fun loopWrite -> 
            match json with
            | JsonValue.Object fields ->
                let jkey, jvalue =  fields |> List.exactlyOne

                let unionCaseInfo =
                    UnionType.getUnionCases ty
                    |> Array.find(fun c -> c.Name = jkey)

                let uionFieldTypes =
                    UnionType.getCaseFields unionCaseInfo
                    |> Array.map(fun info -> info.PropertyType)

                match uionFieldTypes with
                | [||] ->
                    FSharpValue.MakeUnion(unionCaseInfo, Array.empty)
                | [|fieldType|] ->
                    FSharpValue.MakeUnion(unionCaseInfo, Array.singleton(loopWrite fieldType jvalue))
                | _ ->
                    let fields =
                        match jvalue with
                        | JsonValue.Array elements ->
                            elements
                            |> Array.ofList
                            |> Array.zip uionFieldTypes
                            |> Array.map(fun (t,j) -> loopWrite t j)
                        | _ -> failwith "JsonValue structure does not match"

                    FSharpValue.MakeUnion(unionCaseInfo, fields)
            | JsonValue.String tag -> 
                let unionCaseInfo =
                    UnionType.getUnionCases ty
                    |> Array.find(fun c -> c.Name = tag)
                                    
                FSharpValue.MakeUnion(unionCaseInfo, Array.empty)

            | _ -> failwith "JsonValue structure does not match"
        )
    else
        None

