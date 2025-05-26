namespace JellyBins.Console

open FSharp.Core
open System
open System.Data
open System.Diagnostics
open System.Reflection
open JellyBins.Core
open JellyBins.Core.Factories

/// <summary>
/// Printer functions contains here
/// </summary>
module Printer =
    open System.Collections.Generic
    /// <summary>
    /// Prints dictionary as table without delimeters
    /// </summary>
    /// <param name="d"></param>
    let printDictionary (d: Dictionary<string, string>) : unit =
        for KeyValue(k, v) in d do
            printfn $"{k}\t{v}"
    /// <summary>
    /// Prints information about assembly
    /// </summary>
    let printAppVersion () : unit =
        printfn "\nJellyBins (C) CoffeeLake 2024-2025" // 2 paragraphs
        printfn "GUI-less application variant for dumping binaries"
        printfn $"  File version: %s{FileVersionInfo.GetVersionInfo(
            Assembly.GetCallingAssembly().Location).FileVersion |> string}"
        printfn $"  Product version: %s{FileVersionInfo.GetVersionInfo(
            Assembly.GetCallingAssembly().Location).ProductVersion |> string}"
        printfn ""
    /// <summary>
    /// Prints help page
    /// </summary>
    let printHelp () :unit =
        printfn "Usage: " // 2 paragraphs
        printfn "  dump [--section] [--headers] [--imports] [--exports] [--info] [--uses]"
        printfn "  dump [-s] [-h] [-it] [-et] [-i] [-u] -- Prints dump tables by keys"
        printfn "    --sections -s -> Prints object-sections table"
        printfn "    --headers  -h -> Prints object-headers table"
        printfn "    --imports -it -> Prints table of importing entries"
        printfn "    --exports -et -> Prints table of exporting entries"
        printfn "    --uses     -u -> Prints (based on dictionary) suggested API"
        printfn "    --info     -i -> Prints shorten information table"
        printfn "  info [path] -- Prints table of common dumping results"
        printfn "  version -- Prints information about this Assembly"
        printfn "  help    -- Prints this page"
    /// <summary>
    /// Prepares and prints <see cref="DataTable"/> instance
    /// as Markdown Table
    /// </summary>
    /// <param name="table"></param>
    let printDataTable (table: DataTable) =
        let safeToString (value: obj) =
            if Convert.IsDBNull(value) then
                " " // Empty cell (<null> dont need anymore)
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
                    |> Seq.map (fun row -> safeToString row[col] |> String.length)
                    |> Seq.append [headerWidth]
                    |> Seq.max
                (col.Ordinal, (contentWidth + 2))
            )
            |> Map.ofSeq

        let _formatString =
            columns
            |> Seq.map (fun col -> 
                let width = columnWidths[col.Ordinal] - 4
                sprintf "| %%-%ds " width 
            )
            |> String.concat ""
            |> fun s -> s + "|"
            
        columns
            |> Seq.map (fun col -> col.ColumnName.PadRight(columnWidths[col.Ordinal] - 2))
            |> String.concat " | "
            |> printfn "| %s |"

        columns
            |> Seq.map (fun col -> String('-', columnWidths[col.Ordinal] - 2))
            |> String.concat "-|-"
            |> printfn "|-%s-|"

        rows
            |> Seq.iter (fun row ->
                columns
                |> Seq.map (fun col ->
                    row[col]
                    |> safeToString
                    |> _.PadRight(columnWidths[col.Ordinal] - 2)
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
    open Printer
    /// <summary>
    /// Commandline arguments defines
    /// by this tree
    /// </summary>
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
        Info: bool
        Uses: bool
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
        | _ -> None
    /// <summary>
    /// <see cref="DumpOptions"/> has <c>--section</c> flag
    /// </summary>
    let (|DumpSectionsFlag|_|) = function
        | "--sections" :: rest
        | "-s" :: rest -> Some rest
        | _ -> None
    /// <summary>
    /// <see cref="DumpOptions"/> has <c>--headers</c> flag
    /// </summary>
    let (|DumpHeadersFlag|_|) = function
        | "--headers" :: rest
        | "-h" :: rest -> Some rest
        | _ -> None
    /// <summary>
    /// <see cref="DumpOptions"/> has
    /// <c>--imports flag</c>
    /// </summary>
    let (|DumpImportsFlag|_|) = function
        | "--imports" :: rest
        | "-it" :: rest -> Some rest
        | _ -> None
    /// <summary>
    /// <see cref="DumpOptions"/>
    /// has <c>--exports</c> flag
    /// </summary>
    let (|DumpExportsFlag|_|) = function
        | "--exports" :: rest
        | "-et" :: rest -> Some rest
        | _ -> None
    /// <summary>
    /// <see cref="DumpOptions"/> has <c>--info</c>
    /// flag
    /// </summary>
    let (|DumpInfo|_|) = function
        | "--info" :: rest -> Some rest
        | "-i" :: rest -> Some rest
        | _ -> None
    let (|DumpUses|_|) = function
        | "--uses" :: rest
        | "-u" :: rest -> Some rest
        | _ -> None
    /// <summary>
    /// Parses FilePath if "info" argument
    /// was called
    /// </summary>
    /// <param name="args"></param>
    [<CompiledName "ParseInfoOptions">]
    let rec parseInfoOptions args : AppCommand =
        match args with
        | filePath :: rest ->
            let opts = {
                FilePath = filePath
            }
            parseInfoOptionsRest opts rest
        | cmdList -> AppInvalid $"unknown info sequence {cmdList}"
    /// <summary>
    /// Parses reminder after "info" argument
    /// </summary>
    /// <param name="opts"><see cref="InfoOptions"/> structure</param>
    /// <param name="args">
    /// Reminder or list of arguments after "info"
    /// keyword
    /// </param>
    and parseInfoOptionsRest opts args : AppCommand =
        match args with
        | [] -> AppInfo opts
        | unexpected -> AppInvalid $"Unexpected arguments: %A{unexpected}"
    /// <summary>
    /// Makes empty <see cref="DumpOptions"/> struct
    /// with filled FilePath and tries to recognize
    /// other members (keys) which follows
    /// directly by the special key "dump"
    ///
    /// keys must have "--" prefix which tells about
    /// additional parameters for "dump" procedure
    ///
    /// Dump procedure makes full dump by default!
    /// And you tell for application, what you want to see
    /// </summary>
    /// <param name="args"></param>
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
                Uses = false
                Info = false
            }
            parseDumpOptionsRest opts rest // throws: TypeInitializer...Exception
        | _ -> AppInvalid "Missing file path"
    /// <summary>
    /// Recognizes Dump options <see cref="DumpOptions"/>
    /// arguments reminder and fills <see cref="DumpOptions"/>
    /// by read and recognized argument.
    /// </summary>
    /// <param name="opts"><see cref="DumpOptions"/> struct</param>
    /// <param name="args">reminder or list of args followed by "dump" keyword</param>
    and parseDumpOptionsRest opts args : AppCommand =
        match args with
        | DumpVerboseFlag rest -> parseDumpOptionsRest { opts with Verbose = true } rest
        | DumpHeadersFlag rest -> parseDumpOptionsRest { opts with Headers = true } rest
        | DumpSectionsFlag rest -> parseDumpOptionsRest { opts with Sections = true } rest
        | DumpImportsFlag rest -> parseDumpOptionsRest { opts with Imports = true } rest
        | DumpExportsFlag rest -> parseDumpOptionsRest { opts with Exports = true } rest
        | DumpUses rest -> parseDumpOptionsRest {opts with Uses = true } rest
        | DumpInfo rest -> parseDumpOptionsRest {opts with Info = true } rest
        | [] -> AppDump opts
        | unexpected -> AppInvalid $"Unexpected arguments: %A{unexpected}"
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
        | "dump" :: rest -> parseDumpOptions rest
        | "info" :: rest -> parseInfoOptions rest
        | ["help"] -> AppHelp
        | ["version"] -> AppVersion
        | [] -> AppHelp
        | cmdList -> AppInvalid $"Unknown command sequence: %s{cmdList |> string}"
    /// <summary>
    /// Makes <see cref="DataTable"/> instance
    /// from collection of suggested
    /// interfaces
    /// </summary>
    /// <param name="data"></param>
    let createCategoryModuleTable (data: (string * string) list) : DataTable =
        let table = new DataTable("CategoryModules")
        
        table.Columns.Add "Category" |> ignore
        table.Columns.Add "Module" |> ignore
        
        data
            |> List.iter (fun (cat, libName) ->
                let row = table.NewRow()
                row["Category"] <- cat
                row["Module"] <- libName
                table.Rows.Add(row))
        
        table
    /// <summary>
    /// Makes dump of required file
    /// Checks and executes all processed options for <see cref="AppCommand"/>
    /// <see cref="DumpOptions"/> structure
    /// </summary>
    /// <param name="d">processed <see cref="DumpOptions"/></param>
    let processDump (d: DumpOptions) : unit =
        let dumper = FileDumperFactory.CreateInstance d.FilePath
        let drawer = DrawerFactory.CreateInstance dumper
        
        let printEveryTable (segs: DataTable[]) : unit =
            segs
                |> Seq.iter printDataTable
            ()
            
        if d.Verbose then
            printfn $"Dumping file: {d.FilePath}"
            printfn $"Size:\t %d{FileInfo(d.FilePath).Length / int64 1024}K"
            printfn $"Options: {d}"
            printfn $"Segmentation: {dumper.SegmentationType}"
            
        if d.Headers then
            printEveryTable drawer.HeadersTables
        if d.Sections then
            printEveryTable drawer.SectionTables
        if d.Imports then
            printEveryTable drawer.ImportsTables
        if d.Exports then
            printEveryTable drawer.ExportsTables
        if d.Uses then
            let libArray = ModuleProcessor(dumper).TryToKnowUsedModules()
            
            libArray
                |> Array.toList
                |> List.choose (fun s -> 
                    match s.Split([| '('; ')' |], StringSplitOptions.RemoveEmptyEntries) with
                    | [| cat; libName |] -> Some (cat, libName)
                    | _ -> None)
                |> createCategoryModuleTable
                |> printDataTable
        if d.Info then
            printDictionary drawer.InfoDictionary
    /// <summary>
    /// Executes "info" procedure and prints
    /// all information about binary
    /// </summary>
    /// <param name="i"></param>
    let processInfo (i: InfoOption) : unit =
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