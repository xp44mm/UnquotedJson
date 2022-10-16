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

    [<Fact>]
    member _.``001 - 显示冲突状态的冲突项目``() =
        let collection =
            AmbiguousCollection.create <| fsyacc.getMainProductions()
        let conflicts =
            collection.filterConflictedClosures()
        //Should.equal y conflicts
        show conflicts


    [<Fact>]
    member _.``002 - 汇总冲突的产生式``() =
        let collection =
            AmbiguousCollection.create  <| fsyacc.getMainProductions()
        let conflicts =
            collection.filterConflictedClosures()

        let productions =
            AmbiguousCollection.gatherProductions conflicts
        // production -> %prec
        let pprods =
            ProductionUtils.precedenceOfProductions collection.grammar.terminals productions
        //优先级应该据此结果给出，不能少，也不应该多。
        let y = [
            ]

        Should.equal y pprods


    [<Fact>]
    member _.``003 - print the template of type annotaitions``() =
        let grammar = Grammar.from  <| fsyacc.getMainProductions()

        let symbols = 
            grammar.symbols 
            |> Set.filter(fun x -> Regex.IsMatch(x,@"^\w+$"))

        let sourceCode = 
            [
                for i in symbols do
                    i + " \"\";"
            ] |> String.concat "\r\n"
        output.WriteLine(sourceCode)

    [<Fact(Skip="once and for all!")>] // 
    member _.``005 - generate parsing table``() =
        let name = "JsonParseTable"
        let moduleName = $"UnquotedJson.{name}"

        let parseTbl = fsyacc.toFsyaccParseTableFile()
        //解析表数据
        let fsharpCode = parseTbl.generateModule(moduleName)
        let outputDir = Path.Combine(locatePath, $"{name}.fs")

        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output path:"+outputDir)

    [<Fact>]
    member _.``009 - valid ParseTable``() =
        let src = fsyacc.toFsyaccParseTableFile()

        Should.equal src.actions JsonParseTable.actions
        Should.equal src.closures JsonParseTable.closures

        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",src.header)

        let semansFsyacc =
            let mappers = src.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let filePath = Path.Combine(locatePath, "JsonParseTable.fs")
            File.ReadAllText(filePath, Encoding.UTF8)
            |> FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 2

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

    [<Fact>]
    member _.``101 - format norm file test``() =
        let startSymbol = fsyacc.rules.Head |> Triple.first |> List.head
        show startSymbol
        let fsyacc = fsyacc.start(startSymbol,Set.empty).toRaw()
        output.WriteLine(fsyacc.render())

