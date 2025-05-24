module JellyBins.Console
    open System
    open System.Diagnostics
    open System.IO
    open System.Reflection
    open FSharp.Core
    open JellyBins.Abstractions
    open JellyBins.Core.Factories
    open JellyBins.NewExecutable.Models
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
    /// 
    /// </summary>
    /// <param name="args"></param>
    let (|AppCommand|_|) (args: string list) =
        match args with
        | "dump" :: rest -> Some rest
        | "help" :: _ -> Some []    // empty list
        | "version" :: _ -> Some [] // empty list
        | "info" :: _ -> Some []    // empty list
        | _ -> None
    /// <summary>
    /// <see cref="AppCommand"/> has <c>--verbose</c> flag
    /// </summary>
    let (|VerboseFlag|_|) = function
        | "--verbose" :: rest -> Some (true, rest)
        | "-v" :: rest -> Some (true, rest)
        | rest -> Some (false, rest)
    /// <summary>
    /// <see cref="AppCommand"/> has <c>--section</c> flag
    /// </summary>
    let (|SectionsFlag|) = function
        | "--sections" -> Some true
        | "-s" -> Some true
        | _ -> Some false
    let (|ImportsFlag|) = function
        | "--imports" -> Some true
        | "-it" -> Some true
        | _ -> Some false
    let (|ExportsFlag|) = function
        | "--exports" -> Some true
        | "-et" -> Some true
        | _ -> Some false
    let (|Info|) = function
        | "-i" -> Some true
        | _ -> Some false
        
    [<CompiledName "ParseDumpOptions">]
    let rec parseDumpOptions args =
        match args with
        | filePath :: rest ->
            let opts = {
                // declare ALL available options
                FilePath = filePath
                Verbose = false
                Sections = false
                Imports = false
                Exports = false
                Headers = false 
            }
            parseDumpOptionsRest opts rest
        | _ -> AppInvalid "Missing file path"
    and parseDumpOptionsRest opts args =
        match args with
        | VerboseFlag (verbose, rest) ->
            parseDumpOptionsRest {
                opts with Verbose = verbose
            } rest
     // | Sections idea missed
        | [] -> AppDump opts
        | unexpected -> AppInvalid $"Unexpected arguments: %A{unexpected}"

    /// <summary>
    /// Translates items of <c>argv</c> to F# Atoms
    /// </summary>
    /// <param name="argv">Vector of Command line arguments</param>
    [<CompiledName "ParseCommand">]
    let parseCommand (argv: string[]) =
        let args = argv
                   |> Array.toList
        match args with
        | ["dump"] -> AppInvalid "Missing file path for dump command"
        | ("dump" :: rest) -> parseDumpOptions rest
        | ["help"] -> AppHelp
        | ["version"] -> AppVersion
        | [] -> AppHelp
        | cmdList -> AppInvalid $"Unknown command sequence: %s{cmdList |> string}"
    
    let printDump (d: DumpOptions) =
        if d.Verbose then
            printfn $"Dumping file: {d.FilePath}"
            printfn $"Size:\t %d{FileInfo(d.FilePath).Length * int64 1024}K"
        // get ready for totally dumping
        // let dumper = FileDumperFactory.CreateInstance d.FilePath
        // match dumper.SegmentationType with
        //     | FileSegmentationType.NewExecutable ->
        //         dumper.Dump()
        //         // C# part for tabes drawing (saving segmentation as Dictionaries)
        //     | _ -> AppInvalid "Unknown binary (NotSupportedException)"
        //            |> ignore
        //            
        ()
    
    /// <summary>
    /// Executes translated command
    /// </summary>
    /// <param name="command"></param>
    [<CompiledName "HandleCommand">]
    let rec handleCommand (command: AppCommand) =
        match command with
        | AppHelp ->
            printfn "Usage: " // 2 paragraphs
            printfn "  dump [--section] [--headers] [--imports] [--exports] [--info]"
            printfn "  dump [-s] [-h] [-it] [-et] [-i]"
            printfn "  version -- Prints information about this Assembly"
            printfn "  help    -- Prints this page"
            printfn "  info    -- Prints table of common dumping results"
            exit(0) 
        | AppVersion ->
            printfn "\nJellyBins (C) CoffeeLake 2024-2025" // 2 paragraphs
            printfn "GUI-less application variant for dumping binaries"
            printfn $"  File version: %s{FileVersionInfo.GetVersionInfo(
                Assembly.GetCallingAssembly().Location).FileVersion |> string}"
            printfn $"  Product version: %s{FileVersionInfo.GetVersionInfo(
                Assembly.GetCallingAssembly().Location).ProductVersion |> string}"
            printfn ""
            exit(0)
        | AppDump opts ->
            printDump opts
            exit(0)
        | AppInfo opts ->
            handleCommand AppHelp
        | AppInvalid msg ->
            printfn $"Error: {msg}\n"
    
    [<EntryPoint>]
    let main argv =
        argv
        |> parseCommand
        |> handleCommand
        0