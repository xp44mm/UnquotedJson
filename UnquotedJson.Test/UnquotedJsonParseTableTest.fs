namespace UnquotedJson

open Xunit
open Xunit.Abstractions
open System
open System.IO
open System.Text.RegularExpressions

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

    let fsyacc = FsyaccFile.parse text

    [<Fact>]
    member this.``0-产生式冲突``() =
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let pconflicts = ConflictFactory.productionConflict tbl.ambiguousTable
        //show pconflicts
        Assert.True(pconflicts.IsEmpty)

    [<Fact>]
    member this.``1-符号多用警告``() =
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let warning = ConflictFactory.overloadsWarning tbl
        //show warning
        Assert.True(warning.IsEmpty)

    [<Fact>]
    member this.``2-优先级冲突``() =
        let tbl = AmbiguousTable.create fsyacc.mainProductions
        let srconflicts = ConflictFactory.shiftReduceConflict tbl
        show srconflicts
        Assert.True(srconflicts.IsEmpty)

    [<Fact>]
    member this.``3 - print the template of type annotaitions``() =
        let grammar = Grammar.from fsyacc.mainProductions

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
    member this.``5-generate parsing table``() =
        let name = "JsonParseTable"
        let moduleName = $"UnquotedJson.{name}"

        let parseTbl = fsyacc.toFsyaccParseTable()
        //解析表数据
        let fsharpCode = parseTbl.generateParseTable(moduleName)
        let outputDir = Path.Combine(locatePath, $"{name}.fs")

        File.WriteAllText(outputDir,fsharpCode)
        output.WriteLine("output path:"+outputDir)

    [<Fact>]
    member this.``6 - valid ParseTable``() =
        let t = fsyacc.toFsyaccParseTable()

        Should.equal t.header        JsonParseTable.header
        Should.equal t.productions   JsonParseTable.productions
        Should.equal t.actions       JsonParseTable.actions
        Should.equal t.kernelSymbols JsonParseTable.kernelSymbols
        Should.equal t.semantics     JsonParseTable.semantics
        Should.equal t.declarations  JsonParseTable.declarations


