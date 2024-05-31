module UnquotedJson.JsonParseTable
let tokens = set [",";":";"QUOTED";"UNQUOTED";"[";"]";"{";"}"]
let kernels = [[0,0];[0,1];[-1,1;-2,1];[-1,2];[-2,2;-17,1];[-2,3];[-2,4];[-3,1];[-3,2];[-3,3];[-4,1];[-5,1];[-6,1;-7,1];[-6,2;-15,1];[-6,3];[-6,4];[-7,2];[-8,1];[-9,1];[-10,1];[-11,1];[-13,1;-15,2];[-13,1;-17,2];[-14,1];[-15,3];[-16,1];[-17,3]]
let kernelSymbols = ["";"value";"[";"]";"{value+}";"{\",\"?}";"]";"key";":";"value";"QUOTED";"UNQUOTED";"{";"{field+}";"{\",\"?}";"}";"}";"QUOTED";"UNQUOTED";"array";"object";",";",";"field";"field";"value";"value"]
let actions = [["QUOTED",17;"UNQUOTED",18;"[",2;"array",19;"object",20;"value",1;"{",12];["",0];["QUOTED",17;"UNQUOTED",18;"[",2;"]",3;"array",19;"object",20;"value",25;"{",12;"{value+}",4];["",-1;",",-1;"]",-1;"}",-1];[",",22;"]",-12;"{\",\"?}",5];["]",6];["",-2;",",-2;"]",-2;"}",-2];[":",8];["QUOTED",17;"UNQUOTED",18;"[",2;"array",19;"object",20;"value",9;"{",12];[",",-3;"}",-3];[":",-4];[":",-5];["QUOTED",10;"UNQUOTED",11;"field",23;"key",7;"{field+}",13;"}",16];[",",21;"{\",\"?}",14;"}",-12];["}",15];["",-6;",",-6;"]",-6;"}",-6];["",-7;",",-7;"]",-7;"}",-7];["",-8;",",-8;"]",-8;"}",-8];["",-9;",",-9;"]",-9;"}",-9];["",-10;",",-10;"]",-10;"}",-10];["",-11;",",-11;"]",-11;"}",-11];["QUOTED",10;"UNQUOTED",11;"field",24;"key",7;"}",-13];["QUOTED",17;"UNQUOTED",18;"[",2;"]",-13;"array",19;"object",20;"value",26;"{",12];[",",-14;"}",-14];[",",-15;"}",-15];[",",-16;"]",-16];[",",-17;"]",-17]]
open FSharp.Idioms.Jsons
let rules : list<string list*(obj list->obj)> = [
    ["";"value"], fun(ss:obj list)-> ss.[0]
    ["array";"[";"]"], fun(ss:obj list)->
        let result:Json list =
            []
        box result
    ["array";"[";"{value+}";"{\",\"?}";"]"], fun(ss:obj list)->
        let s1 = unbox<Json list> ss.[1]
        let result:Json list =
            List.rev s1
        box result
    ["field";"key";":";"value"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<Json> ss.[2]
        let result:string*Json =
            s0,s2
        box result
    ["key";"QUOTED"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["key";"UNQUOTED"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["object";"{";"{field+}";"{\",\"?}";"}"], fun(ss:obj list)->
        let s1 = unbox<list<string*Json>> ss.[1]
        let result:list<string*Json> =
            List.rev s1
        box result
    ["object";"{";"}"], fun(ss:obj list)->
        let result:list<string*Json> =
            []
        box result
    ["value";"QUOTED"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:Json =
            Json.String s0
        box result
    ["value";"UNQUOTED"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:Json =
            JsonTokenUtils.fromUnquoted s0
        box result
    ["value";"array"], fun(ss:obj list)->
        let s0 = unbox<Json list> ss.[0]
        let result:Json =
            Json.Array  s0
        box result
    ["value";"object"], fun(ss:obj list)->
        let s0 = unbox<list<string*Json>> ss.[0]
        let result:Json =
            Json.Object s0
        box result
    ["{\",\"?}"], fun(ss:obj list)->
        null
    ["{\",\"?}";","], fun(ss:obj list)->
        null
    ["{field+}";"field"], fun(ss:obj list)->
        let s0 = unbox<string*Json> ss.[0]
        let result:list<string*Json> =
            [s0]
        box result
    ["{field+}";"{field+}";",";"field"], fun(ss:obj list)->
        let s0 = unbox<list<string*Json>> ss.[0]
        let s2 = unbox<string*Json> ss.[2]
        let result:list<string*Json> =
            s2::s0
        box result
    ["{value+}";"value"], fun(ss:obj list)->
        let s0 = unbox<Json> ss.[0]
        let result:Json list =
            [s0]
        box result
    ["{value+}";"{value+}";",";"value"], fun(ss:obj list)->
        let s0 = unbox<Json list> ss.[0]
        let s2 = unbox<Json> ss.[2]
        let result:Json list =
            s2::s0
        box result
]
let unboxRoot =
    unbox<Json>
let app: FslexFsyacc.Runtime.ParseTableApp = {
    tokens        = tokens
    kernels       = kernels
    kernelSymbols = kernelSymbols
    actions       = actions
    rules         = rules
}