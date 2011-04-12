#region usings

using System;
using System.Linq;
using System.Reflection;

#endregion

internal class ModuleInitializer
{
    public static void Run()
    {
        AppDomain.CurrentDomain.AssemblyResolve +=
            (sender, args) =>
            {
                string assemblyName = new AssemblyName(args.Name).Name;
                var assemblyFileName = assemblyName + ".dll";
                var pdbFileName = assemblyName + ".pdb";

                var assemblyResources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

                var dllResource = assemblyResources.Where(name => name.EndsWith(assemblyFileName)).SingleOrDefault();
                var symbolsResource = assemblyResources.Where(name => name.EndsWith(pdbFileName)).SingleOrDefault();

                if (dllResource == null)
                {
                    return null;
                }

                byte[] assemblyData = ReadResourceByteArray(dllResource);
                byte[] pdbData = null;

                if(symbolsResource != null)
                {
                    pdbData = ReadResourceByteArray(symbolsResource);
                }

                var loaded = Assembly.Load(assemblyData, pdbData);
                if(loaded.GetName().FullName != args.Name)
                {
                    return null;
                }
                return loaded;
            };
    }

    private static Byte[] ReadResourceByteArray(string resource)
    {
        using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
        {
            var resourceData = new Byte[resourceStream.Length];
            resourceStream.Read(resourceData, 0, resourceData.Length);
            return resourceData;
        }
    }
}