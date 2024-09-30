cmake_minimum_required(VERSION 3.25)
project(jbc C)

set(CMAKE_C_STANDARD 99)

add_executable(jbc main.c binaries/mapping.h fout/printing.c fout/printing.h retyping.h binaries/ibm/linearflags.h binaries/microsoft/newflags.h fout/typeinfo.c fout/typeinfo.h framework.h binaries/unix/elfmodel.h binaries/microsoft/portablemodel.h binaries/microsoft/vddmodel.h binaries/microsoft/newmodel.h binaries/ibm/linearmodel.h binaries/microsoft/portableflags.h binaries/unix/elftypes.h binaries/microsoft/mzmodel.h)
