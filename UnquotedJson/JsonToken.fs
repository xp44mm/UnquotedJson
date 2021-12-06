namespace UnquotedJson

type JsonToken = 
| COMMA
| COLON
| RBRACK
| LBRACK
| RBRACE
| LBRACE
| QUOTED of string
| UNQUOTED of string

//| KEY of string
//| NUMBER of float
//| NULL
//| FALSE
//| TRUE

