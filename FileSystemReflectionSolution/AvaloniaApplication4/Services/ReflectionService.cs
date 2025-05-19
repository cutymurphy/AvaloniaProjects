using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using AvaloniaApplication4.Models;
using FileSystemLibrary.Models;

namespace AvaloniaApplication4.Services
{
    public class ReflectionService
    {
        public List<Type> LoadClassesFromDll(string dllPath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                return assembly.GetTypes()
                    .Where(t => typeof(IFileSystemItem).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки DLL: {ex.Message}");
            }
        }

        public List<MethodInfo> GetMethods(Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Where(m => m.DeclaringType == type && !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_"))
                .ToList();
        }

        public List<MethodParameter> GetMethodParameters(MethodInfo method)
        {
            return method.GetParameters()
                .Select(p => new MethodParameter(p.Name!, p.ParameterType))
                .ToList();
        }

        public object? InvokeMethod(Type type, MethodInfo method, object? instance, List<MethodParameter> parameters,
            ObservableCollection<FileSystemItem> fileSystemItems)
        {
            try
            {
                var paramValues = parameters.Select(p => ConvertParameter(p, type, method, fileSystemItems)).ToArray();
                if (method.IsStatic)
                {
                    return method.Invoke(null, paramValues);
                }
                else
                {
                    instance ??= fileSystemItems.FirstOrDefault(i => i is Folder) ??
                                 Activator.CreateInstance(type, new object[] { "Temp" });
                    return method.Invoke(instance, paramValues);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка вызова метода: {ex.Message}");
            }
        }

        private object? ConvertParameter(MethodParameter param, Type classType, MethodInfo method,
            ObservableCollection<FileSystemItem> fileSystemItems)
        {
            string value = param.Value;
            Type type = param.Type;

            if (string.IsNullOrEmpty(value) && !type.IsValueType) return null;
            if (type == typeof(string)) return value;
            if (type == typeof(int)) return int.Parse(value);
            if (type == typeof(long)) return long.Parse(value);

            if (type == typeof(FileSystemItem) || type == typeof(IFileSystemItem))
            {
                if (classType == typeof(Folder) && method.Name == "Add")
                {
                    return new Folder(value);
                }
            }

            throw new NotSupportedException($"Тип параметра {type.Name} не поддерживается.");
        }

        public Folder? FindFolderByPath(ObservableCollection<FileSystemItem> fileSystemItems, string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            Folder? currentFolder = fileSystemItems.OfType<Folder>().FirstOrDefault();

            foreach (var part in parts)
            {
                if (currentFolder == null) return null;
                currentFolder = currentFolder.Items.OfType<Folder>().FirstOrDefault(f => f.Name == part);
            }

            return currentFolder;
        }
    }
}