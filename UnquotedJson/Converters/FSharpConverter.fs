module UnquotedJson.Converters.FSharpConverter
open UnquotedJson
open System

let tryReaders = [
    DateTimeOffsetConverter.tryRead
    TimeSpanConverter      .tryRead
    GuidConverter          .tryRead
    UriConverter           .tryRead
    EnumConverter          .tryRead
    DBNullConverter        .tryRead
    NullableConverter      .tryRead
    OptionConverter        .tryRead
    ArrayConverter         .tryRead
    TupleConverter         .tryRead
    RecordConverter        .tryRead
    ListConverter          .tryRead
    SetConverter           .tryRead
    MapConverter           .tryRead
    UnionConverter         .tryRead
    ClassConverter         .tryRead
    ]

let tryWriters = [
    DateTimeOffsetConverter.tryWrite
    TimeSpanConverter      .tryWrite
    GuidConverter          .tryWrite
    UriConverter           .tryWrite
    EnumConverter          .tryWrite
    DBNullConverter        .tryWrite
    NullableConverter      .tryWrite
    OptionConverter        .tryWrite
    ArrayConverter         .tryWrite
    TupleConverter         .tryWrite
    RecordConverter        .tryWrite
    ListConverter          .tryWrite
    SetConverter           .tryWrite
    MapConverter           .tryWrite
    UnionConverter         .tryWrite
    ClassConverter         .tryWrite
]

/// dynamic read from obj to json
let rec readObj (tryReaders:seq<Type -> obj -> ((Type -> obj -> JsonValue) -> JsonValue) option>) (ty:Type) (value:obj) =
    let read =
        tryReaders
        |> Seq.tryPick(fun tryRead -> tryRead ty value)
        |> Option.defaultValue (FallbackConverter.fallbackRead ty value)
    read(readObj tryReaders)

/// write to obj from json.
let rec writeObj (tryWriters:seq<Type -> JsonValue -> ((Type -> JsonValue -> obj) -> obj) option>) (ty:Type) (json:JsonValue) =
    let write =
        tryWriters
        |> Seq.tryPick(fun tryWrite -> tryWrite ty json)
        |> Option.defaultValue (fun _ -> FallbackConverter.fallbackWrite ty json)

    write(writeObj tryWriters)



