namespace UnquotedJson

[<RequireQualifiedAccess>]
type JsonValue =
    | Object of list<string*JsonValue>
    | Array  of JsonValue list
    | Null
    | False
    | True
    | String of string
    | Number of float 

    member t.Item with get(idx:int) =
        match t with
        | JsonValue.Array ls -> ls.[idx]
        | _ -> failwith "only for array."

    member t.Item with get(key:string) =
        match t with
        | JsonValue.Object pairs -> 
            match pairs |> List.tryFind(fst>>(=)key) with
            | Some(key,json) -> json
            | _ -> failwith "no found key."
        | _ -> failwith "only for object."
            
    member j.ContainsKey(key:string) =
        match j with
        | JsonValue.Object pairs -> 
            pairs
            |> List.exists(fst>>(=)key)
        | _ -> false
