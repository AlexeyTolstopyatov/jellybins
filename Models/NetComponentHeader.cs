using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
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
    
    internal class NetComponentHeader
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Version { get; private set; }
        public string[] Methods { get; private set; } = new string[0];
        public string[] Types { get; private set; } = new string[0];
        public string SystemType { get; private set; }
        public string Title { get; private set; }
        public bool Loaded { get; private set; }

        public NetComponentHeader(string path) 
        {
            Loaded = true;
            Parallel.Invoke(() => CreateMembersCollection(path)); // асинхронный ад
            Title       = TypeofInformation.GetTitle(TypeofFile.NetObject);
            Description = TypeofInformation.GetInformation(TypeofFile.NetObject);
            SystemType  = TypeofInformation.GetType(TypeofFile.NetObject);
        }

        private void CreateMembersCollection(string path) 
        {
            try
            {
                Assembly assembly    = Assembly.LoadFrom(path);
                List<string> methods = new();
                List<string> types   = new();
                
                Name    = new FileInfo(path).Name;
                Version = assembly.ImageRuntimeVersion;

                foreach (var module in assembly.GetModules())
                {
                    foreach (var type in module.GetTypes())
                    {
                        foreach (var method in type.GetMembers())
                        {
                            methods.Add($"{type.Namespace}.{type.Name}.{method.Name}->({method.ReflectedType}){{}}");
                        }
                    }
                }
                

                Methods = methods.ToArray();
                assembly = null!;
            }
            catch (Exception e)
            {
                Loaded = false;
#if DEBUG
                var debug = new Wpf.Ui.Controls.MessageBox()
                {
                    Title = "jellybins.Models.NetAssembly.CreateMembersCollection",
                    Content = e.ToString()
                }.ShowDialogAsync();
#else
                var message = new Wpf.Ui.Controls.MessageBox()
                {
                    Title = "Jelly Bins",
                    Content = $"Не удалость загрузить сборку"
                }.ShowDialogAsync()
#endif
                return;
            }
            Loaded = true;
            
            return;
        }

        public static void DestroyaMembersCollection(ref NetComponentHeader model) 
        {
            // ah ahah ahahah
            model.Methods = model.Types = new string[0];
            model.Name = model.Description = model.Version = model.Title = model.SystemType = string.Empty;
            model.Loaded = false;
        }
    }
}
