namespace JellyBins.PortableExecutable.Private.Types;
public enum VbComType
{
    DesignerAdd = 0x2,  // add-on for UserControls
    ClassModule = 0x10, // based
    UserControl = 0x20, // ActiveX Control
    UserDocument = 0x80
}