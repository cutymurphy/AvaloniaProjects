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

        private object? _currentInstance;

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
            _currentInstance = null;

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
                // Используем корневую папку из FileSystemItems как экземпляр, если это Folder
                if (_currentInstance == null && FileSystemItems.Any() && SelectedClass == typeof(Folder))
                {
                    _currentInstance = FileSystemItems.FirstOrDefault(i => i is Folder);
                }

                var result = _reflectionService.InvokeMethod(SelectedClass, SelectedMethod, _currentInstance, Parameters.ToList());
                _currentInstance = result as IFileSystemItem ?? _currentInstance;

                // Обновляем FileSystemItems, если метод изменил структуру
                if (_currentInstance is Folder folder)
                {
                    var root = FileSystemItems.FirstOrDefault(i => i is Folder);
                    if (root != folder)
                    {
                        FileSystemItems.Clear();
                        FileSystemItems.Add(folder);
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