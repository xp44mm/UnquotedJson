# UnquotedJson

UnquotedJson is a JSON parser. In addition to being able to parse the standard JSON format, UnquotedJson also allows unquoted strings which are keys and/or in values.

The string may be unquoted if a string is unquoted does not cause parsing ambiguity or errors.

The string must be quoted if a string is unquoted will causes parsing ambiguity or errors.

## Format Convert

unquoted format convert to normal json format:

```F#
let x = """{
    0:{index:0, license:t, nameSID:n, image:"img:left", descriptionSID:t, category:r}
    }"""
let y = 
    x
    |> JSON.parse
    |> JSON.stringifyNormalJson
```

UnquotedJson is a superset of JSON, so the `JSON.parse` parsing function can directly parse normal JSON. follows code convert the normal format to unquoted json format:

```F#
let n = """{
    "0":{"index":0,"license":"t","nameSID":"n","image":"img:left","descriptionSID":"t","category":"r"}
    }"""

let y = 
    n
    |> JSON.parse
    |> JSON.stringifyUnquotedJson
```

## Object Serialization

You can define serialization and deserialization functions for objects.

```F#
let serialize<'t> obj =
    obj
    |> JSON.read<'t>
    |> JSON.stringifyNormalJson

let deserialize<'t> text =
    text
    |> JSON.parse 
    |> JSON.write<'t>
```

Here are some examples of serialization of common object types.

### Tuple

```F#
(1,"x")
```

```json
[1,"x"]
```

### Array, list, Set and so on

```F#
[1;2;3]
```

```json
[1,2,3]
```

### Record

Supports serialization of anonymous records also.

```F#
{ name = "abcdefg"; age = 18 }
```

```json
{"name":"abcdefg","age":18}
```

### Map

```F#
Map [1,"1";2,"2"]
```

```json
[[1,"1"],[2,"2"]]
```

### Option

```F#
[Some 1;None]
```

```json
[1,null]
```

### Union

```F#
type UionExample =
| Zero
| OnlyOne of int
| Pair of int * string

[Zero;OnlyOne 1;Pair(2,"b")]

```

```json
["Zero",{"OnlyOne":1},{"Pair":[2,"b"]}]
```

## Provide tryRead and tryWrite to custom your convert rule

The signature of `tryRead` is:

```F#
tryRead:Type -> obj -> ((Type -> obj -> JsonValue) -> JsonValue) option
```

The signature of `tryWrite` is:

```F#
tryWrite:Type -> JsonValue -> ((Type -> JsonValue -> obj) -> obj) option
```

The return value of `JSON.parse` is `JsonValue` type that is a Discriminated Union type of F#.
