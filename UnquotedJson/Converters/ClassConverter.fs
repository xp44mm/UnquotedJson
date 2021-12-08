module UnquotedJson.Converters.ClassConverter
open UnquotedJson

open System
open System.Reflection

let tryRead (ty:Type) (value:obj) = 
    if ty.IsClass && ty <> typeof<string> then
        Some(fun loopRead -> 
            if isNull value then JsonValue.Null else
            let members =
                ty.GetMembers(BindingFlags.Public ||| BindingFlags.Instance)
                |> Array.filter(fun mmbr ->
                    match mmbr.MemberType with
                    | MemberTypes.Field ->
                        let fieldInfo = mmbr :?> FieldInfo
                        not fieldInfo.IsLiteral && not fieldInfo.IsInitOnly
                    | MemberTypes.Property ->
                        let propertyInfo = mmbr :?> PropertyInfo
                        propertyInfo.CanRead && propertyInfo.CanWrite
                     | _ -> false
                )
                |> Array.map(fun mmbr ->
                    let json =
                        match mmbr.MemberType with
                        | MemberTypes.Field ->
                            let fieldInfo = mmbr:?>FieldInfo
                            let value = fieldInfo.GetValue(value)
                            loopRead fieldInfo.FieldType value
                        | MemberTypes.Property ->
                            let propertyInfo = mmbr :?> PropertyInfo
                            let value = propertyInfo.GetValue(value)
                            loopRead propertyInfo.PropertyType value
                        | _ -> failwith "never"
                    mmbr.Name, json
                )
                |> List.ofArray
            JsonValue.Object members)
    else None

let tryWrite (ty:Type) (json:JsonValue) = 
    if ty.IsClass && ty <> typeof<string> then
        Some(fun loopWrite -> 
            match json with
            | JsonValue.Null -> null
            | JsonValue.Object ls ->
                let target = Activator.CreateInstance(ty)
                ls
                |> List.iter(fun (name,json) -> 
                    let mmbrs = ty.GetMember(name, BindingFlags.Public ||| BindingFlags.Instance)
                    if Array.isEmpty mmbrs then
                        ()
                    else
                        let mmbr = Array.exactlyOne mmbrs
                        match mmbr.MemberType with
                        | MemberTypes.Field ->
                            let fieldInfo = mmbr:?>FieldInfo
                            let value = loopWrite fieldInfo.FieldType json
                            fieldInfo.SetValue(target,value)
                        | MemberTypes.Property ->
                            let propertyInfo = mmbr:?>PropertyInfo
                            let value = loopWrite propertyInfo.PropertyType json
                            propertyInfo.SetValue(target,value)
                        | _ -> ()
                       
                    )
                target
            | _ -> failwith "ClassWriter.write()")
    else None
