namespace jellybins.File.Modeling.Base;

public interface IExecutableInformation
{
    string ProcessorFlagToString<T>(T flag) where T : IComparable;
    string OperatingSystemFlagToString<T>(T? flag) where T : IComparable;
    string VersionFlagsToString<TVersion>(TVersion major, TVersion minor) where TVersion : IComparable;
    string[] FlagsToStrings(object flags);
}