module UnquotedJson.Converters.OptionConverter
open UnquotedJson

open System
open FSharp.Idioms
open Microsoft.FSharp.Reflection

let tryRead (ty:Type) (value:obj) = 
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Option<_>> then
        Some(fun loopRead -> 
            if value = null then
                JsonValue.Null
            else
                let reader = UnionType.readUnion ty
                let _,fields = reader value
                let ftype,fvalue = fields.[0]
                loopRead ftype fvalue
        )
    else None

let tryWrite (ty:Type) (json:JsonValue) = 
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Option<_>> then
        Some(fun loopWrite -> 
            match json with
            | JsonValue.Null -> box None
            | jvalue ->
                let unionCaseInfo =
                    FSharpType.GetUnionCases ty
                    |> Array.find(fun c -> c.Name = "Some")
                let uionFieldType =
                    unionCaseInfo.GetFields()
                    |> Array.map(fun info -> info.PropertyType)
                    |> Array.exactlyOne
                FSharpValue.MakeUnion(unionCaseInfo, Array.singleton(loopWrite uionFieldType jvalue))
        )
    else None

