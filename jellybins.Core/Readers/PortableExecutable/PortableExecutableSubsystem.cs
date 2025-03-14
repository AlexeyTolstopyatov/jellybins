namespace jellybins.Core.Strings;

/// <summary>
/// Here is containing all PE supported
/// subsystem values and specific JellyBins flags
/// for other image's types
/// </summary>
public enum PortableExecutableSubsystem
{
    Unknown = 0,
    /// <summary>
    /// Binary works with absolute minimal
    /// OS API support.
    /// (usually works at Kernel ring)
    /// </summary>
    Native = 1,
    /// <summary>
    /// Binary depends on Windows API
    /// Works at User ring or Service ring (2, 3)
    /// Tells OS draw Window manually 
    /// </summary>
    Win32Gui = 2,
    /// <summary>
    /// Binary depends on Windows API
    /// Works ar User ring or Service ring (2, 3)
    /// Tells OS to connect app with ConDrv Host
    /// </summary>
    Win32Cui = 3,
    /// <summary>
    /// Binary uses doscalls.dll os2ss.exe or other
    /// OS/2 subsystem API. Usually needs for OS/2 1.x CI
    /// applications. Last existed at NT 5.1 (Obsolete)
    /// </summary>
    Os2SsCui = 5,
    /// <summary>
    /// Binary uses POSIX (Portable OS Interface)
    /// e.g. kill, fork, signal functions.
    /// </summary>
    PosixSs = 7,
    /// <summary>
    /// Uses about minimal Windows API, later it was
    /// DRV files, which loaded in User ring. (uses MMSYSTEM.dll)
    /// </summary>
    WindowsNative = 8,
    /// <summary>
    /// Uses WindowsCE Graphic modules
    /// </summary>
    WinCeGui = 9,
    /// <summary>
    /// Extensible Firmware Application,
    /// segmented like Portable Executable
    /// </summary>
    EfiApplication = 10,
    /// <summary>
    /// Extensible Firmware Driver, works between
    /// Kernel ring and Hypervisor ring (0 and -1).
    /// It may be ACPI driver, that uses very limited Physical
    /// Addresses and not connected with Windows.
    /// For those drivers, usually makes WDM/WDF drivers
    /// for communication. 
    /// </summary>
    EfiBootDriver = 11,
    /// <summary>
    /// I don't know what is it.
    /// </summary>
    EfiRomImage = 12,
    /// <summary>
    /// Xbox module. May be module uses Xbox API...
    /// </summary>
    XboxApplication = 13,
    /// <summary>
    /// I DON'T KNOW WHY.... WHY....... 
    /// </summary>
    WinBootApplication = 14,
    
    // My flags for other binaries
    /// <summary>
    /// Win16 environment. Also uses Windows API
    /// but Windows 3x subsystem (not NT!)
    /// </summary>
    Win16Gui = 21,
    /// <summary>
    /// Application need to runs under Virtual DOS Machine
    /// NT-VDM last existed in NT 5.2
    /// </summary>
    NtVirtualDosMachine = 22,
}