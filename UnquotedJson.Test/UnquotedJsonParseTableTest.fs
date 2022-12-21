namespace UnquotedJson

open Xunit
open Xunit.Abstractions
open System
open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.Idioms
open FSharp.Literals
open FSharp.xUnit
open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime

type UnquotedJsonParseTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
    let locatePath = Path.Combine(solutionPath,@"UnquotedJson")
    let filePath = Path.Combine(locatePath, "json.fsyacc")
    let text = File.ReadAllText(filePath)
    let rawFsyacc = RawFsyaccFile.parse text
    let fsyacc = FlatFsyaccFile.fromRaw rawFsyacc

    let parseTblName = "JsonParseTable"
    let parseTblPath = Path.Combine(locatePath, $"{parseTblName}.fs")

    [<Fact>]
    member _.``02 - list all tokens``() =
        let grammar =
            fsyacc.getMainProductions()
            |> Grammar.from

        let tokens = grammar.terminals
        let res =set [",";":";"QUOTED";"UNQUOTED";"[";"]";"{";"}"]

        //show tokens
        Should.equal tokens res

    [<Fact>]
    member _.``03 - precedence Of Productions``() =
        let collection = 
            fsyacc.getMainProductions() 
            |> AmbiguousCollection.create

        let terminals = 
            collection.grammar.terminals

        let productions =
            collection.collectConflictedProductions()

        let pprods = 
            ProductionUtils.precedenceOfProductions terminals productions

        Should.equal [] pprods

    [<Fact>]
    member _.``04 - list all states``() =
        let collection =
            fsyacc.getMainProductions()
            |> AmbiguousCollection.create
        
        let text = collection.render()
        output.WriteLine(text)

    [<Fact>]
    member _.``05 - list the type annotaitions``() =
        let grammar =
            fsyacc.getMainProductions()
            |> Grammar.from

        let sourceCode =
            [
                "// Do not list symbols whose return value is always `null`"
                "// terminals: ref to the returned type of getLexeme"
                for i in grammar.terminals do
                    let i = RenderUtils.renderSymbol i
                    i + " : \"\""
                "\r\n// nonterminals"
                for i in grammar.nonterminals do
                    let i = RenderUtils.renderSymbol i
                    i + " : \"\""
            ] 
            |> String.concat "\r\n"

        output.WriteLine(sourceCode)


    [<Fact(Skip="once and for all!")>] // 
    member _.``06 - generate parsing table``() =
        let moduleName = $"UnquotedJson.{parseTblName}"

        //解析表数据
        let parseTbl = fsyacc.toFsyaccParseTableFile()
        let fsharpCode = parseTbl.generateModule(moduleName)

        File.WriteAllText(parseTblPath,fsharpCode)
        output.WriteLine("output path:"+parseTblPath)

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        let src = fsyacc.toFsyaccParseTableFile()

        Should.equal src.actions JsonParseTable.actions
        Should.equal src.closures JsonParseTable.closures

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFsyacc =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            File.ReadAllText(parseTblPath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

    [<Fact>]
    member _.``08 - format norm file test``() =
        let startSymbol = fsyacc.rules.Head |> Triple.first |> List.head
        show startSymbol
        let fsyacc = fsyacc.start(startSymbol,Set.empty).toRaw()
        output.WriteLine(fsyacc.render())

