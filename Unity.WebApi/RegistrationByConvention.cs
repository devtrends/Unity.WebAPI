using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;

namespace Unity.WebApi
{
    public static class RegistrationByConvention
    {
        public static IEnumerable<Type> FromAssembliesInSearchPath()
        {
            return GetTypes(GetAssembliesInSearchPath());
        }

        private static IEnumerable<Assembly> GetAssembliesInSearchPath()
        {
            try
            {
                string searchPath = (AppDomain.CurrentDomain.RelativeSearchPath == null)
                    ? AppDomain.CurrentDomain.BaseDirectory
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath);

                return Directory.EnumerateFiles(searchPath, "*.dll")
                    .Select(x => LoadAssembly(Path.GetFileNameWithoutExtension(x)))
                    .Where(x => x != null);
            }
            catch (SecurityException)
            {
                return new Assembly[0];
            }
        }

        private static Assembly LoadAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (FileLoadException)
            {
                return null;
            }
            catch (BadImageFormatException)
            {
                return null;
            }
        }

        private static IEnumerable<Type> GetTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(assembly =>
            {
                try
                {
                    return GetTypes(assembly.DefinedTypes);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return GetTypes(ex.Types
                        .TakeWhile(x => x != null)
                        .Select(x => x.GetTypeInfo()));
                }
            });
        }

        private static IEnumerable<Type> GetTypes(IEnumerable<TypeInfo> typeInfos)
        {
            return typeInfos
                .Where(x => x.IsClass & !x.IsAbstract && !x.IsValueType && x.IsVisible)
                .Select(ti => ti.AsType());
        }
    }
}