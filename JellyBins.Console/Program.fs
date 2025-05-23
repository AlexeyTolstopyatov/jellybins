module JellyBins.Console
    open System
    open System.Diagnostics
    open System.Reflection
    type AppCommand =
        | AppDump of options: DumpOptions
        | AppHelp
        | AppVersion
        | AppInvalidCommand of message: String
    and DumpOptions = {
        FilePath: String
        Verbose: bool
        Sections: List<String>
    }
    let (|AppCommand|_|) (args: string list) =
        match args with
        | "dump" :: rest -> Some rest
        | "help" :: _ -> Some []
        | "version" :: _ -> Some []
        | _ -> None
    let (|VerboseFlag|_|) = function
        | "--verbose" :: rest -> Some (true, rest)
        | rest -> Some (false, rest)

    let (|SectionsFlag|_|) = function
        | "--sections" :: sections :: rest ->
            Some (sections.Split(',') |> List.ofArray, rest)
        | rest -> Some ([], rest)
    [<CompiledName "ParseDumpOptions">]
    let rec parseDumpOptions args =
        match args with
        | filePath :: rest ->
            let opts = {
                FilePath = filePath
                Verbose = false
                Sections = []
            }
            parseDumpOptionsRest opts rest
        | _ -> AppInvalidCommand "Missing file path"
    and parseDumpOptionsRest opts args =
        match args with
        | VerboseFlag (verbose, rest) ->
            parseDumpOptionsRest { opts with Verbose = verbose } rest
        | SectionsFlag (sections, rest) ->
            parseDumpOptionsRest { opts with Sections = sections } rest
        | [] -> AppDump opts
        | unexpected -> AppInvalidCommand $"Unexpected arguments: %A{unexpected}"

    [<CompiledName "ParseCommand">]
    let parseCommand (argv: string[]) =
        let args = argv
                   |> List.ofArray
        
        match args with
        | AppCommand ["dump"] -> AppInvalidCommand "Missing file path for dump command"
        | AppCommand ("dump" :: rest) -> parseDumpOptions rest
        | AppCommand ["help"] -> AppHelp
        | AppCommand ["version"] -> AppVersion
        | [] -> AppHelp  // Если нет аргументов - показываем помощь
        | _ -> AppInvalidCommand "Unknown command"
    [<CompiledName "HandleCommand">]
    let rec handleCommand (command: AppCommand) =
        match command with
        | AppDump opts ->
            printfn $"Dumping file: {opts.FilePath}"
            printfn $"Verbose: {opts.Verbose}"
            printfn $"Sections: {opts.Sections}"
            // DumperFactory.CreateInstance(opts.FilePath).Dump()
        | AppHelp ->
            printfn "Usage:"
            printfn "  dump <file> [--format json|text|md] [--verbose] [--sections section1,section2]"
            printfn "  help"
            printfn "  version"
        | AppVersion ->
            printfn $"JellyBins {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().FullName)}"
        | AppInvalidCommand msg ->
            printfn $"Error: {msg}"
            handleCommand AppHelp
    
    [<EntryPoint>]
    let main argv =
        argv
        |> parseCommand
        |> handleCommand
        0