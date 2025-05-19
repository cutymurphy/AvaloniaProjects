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
                .Where(m => m.DeclaringType == type) // Только методы, объявленные в самом классе
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
                    instance ??= Activator.CreateInstance(type);
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
            if (type == typeof(IFileSystemItem) || type == typeof(Folder))
            {
                // Для простоты создаем новую папку с именем из значения
                return new Folder(value);
            }
            throw new NotSupportedException($"Тип параметра {type.Name} не поддерживается.");
        }
    }
}