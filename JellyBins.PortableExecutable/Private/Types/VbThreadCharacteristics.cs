namespace JellyBins.PortableExecutable.Private.Types;

[Flags]
public enum VbThreadCharacteristics
{
    /// <summary>
    /// Specifies Multi-Threading using Apartment-Model
    /// </summary>
    ApartmentModel,
    /// <summary>
    /// Specifies to do license-validation (.OCX only)
    /// </summary>
    RequireLicense,
    /// <summary>
    /// GUI elements should be initialized
    /// </summary>
    Unattended,
    /// <summary>
    /// [STAThread] 
    /// </summary>
    SingleThread,
    /// <summary>
    /// Keep the file in memory (only unattended)
    /// </summary>
    Retained
}