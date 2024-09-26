cmake_minimum_required(VERSION 3.25)
project(jbc C)

set(CMAKE_C_STANDARD 99)

add_executable(jbc main.c binaries/mapping.h fout/printing.c fout/printing.h retyping.h binaries/lxflags.h binaries/neflags.h fout/typeinfo.c fout/typeinfo.h framework.h)
