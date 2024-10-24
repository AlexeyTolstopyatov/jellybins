using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using jellybins.Binary;
using Wpf.Ui.Controls;

namespace jellybins.Models
{
    /// <summary>
    /// .NET Component's Model.
    /// I've decided, that Model's content not only Methods's array
    /// Imagine, this is [COM]
    /// [COM] includes (Name) (Type) (Methods[]) (LoaderFlag)
    ///     
    ///   [ (Name) (Type) (LoaderFlag) (Description)
    ///     (Methods[]) (Modules?[]) Types?[] ]
    ///     
    /// </summary>
    
    internal class ClrInternals
    {
        public string? Name { get; private set; }
        public string Description { get; private set; }
        public string? Version { get; private set; }
        public string[] Methods { get; private set; } = Array.Empty<string>();
        public string[] Types { get; private set; } = Array.Empty<string>();
        public string SystemType { get; private set; }
        public string Title { get; private set; }
        public bool Loaded { get; private set; }

        public ClrInternals(string path) 
        {
            Loaded = true;
            Parallel.Invoke(() => CreateMembersCollection(path));
            Title       = JbTypeInformation.GetTitle(JbFileType.NetObject);
            Description = JbTypeInformation.GetInformation(JbFileType.NetObject);
            SystemType  = JbTypeInformation.GetType(JbFileType.NetObject);
        }

        private void CreateMembersCollection(string path) 
        {
            try
            {
                Assembly assembly    = Assembly.LoadFrom(path);
                
                Name    = new FileInfo(path).Name;
                Version = assembly.ImageRuntimeVersion;

                Methods = (
                    from module in assembly.GetModules() 
                    from type in module.GetTypes() 
                    from method in type.GetMethods() 
                    select
                        $"{type.Namespace}.{type.Name}.{method.Name}(...) -> {method.ReturnType.Name}"
                ).ToArray();

                Types = (
                        from module in assembly.GetModules()
                        from type in module.GetTypes()
                        select $"{type.FullName}").ToArray();

            }
            catch (Exception e)
            {
                Loaded = false;
#if DEBUG
                JbAppReport.ShowException(e);
#else
                JbAppReport.ShowException(new OperationCancelledException("Не удалось загрузить сборку"))
#endif
                return;
            }
            Loaded = true;
        }
    }
}
