module UnquotedJson.Converters.FallbackConverter

open UnquotedJson
open System
open FSharp.Literals

let assertType<'t> (ty:Type) (value:obj) =
    let t = typeof<'t>
    if ty = t then value else failwithf "type should be `%s`"  t.Name


let fallbackRead (ty) (value) (loopRead:Type -> obj -> JsonValue) =
    //已知问题：a loss of precision is possible when converting Decimal, Int64, and UInt64 values to Double values.
    if ty = typeof<bool> then
        let b = unbox<bool> value
        if b then JsonValue.True else JsonValue.False

    elif ty = typeof<sbyte> then
        let value = unbox<sbyte> value
        JsonValue.Number <| Convert.ToDouble value

    elif ty = typeof<byte> then
        let value = unbox<byte> value
        JsonValue.Number <| Convert.ToDouble value

    elif ty = typeof<int16> then
        let value = unbox<int16> value
        JsonValue.Number <| Convert.ToDouble value

    elif ty = typeof<uint16> then
        let value = unbox<uint16> value
        JsonValue.Number <| Convert.ToDouble value

    elif ty = typeof<int> then
        let value = unbox<int> value
        JsonValue.Number <| Convert.ToDouble value

    elif ty = typeof<uint32> then
        let value = unbox<uint32> value
        JsonValue.Number <| Convert.ToDouble value

    elif ty = typeof<int64> then
        let value = unbox<int64> value
        JsonValue.Number <| Convert.ToDouble value

    elif ty = typeof<uint64> then
        let value = unbox<uint64> value
        JsonValue.Number <| Convert.ToDouble value

    elif ty = typeof<single> then
        let value = unbox<single> value
        JsonValue.Number <| Math.Round(Convert.ToDouble value,8)

    elif ty = typeof<float> then
        let value = unbox<float> value
        JsonValue.Number <| Convert.ToDouble value

    elif ty = typeof<char> then
        unbox<char> value
        |> Char.ToString
        |> JsonValue.String

    elif ty = typeof<string> then
        unbox<string> value
        |> JsonValue.String

    elif ty = typeof<decimal> then
        let value = unbox<decimal> value
        JsonValue.Number <| Convert.ToDouble value

    elif ty = typeof<nativeint> then
        let value = unbox<nativeint> value
        JsonValue.Number <| Convert.ToDouble(value.ToInt64())

    elif ty = typeof<unativeint> then
        let value = unbox<unativeint> value
        JsonValue.Number <| Convert.ToDouble(value.ToUInt64())

    elif isNull value then
        JsonValue.Null
    elif ty = typeof<obj> && value.GetType() <> typeof<obj> then
        loopRead (value.GetType()) value
    else
        JsonValue.String (Render.stringify value)

///
let fallbackWrite (ty:Type) (json:JsonValue) =
    match json with
    | JsonValue.Null  -> null
    | JsonValue.False -> box false |> assertType<bool> ty
    | JsonValue.True  -> box true  |> assertType<bool> ty
    | JsonValue.String x ->
        if ty = typeof<string> then
            box x
        elif ty = typeof<char> then
            if x = "" then
                failwith "empty string can't write to char error."
            box x.[0]
        else
            failwithf "type should be `%s`"  ty.Name
    | JsonValue.Number x -> 
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/conversion-tables
        if ty = typeof<sbyte> then
            Convert.ToSByte x
            |> box

        elif ty = typeof<byte> then
            Convert.ToByte x
            |> box

        elif ty = typeof<int16> then
            Convert.ToInt16 x
            |> box

        elif ty = typeof<uint16> then
            Convert.ToUInt16 x
            |> box
  
        elif ty = typeof<int> then
            Convert.ToInt32 x
            |> box

        elif ty = typeof<uint32> then
            Convert.ToUInt32 x
            |> box

        elif ty = typeof<int64> then
            Convert.ToInt64 x
            |> box

        elif ty = typeof<uint64> then
            Convert.ToUInt64 x
            |> box

        elif ty = typeof<single> then
            Convert.ToSingle x
            |> box

        elif ty = typeof<float> then
            Convert.ToDouble x
            |> box

        elif ty = typeof<decimal> then
            Convert.ToDecimal x
            |> box

        elif ty = typeof<nativeint> then
            IntPtr(Convert.ToInt64 x)
            |> box

        elif ty = typeof<unativeint> then
            UIntPtr(Convert.ToUInt64 x)
            |> box

        else
            failwithf "type should be `%s`"  ty.Name

    | JsonValue.Object _ -> failwith "not allowed type"
    | JsonValue.Array _ -> failwith "not allowed type"




