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
        private string _executionResultColor = "Red"; // Новое свойство для цвета

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
            ExecutionResultColor = "Red";

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
            ExecutionResultColor = "Red";

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
                if (string.IsNullOrEmpty(DllPath))
                {
                    ExecutionResult = "Укажите путь к DLL файлу.";
                    ExecutionResultColor = "Red";
                    return;
                }
                Classes.Clear();
                var classes = _reflectionService.LoadClassesFromDll(DllPath);
                foreach (var cls in classes)
                {
                    Classes.Add(cls);
                }
                ExecutionResult = "DLL успешно загружен.";
                ExecutionResultColor = "Green";
            }
            catch (Exception ex)
            {
                ExecutionResult = $"Ошибка при загрузке DLL: {ex.Message}";
                ExecutionResultColor = "Red";
            }
        }

        [RelayCommand]
        private void ExecuteMethod()
        {
            if (SelectedClass == null || SelectedMethod == null)
            {
                ExecutionResult = "Выберите класс и метод для выполнения.";
                ExecutionResultColor = "Red";
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
                    if (string.IsNullOrEmpty(param.Value))
                    {
                        ExecutionResult = "Имя элемента не может быть пустым.";
                        ExecutionResultColor = "Red";
                        return;
                    }
                    if (param.Value.Contains('/'))
                    {
                        var parts = param.Value.Split('/');
                        itemName = parts[^1];
                        if (string.IsNullOrEmpty(itemName))
                        {
                            ExecutionResult = "Имя элемента не может быть пустым.";
                            ExecutionResultColor = "Red";
                            return;
                        }
                        string path = string.Join("/", parts.Take(parts.Length - 1));
                        if (string.IsNullOrEmpty(path))
                        {
                            ExecutionResult = "Указан некорректный путь.";
                            ExecutionResultColor = "Red";
                            return;
                        }
                        targetFolder = _reflectionService.FindFolderByPath(FileSystemItems, path);
                        if (targetFolder == null)
                        {
                            ExecutionResult = $"Папка по пути '{path}' не найдена.";
                            ExecutionResultColor = "Red";
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
                            ExecutionResultColor = "Red";
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
                        ExecutionResultColor = "Red";
                        return;
                    }
                }

                // Для File.Create добавляем размер из второго параметра
                if (SelectedClass == typeof(File) && SelectedMethod.Name == "Create")
                {
                    var sizeParam = Parameters.FirstOrDefault(p => p.Type == typeof(long));
                    if (sizeParam == null || string.IsNullOrEmpty(sizeParam.Value))
                    {
                        ExecutionResult = "Укажите размер файла.";
                        ExecutionResultColor = "Red";
                        return;
                    }
                    if (!long.TryParse(sizeParam.Value, out _))
                    {
                        ExecutionResult = "Размер файла должен быть числом.";
                        ExecutionResultColor = "Red";
                        return;
                    }
                }

                // Обработка методов
                if (SelectedClass == typeof(Folder) && SelectedMethod.Name == "Add")
                {
                    // Создание папки
                    var addMethod = SelectedClass.GetMethod("Add");
                    if (addMethod == null)
                    {
                        ExecutionResult = "Метод Add не найден.";
                        ExecutionResultColor = "Red";
                        return;
                    }
                    var newFolder = new Folder(itemName);
                    addMethod.Invoke(targetFolder, new[] { newFolder as object });
                }
                else if (SelectedClass == typeof(Folder) && SelectedMethod.Name == "Remove")
                {
                    // Удаление папки или файла
                    var itemToRemove = targetFolder.Items.FirstOrDefault(i => i.Name == itemName);
                    if (itemToRemove == null)
                    {
                        ExecutionResult = $"Элемент '{itemName}' не найден в папке '{targetFolder.Name}'.";
                        ExecutionResultColor = "Red";
                        return;
                    }
                    var removeMethod = SelectedClass.GetMethod("Remove");
                    if (removeMethod == null)
                    {
                        ExecutionResult = "Метод Remove не найден.";
                        ExecutionResultColor = "Red";
                        return;
                    }
                    removeMethod.Invoke(targetFolder, new[] { itemToRemove as object });
                }
                else if (SelectedClass == typeof(File) && SelectedMethod.Name == "Create")
                {
                    // Создание файла
                    var result = _reflectionService.InvokeMethod(SelectedClass, SelectedMethod, null, Parameters.ToList(), FileSystemItems);
                    if (result is File file)
                    {
                        targetFolder.Add(file);
                    }
                }
                else if (SelectedClass == typeof(File) && SelectedMethod.Name == "Delete")
                {
                    // Удаление файла
                    var fileToDelete = targetFolder.Items.OfType<File>().FirstOrDefault(f => f.Name == itemName);
                    if (fileToDelete == null)
                    {
                        ExecutionResult = $"Файл '{itemName}' не найден в папке '{targetFolder.Name}'.";
                        ExecutionResultColor = "Red";
                        return;
                    }
                    var deleteMethod = SelectedClass.GetMethod("Delete");
                    if (deleteMethod == null)
                    {
                        ExecutionResult = "Метод Delete не найден.";
                        ExecutionResultColor = "Red";
                        return;
                    }
                    deleteMethod.Invoke(fileToDelete, new[] { itemName as object });
                }
                else
                {
                    // Для других методов
                    var result = _reflectionService.InvokeMethod(SelectedClass, SelectedMethod, null, Parameters.ToList(), FileSystemItems);
                    ExecutionResult = result != null ? $"Результат: {result}" : "Метод успешно выполнен.";
                    ExecutionResultColor = "Green";
                    return;
                }

                ExecutionResult = "Метод успешно выполнен.";
                ExecutionResultColor = "Green";
            }
            catch (Exception ex)
            {
                ExecutionResult = ex.Message;
                ExecutionResultColor = "Red";
            }
        }
    }
}