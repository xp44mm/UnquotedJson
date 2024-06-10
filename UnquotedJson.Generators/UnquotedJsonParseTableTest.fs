namespace UnquotedJson
open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit
open FslexFsyacc
open FslexFsyacc.Fsyacc
open FslexFsyacc.Precedences
open FslexFsyacc.YACCs
open System.IO
open System.Text
open Xunit
open Xunit.Abstractions

type UnquotedJsonParseTableTest(output:ITestOutputHelper) =
    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName

    let parseTblName = "JsonParseTable"
    let parseTblModule = $"UnquotedJson.{parseTblName}"
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "json.fsyacc")

    let locatePath = Path.Combine(solutionPath,@"UnquotedJson")
    let parseTblPath = Path.Combine(locatePath, $"{parseTblName}.fs")

    let show res =
        res
        |> stringify
        |> output.WriteLine
        
    let text = File.ReadAllText(filePath, Encoding.UTF8)

    let rawFsyacc =
        text
        |> FsyaccCompiler2.compile

    let fsyacc =
        rawFsyacc
        |> FlatFsyaccFile.from

    let coder = FsyaccParseTableCoder.from fsyacc

    let tbl =
        fsyacc.getYacc()

    let bnf = tbl.bnf

    //[<Fact>]
    //member _.``01 - norm fsyacc file``() =
    //    let s0 = tblCrew.startSymbol
    //    let flatedFsyacc =
    //        fsyaccCrew
    //        |> FlatedFsyaccFileCrewUtils.toFlatFsyaccFile

    //    let src = 
    //        flatedFsyacc 
    //        |> FlatFsyaccFileUtils.start s0
    //        |> RawFsyaccFileUtils.fromFlat
    //        |> RawFsyaccFileUtils.render

    //    output.WriteLine(src)

    //[<Fact>]
    //member _.``02 - list all tokens``() =
    //    let y = set [",";":";"QUOTED";"UNQUOTED";"[";"]";"{";"}"]
    //    let tokens = tblCrew.symbols - tblCrew.nonterminals
    //    show tokens
    //    Should.equal y tokens
        
    //[<Fact>]
    //member _.``03 - list all states``() =
    //    let text = 
    //        AmbiguousCollectionUtils.render 
    //            tblCrew.terminals
    //            tblCrew.conflictedItemCores
    //            (tblCrew.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq)

    //    output.WriteLine(text)

    [<Fact>]
    member _.``04 - precedence Of Productions``() =
        //优先级应该据此结果给出，不能少，也不应该多。
        let st = ConflictedProduction.from fsyacc.rules
        for cp in st do
        output.WriteLine($"{stringify cp}")

    //[<Fact>]
    //member _.``05 - list declarations``() =
    //    let terminals =
    //        tblCrew.terminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let nonterminals =
    //        tblCrew.nonterminals
    //        |> Seq.map RenderUtils.renderSymbol
    //        |> String.concat " "

    //    let src =
    //        [
    //            "// Do not list symbols whose return value is always `null`"
    //            ""
    //            "// terminals: ref to the returned type of `getLexeme`"
    //            "%type<> " + terminals
    //            ""
    //            "// nonterminals"
    //            "%type<> " + nonterminals
    //        ] 
    //        |> String.concat "\r\n"

    //    output.WriteLine(src)

    [<Fact(
    //Skip="按需更新源代码"
    )>]
    member _.``06 - generate ParseTable``() =
        let outp = coder.generateModule(parseTblModule)
        File.WriteAllText(parseTblPath, outp, Encoding.UTF8)
        output.WriteLine("output yacc:")
        output.WriteLine($"{parseTblPath}")

    [<Fact>]
    member _.``07 - valid ParseTable``() =
        Should.equal coder.tokens JsonParseTable.tokens
        Should.equal coder.kernels JsonParseTable.kernels
        Should.equal coder.actions JsonParseTable.actions

        //产生式比较
        let prodsFsyacc =
            fsyacc.rules
            |> Seq.map (fun rule -> rule.production)
            |> Seq.toList

        let prodsParseTable =
            JsonParseTable.rules
            |> List.map fst

        Should.equal prodsFsyacc prodsParseTable

        //header,semantic代码比较
        let headerFromFsyacc =
            FSharp.Compiler.SyntaxTreeX.Parser.getDecls("header.fsx",fsyacc.header)

        let semansFsyacc =
            let mappers = coder.generateMappers()
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.semansFromMappers mappers

        let header,semans =
            let text = File.ReadAllText(parseTblPath, Encoding.UTF8)
            FSharp.Compiler.SyntaxTreeX.SourceCodeParser.getHeaderSemansFromFSharp 4 text

        Should.equal headerFromFsyacc header
        Should.equal semansFsyacc semans

