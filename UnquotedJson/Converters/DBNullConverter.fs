module UnquotedJson.Converters.DBNullConverter

open UnquotedJson

open System

let tryRead (ty: Type) (value: obj) =
    if ty = typeof<DBNull> || DBNull.Value.Equals value then
        Some(fun loopRead -> JsonValue.Null)
    else
        None

let tryWrite (ty: Type) (json: JsonValue) =
    if ty = typeof<DBNull> then
        Some(fun loopWrite -> box DBNull.Value)
    else
        None
