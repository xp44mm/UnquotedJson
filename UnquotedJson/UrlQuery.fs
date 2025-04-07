module UnquotedJson.UrlQuery

open System
open System.Reflection

open Microsoft.FSharp.Reflection

open FSharp.Idioms
open FSharp.Idioms.Literal

let parseFieldDynamic (ty:Type) (txt:string) =
    if txt = "" then
        if ty = typeof<string> then
            box ""
        elif ty = typeof<DBNull> then 
            box DBNull.Value 
        else null
    elif ty = typeof<string> then
        box txt
    elif ty = typeof<char> then
        box txt.[0]
    elif ty = typeof<bool> then
        Boolean.Parse txt
        |> box
    elif ty = typeof<sbyte> then
        SByte.Parse txt
        |> box
    elif ty = typeof<byte> then
        Byte.Parse txt
        |> box
    elif ty = typeof<int16> then
        Int16.Parse txt
        |> box
    elif ty = typeof<uint16> then
        UInt16.Parse txt
        |> box
    elif ty = typeof<int> then
        Int32.Parse txt
        |> box
    elif ty = typeof<uint32> then
        UInt32.Parse txt
        |> box
    elif ty = typeof<single> then
        Single.Parse txt
        |> box
    elif ty = typeof<float> then
        Double.Parse txt
        |> box

    elif ty = typeof<int64> then
        Int64.Parse txt
        |> box
    elif ty = typeof<uint64> then
        UInt64.Parse txt
        |> box
    elif ty = typeof<decimal> then
        Decimal.Parse txt
        |> box
    else
        txt
        |> Json.parse
        |> Json.toObj ty
    
let parseField<'t> (value:string) = 
    let ty = typeof<'t>
    let json = parseFieldDynamic ty value
    json :?> 't

let parseQueryDynamic (ty:Type) (fields:seq<string*string>) =
    if FSharpType.IsRecord ty then
        let fields = Map.ofSeq fields
        let props = 
            FSharpType.GetRecordFields ty
            |> Array.map(fun pi -> 
                if fields.ContainsKey pi.Name then
                    parseFieldDynamic pi.PropertyType fields.[pi.Name]
                else defaultofDynamic pi.PropertyType)
        FSharpValue.MakeRecord(ty,props)
    else
        let target = Activator.CreateInstance(ty)
        fields
        |> Seq.iter(fun (name,json) -> 
            let mmbr = 
                ty.GetMember(name, BindingFlags.Public ||| BindingFlags.Instance)
                |> Array.exactlyOne
            match mmbr.MemberType with
            | MemberTypes.Field ->
                let fieldInfo = mmbr :?> FieldInfo
                let fty = fieldInfo.FieldType
                fieldInfo.SetValue(target, parseFieldDynamic fty json)
            | MemberTypes.Property ->
                let propertyInfo = mmbr :?> PropertyInfo
                let pty = propertyInfo.PropertyType
                propertyInfo.SetValue(target, parseFieldDynamic pty json)
            | _ -> ()
        )
        target

/// parse Query string collection
let parseQuery<'t> (fields:seq<string*string>) =
    parseQueryDynamic typeof<'t> fields :?> 't

