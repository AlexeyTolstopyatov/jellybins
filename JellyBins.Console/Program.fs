namespace JellyBins.Console

open System
open System.Data
open System.Diagnostics
open System.Reflection
open JellyBins.Core.Factories

/// <summary>
/// Printer functions contains here
/// </summary>
module Printer =
    open System.Collections.Generic
    let printDictionary (d: Dictionary<string, string>) :unit =
        for KeyValue(k, v) in d do
            printfn $"{k}\t{v}"
            
    let printAppVersion () :unit =
        printfn "\nJellyBins (C) CoffeeLake 2024-2025" // 2 paragraphs
        printfn "GUI-less application variant for dumping binaries"
        printfn $"  File version: %s{FileVersionInfo.GetVersionInfo(
            Assembly.GetCallingAssembly().Location).FileVersion |> string}"
        printfn $"  Product version: %s{FileVersionInfo.GetVersionInfo(
            Assembly.GetCallingAssembly().Location).ProductVersion |> string}"
        printfn ""

    let printHelp () :unit =
        printfn "Usage: " // 2 paragraphs
        printfn "  dump [--section] [--headers] [--imports] [--exports] [--info]"
        printfn "  dump [-s] [-h] [-it] [-et] [-i] -- Prints dump tables by keys"
        printfn "  info [path] -- Prints table of common dumping results"
        printfn "  version -- Prints information about this Assembly"
        printfn "  help    -- Prints this page"
    
    let printDataTable (table: DataTable) =
        let safeToString (value: obj) =
            if Convert.IsDBNull(value) then
                "<null>"
            else
                $"%O{value}"

        let rows = table.Rows
                    |> Seq.cast<DataRow>
        let columns = table.Columns
                    |> Seq.cast<DataColumn>

        let columnWidths =
            columns
            |> Seq.map (fun col ->
                let headerWidth = col.ColumnName.Length
                let contentWidth = 
                    rows
                    |> Seq.map (fun row -> safeToString row.[col] |> String.length)
                    |> Seq.append [headerWidth]
                    |> Seq.max
                (col.Ordinal, (contentWidth + 2))
            )
            |> Map.ofSeq

        let formatString =
            columns
            |> Seq.map (fun col -> 
                let width = columnWidths.[col.Ordinal] - 4
                sprintf "| %%-%ds " width 
            )
            |> String.concat ""
            |> fun s -> s + "|"
            
        columns
            |> Seq.map (fun col -> col.ColumnName.PadRight(columnWidths.[col.Ordinal] - 2))
            |> String.concat " | "
            |> printfn "| %s |"

        columns
            |> Seq.map (fun col -> String('-', columnWidths.[col.Ordinal] - 2))
            |> String.concat "-|-"
            |> printfn "|-%s-|"

        rows
            |> Seq.iter (fun row ->
                columns
                |> Seq.map (fun col ->
                    row.[col]
                    |> safeToString
                    |> _.PadRight(columnWidths.[col.Ordinal] - 2)
                )
                |> String.concat " | "
                |> printfn "| %s |"
            )
        printfn ""
    
