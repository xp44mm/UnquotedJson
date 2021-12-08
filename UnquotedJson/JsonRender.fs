﻿module UnquotedJson.JsonRender

open System
open FSharp.Idioms
open System.Text.RegularExpressions

let stringifyKey x =
    if String.IsNullOrWhiteSpace x || Regex.IsMatch(x,@"[,:{}[\]""]|(^\s)|(\s$)") then
        quote x
    else
        x
    
let stringifyStringValue x =
    if x = "true" || x = "false" || x = "null" then
        quote x
    elif Regex.IsMatch(x, @"[-+]?\d+(\.\d+)?([eE][-+]?\d+)?") then
        quote x
    else
        stringifyKey x

let rec stringifyUnquotedJson (json:JsonValue)= 
    match json with
    | JsonValue.Object pairs ->
        pairs
        |> List.map(fun(k,v)->
           stringifyKey k + ":" + stringifyUnquotedJson v
        )
        |> String.concat ","
        |> sprintf "{%s}"

    | JsonValue.Array ls ->
        ls
        |> List.map(stringifyUnquotedJson)
        |> String.concat ","
        |> sprintf "[%s]"

    | JsonValue.Null -> "null"
    | JsonValue.False -> "false"
    | JsonValue.True -> "true"
    | JsonValue.String x -> stringifyStringValue x
    | JsonValue.Number c -> Convert.ToString c

let rec stringifyNormalJson (json:JsonValue)= 
    match json with
    | JsonValue.Object pairs ->
        pairs
        |> List.map(fun(k,v)->
           quote k + ":" + stringifyNormalJson v
        )
        |> String.concat ","
        |> sprintf "{%s}"

    | JsonValue.Array ls ->
        ls
        |> List.map(stringifyNormalJson)
        |> String.concat ","
        |> sprintf "[%s]"

    | JsonValue.Null -> "null"
    | JsonValue.False -> "false"
    | JsonValue.True -> "true"
    | JsonValue.String x -> quote x
    | JsonValue.Number c -> Convert.ToString c
