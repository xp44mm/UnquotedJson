# UnquotedJson

UnquotedJson is a JSON parser. In addition to being able to parse the standard JSON format, UnquotedJson also allows unquoted strings which are keys and/or strings in values.

When a string is unquoted and does not cause parsing ambiguity or errors, the string can be unquoted or quoted.

When a string is unquoted and causes parsing ambiguity or errors, the string must be quoted.

## Example

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

The return value of `JSON.parse` is `JsonValue` type that is a Discriminated Union type of F#.
