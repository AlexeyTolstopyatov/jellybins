# JellyBins.Console (`jbc`)

<img align="right" src="JellyBins.Assets/beans128.png">
Is a GUI-less application variant built for
binary static analysis. using F#

 Works correctly only with Low Endian segmented binaries

### Version

<img src="JellyBins.Assets/version.png">

### Sections

```

PS D:\GitHub\JellyBins\JellyBins.Console\bin\Debug\net8.0> ./JellyBins.Console.exe dump "D:\Анализ файлов\inst\PE\acpi.sys" --sections
| Name                                                                    | Address | Size |
|-------------------------------------------------------------------------|---------|------|
| PE Data Directory (JellyBins: IMAGE_EXPORT_DIRECTORY)                   |         | 8    |
| PE Data Directory (JellyBins: IMAGE_IMPORT_DIRECTORY)                   |         | 8    |
| PE Data Directory (JellyBins: IMAGE_RESOURCE_DIRECTORY)                 |         | 8    |
| PE Data Directory (JellyBins: IMAGE_EXCEPTION_DIRECTORY)                |         | 8    |
| PE Data Directory (JellyBins: IMAGE_CERTIFICATION_DIRECTORY)            |         | 8    |
| PE Data Directory (JellyBins: IMAGE_BASERELOCS_DIRECTORY)               |         | 8    |
| PE Data Directory (JellyBins: IMAGE_DEBUG_DIRECTORY)                    |         | 8    |
| PE Data Directory (JellyBins: IMAGE_ARCHITECTURE_DIRECTORY)             |         | 8    |
| PE Data Directory (JellyBins: IMAGE_GLOBALPTR_DIRECTORY)                |         | 8    |
| PE Data Directory (JellyBins: IMAGE_TLS_DIRECTORY)                      |         | 8    |
| PE Data Directory (JellyBins: IMAGE_LOADCONFIG_DIRECTORY)               |         | 8    |
| PE Data Directory (JellyBins: IMAGE_BOUNDIMPORT_DIRECTORY)              |         | 8    |
| PE Data Directory (JellyBins: IMAGE_IAT_DIRECTORY)                      |         | 8    |
| PE Data Directory (JellyBins: IMAGE_DELAY_IMPORT_DESCRIPTORS_DIRECTORY) |         | 8    |
| PE Data Directory (JellyBins: IMAGE_COM_DIRECTORY)                      |         | 8    |
| PE Data Directory (JellyBins: IMAGE_RESERVED_DIRECTORY)                 |         | 8    |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 504     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 544     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 584     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 624     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 664     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 704     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 744     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 784     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 824     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 864     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 904     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 944     | 40   |
| PE Section (WinAPI: IMAGE_SECTION_HEADER)                               | 984     | 40   |

| VirtualAddress | Size |
|----------------|------|
| A5000          | 77   |
| 77950          | 78   |
| AE000          | 4498 |
| 71000          | 5520 |
| B6000          | 2580 |
| B3000          | CA8  |
| 62350          | 70   |
| 0              | 0    |
| 0              | 0    |
| 0              | 0    |
| 600A0          | 140  |
| 0              | 0    |
| 77000          | 920  |
| 0              | 0    |
| 0              | 0    |
| 0              | 0    |

| Name     | VirtualAddress | VirtualSize | SizeOfRawData | PointerToRawData | PointerToRelocs | PointerToLine# | #Relocs | #LineNumbers | Characteristics |
|----------|----------------|-------------|---------------|------------------|-----------------|----------------|---------|--------------|-----------------|
| .text    | 1000           | 5E06C       | 5F000         | 1000             | 0               | 0              | 0       | 0            | 68000020        |
| .rdata   | 60000          | B180        | C000          | 60000            | 0               | 0              | 0       | 0            | 48000040        |
| .data    | 6C000          | 4A28        | 3000          | 6C000            | 0               | 0              | 0       | 0            | C8000040        |
| .pdata   | 71000          | 5520        | 6000          | 6F000            | 0               | 0              | 0       | 0            | 48000040        |
| .idata   | 77000          | 2E0A        | 3000          | 75000            | 0               | 0              | 0       | 0            | 48000040        |
| PAGE     | 7A000          | 2934F       | 2A000         | 78000            | 0               | 0              | 0       | 0            | 60000020        |
| fothk    | A4000          | 1000        | 1000          | A2000            | 0               | 0              | 0       | 0            | 60000020        |
| .edata   | A5000          | 77          | 1000          | A3000            | 0               | 0              | 0       | 0            | 40000040        |
| PAGECONS | A6000          | BE0         | 1000          | A4000            | 0               | 0              | 0       | 0            | 40000040        |
| INIT     | A7000          | 5640        | 6000          | A5000            | 0               | 0              | 0       | 0            | 62000020        |
| GFIDS    | AD000          | BD8         | 1000          | AB000            | 0               | 0              | 0       | 0            | 42000040        |
| .rsrc    | AE000          | 4498        | 5000          | AC000            | 0               | 0              | 0       | 0            | 42000040        |
| .reloc   | B3000          | 4EA4        | 5000          | B1000            | 0               | 0              | 0       | 0            | 42000040        |
```

