module UnquotedJson.ComponentRecordJson

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal
open FSharp.xUnit

open System.IO
open System.Text


///注意：使用前需要确保对象具有path属性
let has (collection:Json list) (node:Json) =
    let path0 = node.["path"].stringText
    collection
    |> Seq.exists(fun e ->
        let path1 = e.["path"].stringText
        path0 = path1
    )

/// 判断一个节点是否可以完全展开
let rec canExpand (collection:Json list) (node:Json) =
    if node.hasProperty "path" then
        has collection node
    else
        if node.hasProperty "children" then
            // 每个孩子可以expand
            node.["children"].elements
            |> List.forall(fun child ->
                canExpand collection child
            )
        else true

/// 不能使用替换，而是使用合并
let rec merge (collection:Json list) (node:Json) =
    if node.hasProperty "path" then
        node.coalesce(
            collection
            |> Seq.find(fun e -> node.["path"].stringText = e.["path"].stringText)
        )
    else //没有path
        if node.hasProperty "children" then
            // expand每个孩子
            let children =
                node.["children"].elements
                |> List.map(fun child ->
                    merge collection child
                )
            node.setProperty("children", Json.Array children)
        else // 没有path，没有children 
            node

let tryExpanding (expanded:Json list) (node:Json) =
    // 所有的零件已经放入了expanded了，剩下的都是asm
    if (
        node.["children"].elements
        |> List.forall(fun child ->
            canExpand expanded child
        )
    ) then
        let children =
            node.["children"].elements
            |> List.map(fun child ->
                merge expanded child
            )
        node.setProperty("children", Json.Array children)
        |> Some
    else None

let rec toExpand (expanded:Json list) (original:Json list) =
    let expanding =
        original
        |> List.choose(fun org ->
             tryExpanding expanded org
        )

    let original =
        original 
        |> List.filter(fun org -> has expanding org |> not)

    match original with
    | [] -> expanded @ expanding
    | _ ->
        match expanding with
        | [] -> 
            let outp =
                original
                |> List.map(Json.print)
                |> String.concat ";"
                |> sprintf "[%s]"
            failwith $"{outp}"
        | _ -> toExpand (expanded @ expanding) original
