using System;
using System.Collections.Generic;
using System.Reflection;
using AvaloniaApplication4.Models;
using FileSystemLibrary.Models;
using System.Linq;

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
                .Where(m => m.DeclaringType == type)
                .ToList();
        }

        public List<MethodParameter> GetMethodParameters(MethodInfo method)
        {
            return method.GetParameters()
                .Select(p => new MethodParameter(p.Name!, p.ParameterType))
                .ToList();
        }

        public object? InvokeMethod(Type type, MethodInfo method, object? instance, List<MethodParameter> parameters)
        {
            try
            {
                var paramValues = parameters.Select(p => ConvertParameter(p.Value, p.Type)).ToArray();
                if (method.IsStatic)
                {
                    return method.Invoke(null, paramValues);
                }
                else
                {
                    instance ??= Activator.CreateInstance(type, new object[] { "Temp" }); // Создаем экземпляр с временным именем
                    return method.Invoke(instance, paramValues);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка вызова метода: {ex.Message}");
            }
        }

        private object? ConvertParameter(string value, Type type)
        {
            if (string.IsNullOrEmpty(value) && !type.IsValueType) return null;
            if (type == typeof(string)) return value;
            if (type == typeof(int)) return int.Parse(value);
            if (type == typeof(long)) return long.Parse(value);
            if (type == typeof(Folder) || type == typeof(IFileSystemItem) || type == typeof(FileSystemItem))
            {
                // Ожидаем формат: "Folder:name" или "File:name:size"
                var parts = value.Split(':');
                if (parts.Length < 2) throw new ArgumentException("Неверный формат ввода. Ожидается 'Folder:name' или 'File:name:size'.");

                string itemType = parts[0].ToLower();
                string name = parts[1];

                if (itemType == "folder")
                {
                    return new Folder(name);
                }
                else if (itemType == "file")
                {
                    if (parts.Length < 3 || !long.TryParse(parts[2], out long size))
                        throw new ArgumentException("Для файла требуется размер в формате 'File:name:size'.");
                    return new File(name, size);
                }
                else
                {
                    throw new ArgumentException("Неизвестный тип: укажите 'Folder' или 'File'.");
                }
            }
            throw new NotSupportedException($"Тип параметра {type.Name} не поддерживается.");
        }
    }
}