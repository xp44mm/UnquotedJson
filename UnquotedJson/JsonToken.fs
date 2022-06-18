namespace UnquotedJson

type JsonToken = 
    | COMMA
    | COLON
    | LBRACK
    | RBRACK
    | LBRACE
    | RBRACE
    | QUOTED of string
    | UNQUOTED of string
    | WS of string

    member this.raw =
        match this with
        | COMMA  -> ","
        | COLON  -> ":"
        | LBRACK -> "["
        | RBRACK -> "]"
        | LBRACE -> "{"
        | RBRACE -> "}"
        | QUOTED raw 
        | UNQUOTED raw
        | WS raw 
            -> raw