/// <summary>
/// Main Application logic
/// contains here
/// </summary>
module Application =
    open System.IO
    open FSharp.Core
    open Printer
    
    type AppCommand =
        | AppDump of dOptions: DumpOptions
        | AppInfo of iOptions: InfoOption
        | AppInvalid of message: String
        | AppHelp
        | AppVersion
    and DumpOptions = {
        FilePath: String
        Verbose: bool
        Sections: bool
        Imports: bool
        Exports: bool
        Headers: bool
    }
    and InfoOption = {
        FilePath: string
    }
    /// <summary>
    /// Main recognizer
    /// </summary>
    /// <param name="args"></param>
    let (|AppCommand|_|) (args: string list) =
        match args with
        | "dump" :: rest -> Some rest
        | "info" :: rest -> Some rest
        | "help" :: _ -> Some []    // empty list
        | "version" :: _ -> Some [] // empty list
        | _ -> None
    /// <summary>
    /// <see cref="DumpOptions"/> has <c>--verbose</c> flag
    /// </summary>
    let (|DumpVerboseFlag|_|) = function
        | "--verbose" :: rest
        | "-v" :: rest -> Some rest
        | rest -> None
    /// <summary>
    /// <see cref="DumpOptions"/> has <c>--section</c> flag
    /// </summary>
    let (|DumpSectionsFlag|_|) = function
        | "--sections" :: rest
        | "-s" :: rest -> Some rest
        | rest -> None
    /// <summary>
    /// <see cref="DumpOptions"/> has <c>--headers</c> flag
    /// </summary>
    let (|DumpHeadersFlag|_|) = function
        | "--headers" :: rest
        | "-h" :: rest -> Some rest
        | rest -> None
    /// <summary>
    /// <see cref="DumpOptions"/> has
    /// <c>--imports flag</c>
    /// </summary>
    let (|DumpImportsFlag|_|) = function
        | "--imports" :: rest
        | "-it" :: rest -> Some rest
        | rest -> None
    /// <summary>
    /// <see cref="DumpOptions"/>
    /// has <c>--exports</c> flag
    /// </summary>
    let (|DumpExportsFlag|_|) = function
        | "--exports" :: rest
        | "-et" :: rest -> Some rest
        | rest -> None
    /// <summary>
    /// <see cref="DumpOptions"/> has <c>--info</c>
    /// flag
    /// </summary>
    let (|DumpInfo|_|) = function
        | "--info" :: rest -> Some (true, rest)
        | "-i" :: rest -> Some (true, rest)
        | rest -> None
    
    [<CompiledName "ParseInfoOptions">]
    let rec parseInfoOptions args : AppCommand =
        match args with
        | filePath :: rest ->
            let opts = {
                FilePath = filePath
            }
            parseInfoOptionsRest opts rest
        | cmdList -> AppInvalid $"unknown info sequence {cmdList}"
    and parseInfoOptionsRest opts args : AppCommand =
        match args with
        | [] -> AppInfo opts
        | unexpected -> AppInvalid $"Unexpected arguments: %A{unexpected}"
            
    [<CompiledName "ParseDumpOptions">]
    let rec parseDumpOptions args =
        match args with
        | filePath :: rest ->
            let opts = {
                FilePath = filePath
                Verbose = false
                Headers = false
                Sections = false
                Imports = false
                Exports = false 
            }
            parseDumpOptionsRest opts rest // throws: TypeInitializer...Exception
        | _ -> AppInvalid "Missing file path"
    and parseDumpOptionsRest opts args : AppCommand =
        match args with
        | DumpVerboseFlag rest -> 
            parseDumpOptionsRest { opts with Verbose = true } rest
        | DumpHeadersFlag rest -> 
            parseDumpOptionsRest { opts with Headers = true } rest
        | DumpSectionsFlag rest -> 
            parseDumpOptionsRest { opts with Sections = true } rest
        | DumpImportsFlag rest -> 
            parseDumpOptionsRest { opts with Imports = true } rest
        | DumpExportsFlag rest -> 
            parseDumpOptionsRest { opts with Exports = true } rest
        | [] -> AppDump opts
        | unexpected -> 
            AppInvalid $"Unexpected arguments: %A{unexpected}"

    /// <summary>
    /// Translates items of <c>argv</c> to F# Atoms
    /// </summary>
    /// <param name="argv">Vector of Command line arguments</param>
    [<CompiledName "ParseCommand">]
    let parseCommand (argv: string[]) : AppCommand =
        let args = argv
                   |> Array.toList
        match args with
        | ["dump"] -> AppInvalid "Missing file path for dump command"
        | ("dump" :: rest) -> parseDumpOptions rest
        | ("info" :: rest) -> parseInfoOptions rest
        | ["help"] -> AppHelp
        | ["version"] -> AppVersion
        | [] -> AppHelp
        | cmdList -> AppInvalid $"Unknown command sequence: %s{cmdList |> string}"
    
    let processDump (d: DumpOptions) : unit =
        if d.Verbose then
            printfn $"Dumping file: {d.FilePath}"
            printfn $"Size:\t %d{FileInfo(d.FilePath).Length * int64 1024}K"
            printfn $"Options {d}"
        
        let dumper = FileDumperFactory.CreateInstance d.FilePath
        let drawer = DrawerFactory.CreateInstance dumper
        
        let printEveryTable (segs: DataTable[]) : unit =
            segs
                |> Seq.iter (fun dt -> printDataTable dt)
            ()
            
        if d.Headers then
            printEveryTable drawer.HeadersTables
        if d.Sections then
            printEveryTable drawer.SectionTables
        if d.Imports then
            printEveryTable drawer.ImportsTables
        if d.Exports then
            printEveryTable drawer.ExportsTables
            
        ()
    let processInfo (i: InfoOption) : unit =
        // verbose option missing
        let dumper = FileDumperFactory.CreateInstance i.FilePath
        let drawer = DrawerFactory.CreateInstance dumper
            
        printDictionary drawer.InfoDictionary
        printfn ""
    /// <summary>
    /// Executes translated command
    /// </summary>
    /// <param name="command"></param>
    [<CompiledName "HandleCommand">]
    let rec handleCommand (command: AppCommand) : unit =
        match command with
        | AppHelp ->
            printHelp ()
            exit 0 
        | AppVersion ->
            printAppVersion ()
            exit 0
        | AppDump opts ->
            processDump opts
            exit 0
        | AppInfo opts ->
            processInfo opts
        | AppInvalid msg ->
            printfn $"Error: {msg}\n"
    
    [<EntryPoint>]
    let main argv =
        argv
        |> parseCommand
        |> handleCommand
        0