### Exports

```powershell
PS D:\GitHub\JellyBins\JellyBins.Console\bin\Debug\net8.0> ./JellyBins.Console.exe dump "D:\Анализ файлов\inst\NE\GDI.EXE" --exports 
```
| #  | Name                                        | Ordinal |
|----|---------------------------------------------|---------|
| 43 | Microsoft Windows Graphics Device Interface | @0      |
| 14 | GETWINDOWEXTEX                              | @474    |
| 10 | COMBINERGN                                  | @47     |
| 14 | SETWINDOWORGEX                              | @482    |
| 16 | SETVIEWPORTORGEX                            | @480    |
| 10 | QUERYABORT                                  | @155    |
| 16 | ENGINEDELETEFONT                            | @301    |
| 9  | EXTRACTPQ                                   | @232    |
| 11 | GETBRUSHORG                                 | @149    |
| 16 | DEVICECOLORMATCH                            | @449    |
| 7  | TEXTOUT                                     | @33     |
| 14 | SETENVIRONMENT                              | @132    |
| 7  | RESETDC                                     | @376    |
| 17 | SETSTRETCHBLTMODE                           | @7      |
| 16 | CREATEHATCHBRUSH                            | @58     |
| 20 | ENGINESETFONTCONTEXT                        | @304    |
| 6  | PATBLT                                      | @29     |
| 14 | SETWINDOWEXTEX                              | @481    |
| 13 | GETBOUNDSRECT                               | @194    |
| 6  | BITBLT                                      | @34     |
| 9  | STARTPAGE                                   | @379    |
| 15 | OFFSETWINDOWORG                             | @15     |
| 5  | CHORD                                       | @348    |
| 11 | SETBRUSHORG                                 | @148    |
| 8  | CREATEDC                                    | @53     |
| 7  | ELLIPSE                                     | @24     |
| 18 | GETBITMAPDIMENSION                          | @162    |
| 10 | PTINREGION                                  | @161    |
| 16 | GETVIEWPORTEXTEX                            | @472    |
| 14 | CREATEDIBITMAP                              | @442    |
| 21 | CREATEDIBPATTERNBRUSH                       | @445    |
| 6  | MULDIV                                      | @128    |
| 15 | GETPOLYFILLMODE                             | @84     |
| 15 | GETNEARESTCOLOR                             | @154    |
| 16 | RECTINREGION_EHH                            | @466    |
| 13 | GETCURLOGFONT                               | @411    |
| 8  | DELETEDC                                    | @68     |
| 9  | DELETEJOB                                   | @244    |
| 12 | COPYMETAFILE                                | @151    |
| 21 | GETTEXTCHARACTEREXTRA                       | @89     |
| 11 | GETMETAFILE                                 | @124    |
| 12 | OFFSETVISRGN                                | @102    |
| 12 | CREATEBITMAP                                | @48     |
| 10 | GETMAPMODE                                  | @81     |
| 9  | INVERTRGN                                   | @42     |
| 14 | DMGETCHARWIDTH                              | @215    |
| 13 | SETBOUNDSRECT                               | @193    |
| 7  | FILLRGN                                     | @40     |
| 17 | INTERSECTCLIPRECT                           | @22     |
| 19 | OFFSETVIEWPORTORGEX                         | @476    |
| 8  | INSERTPQ                                    | @233    |
| 8  | DMOUTPUT                                    | @208    |
| 6  | LPTODP                                      | @99     |
| 6  | DPTOLP                                      | @67     |
| 12 | GETCHARWIDTH                                | @350    |
| 18 | SETBITMAPDIMENSION                          | @163    |
| 18 | REMOVEFONTRESOURCE                          | @136    |
| 18 | SCALEVIEWPORTEXTEX                          | @484    |
| 16 | SETVIEWPORTEXTEX                            | @479    |
| 13 | RESIZEPALETTE                               | @368    |
| 18 | PLAYMETAFILERECORD                          | @176    |
| 12 | RESURRECTION                                | @122    |
| 15 | SETPOLYFILLMODE                             | @6      |
| 10 | EXTTEXTOUT                                  | @351    |
| 12 | DMSTRETCHBLT                                | @216    |
| 11 | RECTVISIBLE                                 | @104    |
| 20 | GETASPECTRATIOFILTER                        | @353    |
| 13 | INQUIREVISRGN                               | @131    |
| 12 | GETWINDOWORG                                | @97     |
| 16 | CREATEPOLYGONRGN                            | @63     |
| 12 | DMEXTTEXTOUT                                | @214    |
| 8  | DMBITBLT                                    | @201    |
| 11 | ISGDIOBJECT                                 | @462    |
| 21 | SETTEXTCHARACTEREXTRA                       | @8      |
| 5  | MINPQ                                       | @231    |
| 26 | CREATESCALABLEFONTRESOURCE                  | @310    |
| 8  | CREATEIC                                    | @153    |
| 13 | RESTOREVISRGN                               | @130    |
| 10 | SETMAPMODE                                  | @3      |
| 12 | ENUMMETAFILE                                | @175    |
| 9  | GETRELABS                                   | @86     |
| 14 | GETVIEWPORTORG                              | @95     |
| 8  | FRAMERGN                                    | @41     |
| 16 | ENUMFONTFAMILIES                            | @330    |
| 18 | ENGINEGETCHARWIDTH                          | @303    |
| 20 | CREATEBITMAPINDIRECT                        | @49     |
| 21 | GETOUTLINETEXTMETRICS                       | @308    |
| 11 | WRITEDIALOG                                 | @242    |
| 17 | SETDIBITSTODEVICE                           | @443    |
| 12 | SELECTVISRGN                                | @105    |
| 12 | GETTEXTALIGN                                | @345    |
| 7  | DMPIXEL                                     | @209    |
| 16 | SCALEWINDOWEXTEX                            | @485    |
| 12 | GETWINDOWEXT                                | @96     |
| 6  | MOVETO                                      | @20     |
| 15 | DMSTRETCHDIBITS                             | @218    |
| 12 | SETWINDOWORG                                | @11     |
| 9  | FLOODFILL                                   | @25     |
| 16 | INTERSECTVISRECT                            | @98     |
| 9  | GETOBJECT                                   | @82     |
| 10 | GETBKCOLOR                                  | @75     |
| 16 | ENGINEEXTTEXTOUT                            | @314    |
| 14 | GETSTOCKOBJECT                              | @87     |
| 13 | GETBRUSHORGEX                               | @469    |
| 22 | CREATECOMPATIBLEBITMAP                      | @51     |
| 9  | CREATEPEN                                   | @61     |
| 9  | SETRELABS                                   | @5      |
| 13 | CLOSEMETAFILE                               | @126    |
| 14 | GETTEXTMETRICS                              | @93     |
| 16 | CREATESOLIDBRUSH                            | @66     |
| 14 | SETVIEWPORTORG                              | @13     |
| 22 | CONVERTOUTLINEFONTFILE                      | @312    |
| 10 | GETCLIPRGN                                  | @173    |
| 25 | CREATEELLIPTICRGNINDIRECT                   | @55     |
| 12 | SETTEXTALIGN                                | @346    |
| 9  | DMENUMOBJ                                   | @207    |
| 12 | EXTFLOODFILL                                | @372    |
| 15 | RECTVISIBLE_EHH                             | @465    |
| 8  | GDIINIT2                                    | @403    |
| 6  | SIZEPQ                                      | @234    |
| 9  | GETDIBITS                                   | @441    |
| 12 | SETWINDOWEXT                                | @12     |
| 12 | SELECTBITMAP                                | @195    |
| 11 | SETDCSTATUS                                 | @170    |
| 13 | GETTEXTEXTENT                               | @91     |
| 20 | SETTEXTJUSTIFICATION                        | @10     |
| 15 | GETKERNINGPAIRS                             | @332    |
| 9  | GETRGNBOX                                   | @134    |
| 10 | SETBKCOLOR                                  | @1      |
| 17 | OFFSETWINDOWORGEX                           | @477    |
| 10 | STRETCHBLT                                  | @35     |
| 13 | GETDEVICECAPS                               | @80     |
| 6  | ENDDOC                                      | @378    |
| 16 | GETCHARABCWIDTHS                            | @307    |
| 7  | POLYGON                                     | @36     |
| 5  | BRUTE                                       | @213    |
| 12 | DELETEOBJECT                                | @69     |
| 8  | PAINTRGN                                    | @43     |
| 15 | ISVALIDMETAFILE                             | @410    |
| 13 | UNICODETOANSI                               | @467    |
| 10 | FTRAPPING0                                  | @355    |
| 17 | ENGINEGETGLYPHBMP                           | @305    |
| 14 | EXCLUDEVISRECT                              | @73     |
| 4  | COPY                                        | @250    |
| 10 | GETDCSTATE                                  | @179    |
| 14 | GETVIEWPORTEXT                              | @94     |
| 9  | SETDIBITS                                   | @440    |
| 20 | GETCURRENTPOSITIONEX                        | @470    |
| 27 | CREATEUSERDISCARDABLEBITMAP                 | @409    |
| 9  | SPOOLFILE                                   | @254    |
| 10 | SETRECTRGN                                  | @172    |
| 9  | RESTOREDC                                   | @39     |
| 11 | DMCOLORINFO                                 | @202    |
| 6  | ESCAPE                                      | @38     |
| 8  | STARTDOC                                    | @377    |
| 13 | SHRINKGDIHEAP                               | @354    |
| 10 | GETCLIPBOX                                  | @77     |
| 21 | CREATERECTRGNINDIRECT                       | @65     |
| 15 | GETGLYPHOUTLINE                             | @309    |
| 8  | CREATEPQ                                    | @230    |
| 9  | ISDCDIRTY                                   | @169    |
| 16 | SCALEVIEWPORTEXT                            | @18     |
| 12 | SETABORTPROC                                | @381    |
| 23 | GETSYSTEMPALETTEENTRIES                     | @375    |
| 6  | SCANLR                                      | @135    |
| 8  | ABORTDOC                                    | @382    |
| 17 | ENGINEMAKEFONTDIR                           | @306    |
| 10 | WRITESPOOL                                  | @241    |
| 9  | OFFSETRGN                                   | @101    |
| 12 | FINALGDIINIT                                | @405    |
| 9  | GETDCHOOK                                   | @191    |
| 8  | DELETEPQ                                    | @235    |
| 10 | SETDCSTATE                                  | @180    |
| 14 | SETVIEWPORTEXT                              | @14     |
| 9  | GETBKMODE                                   | @76     |
| 18 | CREATEFONTINDIRECT                          | @57     |
| 7  | ENDPAGE                                     | @380    |
| 11 | GETSPOOLJOB                                 | @245    |
| 12 | ENDSPOOLPAGE                                | @247    |
| 9  | PTVISIBLE                                   | @103    |
| 17 | GETRASTERIZERCAPS                           | @313    |
| 19 | CREATEBRUSHINDIRECT                         | @50     |
| 15 | DELETESPOOLPAGE                             | @253    |
| 3  | ARC                                         | @23     |
| 15 | EXCLUDECLIPRECT                             | @21     |
| 11 | GETFONTDATA                                 | @311    |
| 11 | GETTEXTFACE                                 | @92     |
| 12 | GETTEXTCOLOR                                | @90     |
| 10 | SAVEVISRGN                                  | @129    |
| 7  | GETROP2                                     | @85     |
| 13 | OFFSETCLIPRGN                               | @32     |
| 15 | DMREALIZEOBJECT                             | @210    |
| 9  | SETDCHOOK                                   | @190    |
| 15 | ADDFONTRESOURCE                             | @119    |
| 13 | GDIMOVEBITMAP                               | @401    |
| 5  | DEATH                                       | @121    |
| 12 | SELECTOBJECT                                | @45     |
| 3  | PIE                                         | @26     |
| 14 | SCALEWINDOWEXT                              | @16     |
| 14 | STARTSPOOLPAGE                              | @246    |
| 8  | CLOSEJOB                                    | @243    |
| 18 | CREATECOMPATIBLEDC                          | @52     |
| 9  | SETBKMODE                                   | @2      |
| 7  | LINEDDA                                     | @100    |
| 21 | GETPHYSICALFONTHANDLE                       | @352    |
| 13 | DMSETDIBTODEV                               | @219    |
| 17 | MAKEOBJECTPRIVATE                           | @463    |
| 12 | SETTEXTCOLOR                                | @9      |
| 8  | GETPIXEL                                    | @83     |
| 20 | GETBITMAPDIMENSIONEX                        | @468    |
| 12 | PLAYMETAFILE                                | @123    |
| 7  | SETROP2                                     | @4      |
| 18 | GETTEXTEXTENTPOINT                          | @471    |
| 9  | ROUNDRECT                                   | @28     |
| 18 | CREATEROUNDRECTRGN                          | @444    |
| 18 | CREATEPATTERNBRUSH                          | @60     |
| 19 | ENGINEENUMERATEFONT                         | @300    |
| 15 | FASTWINDOWFRAME                             | @400    |
| 13 | CREATEPALETTE                               | @360    |
| 8  | DMSCANLR                                    | @212    |
| 13 | SELECTCLIPRGN                               | @44     |
| 13 | STRETCHDIBITS                               | @439    |
| 22 | GETNEARESTPALETTEINDEX                      | @370    |
| 8  | QUERYJOB                                    | @248    |
| 15 | GETMETAFILEBITS                             | @159    |
| 8  | POLYLINE                                    | @37     |
| 21 | SETMETAFILEBITSBETTER                       | @196    |
| 8  | DMSTRBLT                                    | @211    |
| 15 | UNREALIZEOBJECT                             | @150    |
| 12 | UPDATECOLORS                                | @366    |
| 14 | SETMAPPERFLAGS                              | @349    |
| 23 | CREATEDISCARDABLEBITMAP                     | @156    |
| 19 | GETSYSTEMPALETTEUSE                         | @374    |
| 8  | SETPIXEL                                    | @31     |
| 20 | SETBITMAPDIMENSIONEX                        | @478    |
| 17 | CREATEELLIPTICRGN                           | @54     |
| 6  | SAVEDC                                      | @30     |
| 7  | OPENJOB                                     | @240    |
| 16 | CREATEUSERBITMAP                            | @407    |
| 17 | OFFSETVIEWPORTORG                           | @17     |
| 14 | CREATEMETAFILE                              | @125    |
| 18 | GETCURRENTPOSITION                          | @78     |
| 15 | SETMETAFILEBITS                             | @160    |
| 8  | EQUALRGN                                    | @72     |
| 17 | GETPALETTEENTRIES                           | @363    |
| 13 | GETBITMAPBITS                               | @74     |
| 12 | RECTINREGION                                | @181    |
| 8  | GETDCORG                                    | @79     |
| 18 | GDITASKTERMINATION                          | @460    |
| 10 | CREATEFONT                                  | @56     |
| 22 | GETASPECTRATIOFILTEREX                      | @486    |
| 16 | GDISELECTPALETTE                            | @361    |
| 14 | DELETEMETAFILE                              | @127    |
| 14 | SETOBJECTOWNER                              | @461    |
| 17 | CREATEPENINDIRECT                           | @62     |
| 14 | GETWINDOWORGEX                              | @475    |
| 16 | GETVIEWPORTORGEX                            | @473    |
| 18 | ISDCCURRENTPALETTE                          | @412    |
| 12 | DMENUMDFONTS                                | @206    |
| 11 | ENUMOBJECTS                                 | @71     |
| 11 | DMTRANSPOSE                                 | @220    |
| 12 | SETHOOKFLAGS                                | @192    |
| 20 | CREATEPOLYPOLYGONRGN                        | @451    |
| 19 | SETSYSTEMPALETTEUSE                         | @373    |
| 11 | POLYPOLYGON                                 | @450    |
| 14 | ANIMATEPALETTE                              | @367    |
| 14 | GETENVIRONMENT                              | @133    |
| 9  | ENUMFONTS                                   | @70     |
| 17 | GDIREALIZEPALETTE                           | @362    |
| 27 | FIXUPBOGUSPUBLISHERMETAFILE                 | @464    |
| 9  | RECTANGLE                                   | @27     |
| 17 | GETSTRETCHBLTMODE                           | @88     |
| 8  | MOVETOEX                                    | @483    |
| 11 | GDISEEGDIDO                                 | @452    |
| 21 | REALIZEDEFAULTPALETTE                       | @365    |
| 6  | LINETO                                      | @19     |
| 9  | DMDIBBITS                                   | @217    |
| 17 | ENGINEREALIZEFONT                           | @302    |
| 17 | SETPALETTEENTRIES                           | @364    |
| 13 | SETBITMAPBITS                               | @106    |
| 13 | CREATERECTRGN                               | @64     |
| 8  | SETDCORG                                    | @117    |