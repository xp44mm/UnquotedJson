namespace UnquotedJson

open Xunit
open Xunit.Abstractions
open System
open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime

type UnquotedJsonParseTableTest(output:ITestOutputHelper) =
    let parseTblName = "JsonParseTable"
    let parseTblModule = $"UnquotedJson.{parseTblName}"
    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
    let locatePath = Path.Combine(solutionPath,@"UnquotedJson")
    let filePath = Path.Combine(locatePath, "json.fsyacc")
    let parseTblPath = Path.Combine(locatePath, $"{parseTblName}.fs")

    let show res =
        res
        |> stringify
        |> output.WriteLine
        
    let text = File.ReadAllText(filePath,Encoding.UTF8)

    let fsyaccCrew =
        text
        |> RawFsyaccFileCrewUtils.parse
        |> FlatedFsyaccFileCrewUtils.fromRawFsyaccFileCrew

    let inputProductionList =
        fsyaccCrew.flatedRules
        |> List.map Triple.first

    let collectionCrew = 
        inputProductionList
        |> AmbiguousCollectionCrewUtils.newAmbiguousCollectionCrew

    let tblCrew =
        fsyaccCrew
        |> FlatedFsyaccFileCrewUtils.getSemanticParseTableCrew

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let s0 = tblCrew.startSymbol
        let flatedFsyacc =
            fsyaccCrew
            |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

        let src = 
            flatedFsyacc 
            |> FlatFsyaccFileUtils.start s0
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)

    [<Fact>]
    member _.``02 - list all tokens``() =
        let y = set [",";":";"QUOTED";"UNQUOTED";"[";"]";"{";"}"]
        let tokens = tblCrew.symbols - tblCrew.nonterminals
        show tokens
        Should.equal y tokens
        
    [<Fact>]
    member _.``03 - list all states``() =
        let text = 
            AmbiguousCollectionUtils.render 
                tblCrew.terminals
                tblCrew.conflictedItemCores
                (tblCrew.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq)

        output.WriteLine(text)

    [<Fact>]
    member _.``04 - precedence Of Productions``() =
        let productions = 
            AmbiguousCollectionUtils.collectConflictedProductions tblCrew.conflictedItemCores

        // production -> %prec
        let pprods =
            ProductionSetUtils.precedenceOfProductions tblCrew.terminals productions

        //优先级应该据此结果给出，不能少，也不应该多。
        let y = []

        Should.equal y pprods

    [<Fact>]
    member _.``05 - list declarations``() =
        let terminals =
            tblCrew.terminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let nonterminals =
            tblCrew.nonterminals
            |> Seq.map RenderUtils.renderSymbol
            |> String.concat " "

        let src =
            [
                "// Do not list symbols whose return value is always `null`"
                ""
                "// terminals: ref to the returned type of `getLexeme`"
                "%type<> " + terminals
                ""
                "// nonterminals"
                "%type<> " + nonterminals
            ] 
            |> String.concat "\r\n"

        output.WriteLine(src)

    [<Fact(
    Skip="按需更新源代码"
    )>]
    member _.``06 - generate ParseTable``() =
        let src =
            tblCrew
            |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew
            |> FsyaccParseTableFileUtils.generateModule(parseTblModule)

        File.WriteAllText(parseTblPath, src, Encoding.UTF8)
        output.WriteLine($"output yacc:\r\n{parseTblPath}")

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        Should.equal tblCrew.encodedActions  JsonParseTable.actions
        Should.equal tblCrew.encodedClosures JsonParseTable.closures

        //产生式比较
        let prodsFsyacc = 
            List.map fst tblCrew.rules

        let prodsParseTable = 
            List.map fst JsonParseTable.rules

        Should.equal prodsFsyacc prodsParseTable

        //header,semantic代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",tblCrew.header)

        let semansFsyacc =
            let mappers = 
                tblCrew
                |> FsyaccParseTableFileUtils.ofSemanticParseTableCrew
                |> FsyaccParseTableFileUtils.generateMappers
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

