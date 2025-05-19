using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileSystemLibrary.Models;

namespace AvaloniaApplication4.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ReflectionService _reflectionService = new ReflectionService();

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

        private object? _currentInstance;

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
                var result = _reflectionService.InvokeMethod(SelectedClass, SelectedMethod, _currentInstance, Parameters.ToList());
                _currentInstance = result as IFileSystemItem; // Сохраняем экземпляр, если метод возвращает IFileSystemItem
                ExecutionResult = result != null ? $"Результат: {result}" : "Метод выполнен.";
            }
            catch (Exception ex)
            {
                ExecutionResult = ex.Message;
            }
        }
    }
}