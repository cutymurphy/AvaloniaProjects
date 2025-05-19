using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileSystemLibrary.Models;
using FileSystemLibrary.Services;

namespace AvaloniaApplication4.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ReflectionService _reflectionService = new ReflectionService();
        private readonly FileSystemManager _fileSystemManager = new FileSystemManager();

        [ObservableProperty]
        private string _dllPath = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Type> _classes = new ObservableCollection<Type>();

        [ObservableProperty]
        private Type? _selectedClass;

        [ObservableProperty]
        private ObservableCollection<MethodInfo> _methods = new ObservableCollection<MethodInfo>();

        [ObservableProperty]
        private MethodInfo? _selectedMethod;

        [ObservableProperty]
        private ObservableCollection<MethodParameter> _parameters = new ObservableCollection<MethodParameter>();

        [ObservableProperty]
        private string _executionResult = string.Empty;

        [ObservableProperty]
        private ObservableCollection<FileSystemItem> _fileSystemItems;

        public MainWindowViewModel()
        {
            _fileSystemManager.Initialize();
            FileSystemItems = _fileSystemManager.FileSystemItems;
        }

        partial void OnSelectedClassChanged(Type? value)
        {
            Methods.Clear();
            Parameters.Clear();
            ExecutionResult = string.Empty;

            if (value != null)
            {
                var methods = _reflectionService.GetMethods(value);
                foreach (var method in methods)
                {
                    Methods.Add(method);
                }
            }
        }

        partial void OnSelectedMethodChanged(MethodInfo? value)
        {
            Parameters.Clear();
            ExecutionResult = string.Empty;

            if (value != null)
            {
                var parameters = _reflectionService.GetMethodParameters(value);
                foreach (var param in parameters)
                {
                    Parameters.Add(param);
                }
            }
        }

        [RelayCommand]
        private void LoadDll()
        {
            try
            {
                Classes.Clear();
                var classes = _reflectionService.LoadClassesFromDll(DllPath);
                foreach (var cls in classes)
                {
                    Classes.Add(cls);
                }
                ExecutionResult = "DLL успешно загружен.";
            }
            catch (Exception ex)
            {
                ExecutionResult = ex.Message;
            }
        }

        [RelayCommand]
        private void ExecuteMethod()
        {
            if (SelectedClass == null || SelectedMethod == null)
            {
                ExecutionResult = "Выберите класс и метод.";
                return;
            }

            try
            {
                // Находим целевую папку и имя элемента
                Folder? targetFolder = null;
                string itemName = string.Empty;
                var param = Parameters.FirstOrDefault();

                if (param != null)
                {
                    if (param.Value.Contains('/'))
                    {
                        var parts = param.Value.Split('/');
                        itemName = parts[^1];
                        string path = string.Join("/", parts.Take(parts.Length - 1));
                        targetFolder = _reflectionService.FindFolderByPath(FileSystemItems, path);
                        if (targetFolder == null)
                        {
                            ExecutionResult = $"Папка по пути '{path}' не найдена.";
                            return;
                        }
                        // Обновляем параметр, чтобы передать только имя элемента
                        param.Value = itemName;
                    }
                    else
                    {
                        itemName = param.Value;
                        targetFolder = FileSystemItems.FirstOrDefault(i => i is Folder) as Folder;
                        if (targetFolder == null)
                        {
                            ExecutionResult = "Корневая папка не найдена.";
                            return;
                        }
                    }
                }
                else
                {
                    targetFolder = FileSystemItems.FirstOrDefault(i => i is Folder) as Folder;
                    if (targetFolder == null)
                    {
                        ExecutionResult = "Корневая папка не найдена.";
                        return;
                    }
                }

                // Для File.Create добавляем размер из второго параметра
                if (SelectedClass == typeof(File) && SelectedMethod.Name == "Create")
                {
                    var sizeParam = Parameters.FirstOrDefault(p => p.Type == typeof(long));
                    if (sizeParam == null || string.IsNullOrEmpty(sizeParam.Value) || !long.TryParse(sizeParam.Value, out _))
                    {
                        ExecutionResult = "Укажите корректный размер файла.";
                        return;
                    }
                }

                // Вызываем метод
                object? result;
                if (SelectedClass == typeof(Folder) && SelectedMethod.Name == "Add")
                {
                    // Вызываем Add на целевой папке
                    var addMethod = SelectedClass.GetMethod("Add");
                    if (addMethod == null)
                    {
                        ExecutionResult = "Метод Add не найден.";
                        return;
                    }
                    var newFolder = new Folder(itemName);
                    addMethod.Invoke(targetFolder, new[] { newFolder as object });
                    result = newFolder;
                }
                else
                {
                    // Для других методов (например, File.Create)
                    result = _reflectionService.InvokeMethod(SelectedClass, SelectedMethod, null, Parameters.ToList(), FileSystemItems);
                    if (SelectedClass == typeof(File) && SelectedMethod.Name == "Create")
                    {
                        if (result is File file)
                        {
                            targetFolder.Add(file);
                        }
                    }
                }

                ExecutionResult = result != null ? $"Результат: {result}" : "Метод выполнен.";
            }
            catch (Exception ex)
            {
                ExecutionResult = ex.Message;
            }
        }
    }
}