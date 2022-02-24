module UnquotedJson.JsonParseTable
let rules = [|["value";"object"],"JsonValue.Object s0";["value";"array"],"JsonValue.Array  s0";["value";"QUOTED"],"JsonValue.String s0";["value";"UNQUOTED"],"fromUnquoted s0";["object";"{";"}"],"[]";["object";"{";"fields";"}"],"List.rev s1";["array";"[";"]"],"[]";["array";"[";"values";"]"],"List.rev s1";["fields";"field"],"[s0]";["fields";"fields";",";"field"],"s2::s0";["field";"key";":";"value"],"s0,s2";["key";"QUOTED"],"s0";["key";"UNQUOTED"],"s0";["values";"value"],"[s0]";["values";"values";",";"value"],"s2::s0"|]
let actions = [|[|"QUOTED",18;"UNQUOTED",19;"[",2;"array",20;"object",21;"value",1;"{",15|];[|"",0|];[|"QUOTED",18;"UNQUOTED",19;"[",2;"]",3;"array",20;"object",21;"value",22;"values",4;"{",15|];[|"",-1;",",-1;"]",-1;"}",-1|];[|",",23;"]",5|];[|"",-2;",",-2;"]",-2;"}",-2|];[|":",7|];[|"QUOTED",18;"UNQUOTED",19;"[",2;"array",20;"object",21;"value",8;"{",15|];[|",",-3;"}",-3|];[|",",-4;"}",-4|];[|",",11;"}",16|];[|"QUOTED",13;"UNQUOTED",14;"field",12;"key",6|];[|",",-5;"}",-5|];[|":",-6|];[|":",-7|];[|"QUOTED",13;"UNQUOTED",14;"field",9;"fields",10;"key",6;"}",17|];[|"",-8;",",-8;"]",-8;"}",-8|];[|"",-9;",",-9;"]",-9;"}",-9|];[|"",-10;",",-10;"]",-10;"}",-10|];[|"",-11;",",-11;"]",-11;"}",-11|];[|"",-12;",",-12;"]",-12;"}",-12|];[|"",-13;",",-13;"]",-13;"}",-13|];[|",",-14;"]",-14|];[|"QUOTED",18;"UNQUOTED",19;"[",2;"array",20;"object",21;"value",24;"{",15|];[|",",-15;"]",-15|]|]
let closures = [|[|0,0,[||];-1,0,[||];-2,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||]|];[|0,1,[|""|]|];[|-1,0,[||];-1,1,[||];-2,0,[||];-2,1,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||]|];[|-1,2,[|"";",";"]";"}"|]|];[|-2,2,[||];-15,1,[||]|];[|-2,3,[|"";",";"]";"}"|]|];[|-3,1,[||]|];[|-1,0,[||];-2,0,[||];-3,2,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||]|];[|-3,3,[|",";"}"|]|];[|-4,1,[|",";"}"|]|];[|-5,1,[||];-8,2,[||]|];[|-3,0,[||];-5,2,[||];-6,0,[||];-7,0,[||]|];[|-5,3,[|",";"}"|]|];[|-6,1,[|":"|]|];[|-7,1,[|":"|]|];[|-3,0,[||];-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,1,[||];-9,1,[||]|];[|-8,3,[|"";",";"]";"}"|]|];[|-9,2,[|"";",";"]";"}"|]|];[|-10,1,[|"";",";"]";"}"|]|];[|-11,1,[|"";",";"]";"}"|]|];[|-12,1,[|"";",";"]";"}"|]|];[|-13,1,[|"";",";"]";"}"|]|];[|-14,1,[|",";"]"|]|];[|-1,0,[||];-2,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-15,2,[||]|];[|-15,3,[|",";"]"|]|]|]
let header = "open UnquotedJson\r\nopen UnquotedJson.JsonTokenUtils"
let declarations = [|"QUOTED","string";"UNQUOTED","string";"key","string";"array","JsonValue list";"field","string*JsonValue";"fields","list<string*JsonValue>";"object","list<string*JsonValue>";"value","JsonValue";"values","JsonValue list"|]
open UnquotedJson
open UnquotedJson.JsonTokenUtils
let fxRules:(string list*(obj[]->obj))[] = [|
    ["value";"object"],fun (ss:obj[]) ->
            let s0 = unbox<list<string*JsonValue>> ss.[0]
            let result:JsonValue =
                JsonValue.Object s0
            box result
    ["value";"array"],fun (ss:obj[]) ->
            let s0 = unbox<JsonValue list> ss.[0]
            let result:JsonValue =
                JsonValue.Array  s0
            box result
    ["value";"QUOTED"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let result:JsonValue =
                JsonValue.String s0
            box result
    ["value";"UNQUOTED"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let result:JsonValue =
                fromUnquoted s0
            box result
    ["object";"{";"}"],fun (ss:obj[]) ->
            let result:list<string*JsonValue> =
                []
            box result
    ["object";"{";"fields";"}"],fun (ss:obj[]) ->
            let s1 = unbox<list<string*JsonValue>> ss.[1]
            let result:list<string*JsonValue> =
                List.rev s1
            box result
    ["array";"[";"]"],fun (ss:obj[]) ->
            let result:JsonValue list =
                []
            box result
    ["array";"[";"values";"]"],fun (ss:obj[]) ->
            let s1 = unbox<JsonValue list> ss.[1]
            let result:JsonValue list =
                List.rev s1
            box result
    ["fields";"field"],fun (ss:obj[]) ->
            let s0 = unbox<string*JsonValue> ss.[0]
            let result:list<string*JsonValue> =
                [s0]
            box result
    ["fields";"fields";",";"field"],fun (ss:obj[]) ->
            let s0 = unbox<list<string*JsonValue>> ss.[0]
            let s2 = unbox<string*JsonValue> ss.[2]
            let result:list<string*JsonValue> =
                s2::s0
            box result
    ["field";"key";":";"value"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let s2 = unbox<JsonValue> ss.[2]
            let result:string*JsonValue =
                s0,s2
            box result
    ["key";"QUOTED"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let result:string =
                s0
            box result
    ["key";"UNQUOTED"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let result:string =
                s0
            box result
    ["values";"value"],fun (ss:obj[]) ->
            let s0 = unbox<JsonValue> ss.[0]
            let result:JsonValue list =
                [s0]
            box result
    ["values";"values";",";"value"],fun (ss:obj[]) ->
            let s0 = unbox<JsonValue list> ss.[0]
            let s2 = unbox<JsonValue> ss.[2]
            let result:JsonValue list =
                s2::s0
            box result
|]
open FslexFsyacc.Runtime
let parser = Parser(fxRules, actions, closures)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<JsonValue>