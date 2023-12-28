module UnquotedJson.JsonParseTable
let actions = [["QUOTED",15;"UNQUOTED",16;"[",2;"array",17;"object",18;"value",1;"{",11];["",0];["QUOTED",15;"UNQUOTED",16;"[",2;"]",3;"array",17;"object",18;"value",22;"{",11;"{value+}",4];["",-1;",",-1;"]",-1;"}",-1];[",",23;"]",5];["",-2;",",-2;"]",-2;"}",-2];[":",7];["QUOTED",15;"UNQUOTED",16;"[",2;"array",17;"object",18;"value",8;"{",11];[",",-3;"}",-3];[":",-4];[":",-5];["QUOTED",9;"UNQUOTED",10;"field",19;"key",6;"{field+}",12;"}",14];[",",20;"}",13];["",-6;",",-6;"]",-6;"}",-6];["",-7;",",-7;"]",-7;"}",-7];["",-8;",",-8;"]",-8;"}",-8];["",-9;",",-9;"]",-9;"}",-9];["",-10;",",-10;"]",-10;"}",-10];["",-11;",",-11;"]",-11;"}",-11];[",",-12;"}",-12];["QUOTED",9;"UNQUOTED",10;"field",21;"key",6];[",",-13;"}",-13];[",",-14;"]",-14];["QUOTED",15;"UNQUOTED",16;"[",2;"array",17;"object",18;"value",24;"{",11];[",",-15;"]",-15]]
let closures = [[0,0,[];-1,0,[];-2,0,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[]];[0,1,[""]];[-1,0,[];-1,1,[];-2,0,[];-2,1,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-14,0,[];-15,0,[]];[-1,2,["";",";"]";"}"]];[-2,2,[];-15,1,[]];[-2,3,["";",";"]";"}"]];[-3,1,[]];[-1,0,[];-2,0,[];-3,2,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[]];[-3,3,[",";"}"]];[-4,1,[":"]];[-5,1,[":"]];[-3,0,[];-4,0,[];-5,0,[];-6,1,[];-7,1,[];-12,0,[];-13,0,[]];[-6,2,[];-13,1,[]];[-6,3,["";",";"]";"}"]];[-7,2,["";",";"]";"}"]];[-8,1,["";",";"]";"}"]];[-9,1,["";",";"]";"}"]];[-10,1,["";",";"]";"}"]];[-11,1,["";",";"]";"}"]];[-12,1,[",";"}"]];[-3,0,[];-4,0,[];-5,0,[];-13,2,[]];[-13,3,[",";"}"]];[-14,1,[",";"]"]];[-1,0,[];-2,0,[];-6,0,[];-7,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[];-15,2,[]];[-15,3,[",";"]"]]]
open FSharp.Idioms.Jsons
let rules:(string list*(obj list->obj))list = [
    ["value";"object"],fun(ss:obj list)->
        let s0 = unbox<list<string*Json>> ss.[0]
        let result:Json =
            Json.Object s0
        box result
    ["value";"array"],fun(ss:obj list)->
        let s0 = unbox<Json list> ss.[0]
        let result:Json =
            Json.Array  s0
        box result
    ["value";"QUOTED"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:Json =
            Json.String s0
        box result
    ["value";"UNQUOTED"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:Json =
            JsonTokenUtils.fromUnquoted s0
        box result
    ["object";"{";"}"],fun(ss:obj list)->
        let result:list<string*Json> =
            []
        box result
    ["object";"{";"{field+}";"}"],fun(ss:obj list)->
        let s1 = unbox<list<string*Json>> ss.[1]
        let result:list<string*Json> =
            List.rev s1
        box result
    ["{field+}";"field"],fun(ss:obj list)->
        let s0 = unbox<string*Json> ss.[0]
        let result:list<string*Json> =
            [s0]
        box result
    ["{field+}";"{field+}";",";"field"],fun(ss:obj list)->
        let s0 = unbox<list<string*Json>> ss.[0]
        let s2 = unbox<string*Json> ss.[2]
        let result:list<string*Json> =
            s2::s0
        box result
    ["field";"key";":";"value"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<Json> ss.[2]
        let result:string*Json =
            s0,s2
        box result
    ["key";"QUOTED"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["key";"UNQUOTED"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["array";"[";"]"],fun(ss:obj list)->
        let result:Json list =
            []
        box result
    ["array";"[";"{value+}";"]"],fun(ss:obj list)->
        let s1 = unbox<Json list> ss.[1]
        let result:Json list =
            List.rev s1
        box result
    ["{value+}";"value"],fun(ss:obj list)->
        let s0 = unbox<Json> ss.[0]
        let result:Json list =
            [s0]
        box result
    ["{value+}";"{value+}";",";"value"],fun(ss:obj list)->
        let s0 = unbox<Json list> ss.[0]
        let s2 = unbox<Json> ss.[2]
        let result:Json list =
            s2::s0
        box result
]
let unboxRoot =
    unbox<Json>
let theoryParser = FslexFsyacc.Runtime.TheoryParser.create(rules, actions, closures)
let stateSymbolPairs = theoryParser.getStateSymbolPairs()