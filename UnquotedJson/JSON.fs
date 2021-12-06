module UnquotedJson.JSON

///
let parse(text:string) = 
    if System.String.IsNullOrWhiteSpace text then
        failwith "empty string is illeagal json string."
    else
        text
        |> JsonTokenUtils.tokenize
        |> JsonParseTable.parse

///
let stringifyUnquotedJson (json:JsonValue) = JsonRender.stringifyUnquotedJson json

let stringifyNormalJson (json:JsonValue) = JsonRender.stringifyNormalJson json
