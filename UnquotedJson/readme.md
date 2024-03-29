# UnquotedJson

UnquotedJson is a JSON5 parser.

### Objects
- Object keys may be an ECMAScript 5.1.
- Objects may have a single trailing comma.

### Arrays
- Arrays may have a single trailing comma.

### Comments
- Single and multi-line comments are allowed.

### White Space
- Additional white space characters are allowed.

## Format Convert

unquoted format convert to normal json format:

```F#
let x = """{
    0:{index:0, license:t, nameSID:n, image:"img:left", descriptionSID:t, category:r}
    }"""
let y = 
    x
    |> Json.parse
    |> Json.print
```

UnquotedJson is a superset of JSON, so the `JSON.parse` parsing function can directly parse normal JSON. follows code convert the normal format to unquoted json format:

```F#
let n = """{
    "0":{"index":0,"license":"t","nameSID":"n","image":"img:left","descriptionSID":"t","category":"r"}
    }"""

let y = 
    n
    |> Json.parse
    |> Json.print
```

## Object Serialization

You can define serialization and deserialization functions for objects.

```F#
let serialize<'t> obj =
    obj
    |> Json.read<'t>
    |> Json.print

let deserialize<'t> text =
    text
    |> Json.parse 
    |> Json.write<'t>
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
[[1],[]]
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
["Zero";["OnlyOne", 1];["Pair",2,"b"]]
```

## Provide tryRead and tryWrite to custom your convert rule

The signature of `tryRead` is:

```F#
tryRead:Type -> obj -> ((Type -> obj -> Json) -> Json) option
```

The signature of `tryWrite` is:

```F#
tryWrite:Type -> Json -> ((Type -> Json -> obj) -> obj) option
```

The usage see also [FSharpConverter.fs](https://github.com/xp44mm/UnquotedJson/blob/master/UnquotedJson/Converters/FSharpConverter.fs)
The return value of `JSON.parse` is `Json` type that is a Discriminated Union type of F#.

### UrlQuery

UnquotedJson can be used for query strings in URLs. When the field is of primitive type, the query string format is used. When the field is a complex type, use the Unqoted Json format.

The source see [UrlQuery](https://github.com/xp44mm/UnquotedJson/blob/master/UnquotedJson/UrlQuery.fs)

The usage see also [UrlQueryTest.fs](https://github.com/xp44mm/UnquotedJson/blob/master/UnquotedJson.Test/UrlQueryTest.fs)


## API

The main structure types are defined as follows:

- The type `Json` see to [Json](https://github.com/xp44mm/UnquotedJson/blob/master/UnquotedJson/Json.fs).

- The type `JsonToken` see to [JsonToken](https://github.com/xp44mm/UnquotedJson/blob/master/UnquotedJson/JsonToken.fs).



