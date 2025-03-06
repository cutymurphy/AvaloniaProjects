using Avalonia.Interactivity;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AvaloniaApplication1.Models;

namespace AvaloniaApplication1.ViewModels
{

    public class SimpleViewModel : INotifyPropertyChanged
    {
        workList<int> myList = new workList<int>();


        public event PropertyChangedEventHandler? PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ---- Add some Properties ----

        private string? _ValueData; // This is our backing field for Name
   

        public string? ValueData
        {
            get 
            {
                return _ValueData; 
            }
            set
            {
                // We only want to update the UI if the Name actually changed, so we check if the value is actually new
                if (_ValueData != value)
                {
                    // 1. update our backing field
                    _ValueData = value;

                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(Greeting));
                }
            }
        }
       

        // Greeting will change based on a Name.
        public string Greeting
        {
            get
            {
                return "Show List :" + myList.Print();
            }
        }

        public ICommand addButtonClickCommand { get; }

        public SimpleViewModel()
        {
            addButtonClickCommand = new RelayCommand(addOnButtonClick);
            delButtonClickCommand = new RelayCommand(delOnButtonClick);
        }

        private void addOnButtonClick()
        {
            int x = Int32.Parse(_ValueData);
            myList.Add(x);
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(Greeting));
        }

        public ICommand delButtonClickCommand { get; }

        private void delOnButtonClick()
        {
            int x = Int32.Parse(_ValueData);
            myList.Remove(x);
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(Greeting));
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged;
    }

}
