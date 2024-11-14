cmake_minimum_required(VERSION 3.25)
project(jbc C)

set(CMAKE_C_STANDARD 99)

add_executable(jbc main.c Format/printing.c Format/printing.h LinearExecutable/ModuleFlags.h NewExecutable/ProgramFlags.h Format/Table.c Format/Table.h framework.h LinkableExecutable/ELFHeader.h PortableExecutable/PEHeader.h NewExecutable/NEHeader.h LinearExecutable/LEHeader.h PortableExecutable/Characteristics.h LinkableExecutable/ELFTypes MarkExecutable/MZHeader.h
        LinearExecutable/ModuleFlags.c
        NewExecutable/ProgramFlags.c
        Factory/ExecutableReader.c
        Factory/ExecutableReader.h
        Factory/ExecutableAnalyser.c
        Factory/ExecutableAnalyser.h)
