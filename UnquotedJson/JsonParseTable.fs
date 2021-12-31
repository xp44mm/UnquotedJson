module UnquotedJson.JsonParseTable
let header = "open UnquotedJson\r\nopen UnquotedJson.JsonTokenUtils"
let productions = [|0,[|"";"value"|];-1,[|"array";"[";"]"|];-2,[|"array";"[";"values";"]"|];-3,[|"field";"key";":";"value"|];-4,[|"fields";"field"|];-5,[|"fields";"fields";",";"field"|];-6,[|"key";"QUOTED"|];-7,[|"key";"UNQUOTED"|];-8,[|"object";"{";"fields";"}"|];-9,[|"object";"{";"}"|];-10,[|"value";"QUOTED"|];-11,[|"value";"UNQUOTED"|];-12,[|"value";"array"|];-13,[|"value";"object"|];-14,[|"values";"value"|];-15,[|"values";"values";",";"value"|]|]
let actions = [|0,[|"QUOTED",18;"UNQUOTED",19;"[",2;"array",20;"object",21;"value",1;"{",15|];1,[|"",0|];2,[|"QUOTED",18;"UNQUOTED",19;"[",2;"]",3;"array",20;"object",21;"value",22;"values",4;"{",15|];3,[|"",-1;",",-1;"]",-1;"}",-1|];4,[|",",23;"]",5|];5,[|"",-2;",",-2;"]",-2;"}",-2|];6,[|":",7|];7,[|"QUOTED",18;"UNQUOTED",19;"[",2;"array",20;"object",21;"value",8;"{",15|];8,[|",",-3;"}",-3|];9,[|",",-4;"}",-4|];10,[|",",11;"}",16|];11,[|"QUOTED",13;"UNQUOTED",14;"field",12;"key",6|];12,[|",",-5;"}",-5|];13,[|":",-6|];14,[|":",-7|];15,[|"QUOTED",13;"UNQUOTED",14;"field",9;"fields",10;"key",6;"}",17|];16,[|"",-8;",",-8;"]",-8;"}",-8|];17,[|"",-9;",",-9;"]",-9;"}",-9|];18,[|"",-10;",",-10;"]",-10;"}",-10|];19,[|"",-11;",",-11;"]",-11;"}",-11|];20,[|"",-12;",",-12;"]",-12;"}",-12|];21,[|"",-13;",",-13;"]",-13;"}",-13|];22,[|",",-14;"]",-14|];23,[|"QUOTED",18;"UNQUOTED",19;"[",2;"array",20;"object",21;"value",24;"{",15|];24,[|",",-15;"]",-15|]|]
let kernelSymbols = [|1,"value";2,"[";3,"]";4,"values";5,"]";6,"key";7,":";8,"value";9,"field";10,"fields";11,",";12,"field";13,"QUOTED";14,"UNQUOTED";15,"{";16,"}";17,"}";18,"QUOTED";19,"UNQUOTED";20,"array";21,"object";22,"value";23,",";24,"value"|]
let semantics = [|-1,"[]";-2,"List.rev s1";-3,"s0,s2";-4,"[s0]";-5,"s2::s0";-6,"s0";-7,"s0";-8,"List.rev s1";-9,"[]";-10,"JsonValue.String s0";-11,"fromUnquoted s0";-12,"JsonValue.Array  s0";-13,"JsonValue.Object s0";-14,"[s0]";-15,"s2::s0"|]
let declarations = [|"QUOTED","string";"UNQUOTED","string";"key","string";"array","JsonValue list";"field","string*JsonValue";"fields","list<string*JsonValue>";"object","list<string*JsonValue>";"value","JsonValue";"values","JsonValue list"|]
open UnquotedJson
open UnquotedJson.JsonTokenUtils
let mappers:(int*(obj[]->obj))[] = [|
    -1,fun (ss:obj[]) ->
        // array -> "[" "]"
        let result:JsonValue list =
            []
        box result
    -2,fun (ss:obj[]) ->
        // array -> "[" values "]"
        let s1 = unbox<JsonValue list> ss.[1]
        let result:JsonValue list =
            List.rev s1
        box result
    -3,fun (ss:obj[]) ->
        // field -> key ":" value
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<JsonValue> ss.[2]
        let result:string*JsonValue =
            s0,s2
        box result
    -4,fun (ss:obj[]) ->
        // fields -> field
        let s0 = unbox<string*JsonValue> ss.[0]
        let result:list<string*JsonValue> =
            [s0]
        box result
    -5,fun (ss:obj[]) ->
        // fields -> fields "," field
        let s0 = unbox<list<string*JsonValue>> ss.[0]
        let s2 = unbox<string*JsonValue> ss.[2]
        let result:list<string*JsonValue> =
            s2::s0
        box result
    -6,fun (ss:obj[]) ->
        // key -> QUOTED
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    -7,fun (ss:obj[]) ->
        // key -> UNQUOTED
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    -8,fun (ss:obj[]) ->
        // object -> "{" fields "}"
        let s1 = unbox<list<string*JsonValue>> ss.[1]
        let result:list<string*JsonValue> =
            List.rev s1
        box result
    -9,fun (ss:obj[]) ->
        // object -> "{" "}"
        let result:list<string*JsonValue> =
            []
        box result
    -10,fun (ss:obj[]) ->
        // value -> QUOTED
        let s0 = unbox<string> ss.[0]
        let result:JsonValue =
            JsonValue.String s0
        box result
    -11,fun (ss:obj[]) ->
        // value -> UNQUOTED
        let s0 = unbox<string> ss.[0]
        let result:JsonValue =
            fromUnquoted s0
        box result
    -12,fun (ss:obj[]) ->
        // value -> array
        let s0 = unbox<JsonValue list> ss.[0]
        let result:JsonValue =
            JsonValue.Array  s0
        box result
    -13,fun (ss:obj[]) ->
        // value -> object
        let s0 = unbox<list<string*JsonValue>> ss.[0]
        let result:JsonValue =
            JsonValue.Object s0
        box result
    -14,fun (ss:obj[]) ->
        // values -> value
        let s0 = unbox<JsonValue> ss.[0]
        let result:JsonValue list =
            [s0]
        box result
    -15,fun (ss:obj[]) ->
        // values -> values "," value
        let s0 = unbox<JsonValue list> ss.[0]
        let s2 = unbox<JsonValue> ss.[2]
        let result:JsonValue list =
            s2::s0
        box result|]
open FslexFsyacc.Runtime
let parser = Parser(productions, actions, kernelSymbols, mappers)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<JsonValue>