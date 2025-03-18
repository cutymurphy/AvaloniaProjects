using System;
using AvaloniaApplication1.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaApplication1.ViewModels
{
    public class SimpleViewModel : INotifyPropertyChanged
    {
        private Queue<int> queue = new Queue<int>();

        public event PropertyChangedEventHandler? PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string? _ValueData;

        public string? ValueData
        {
            get => _ValueData;
            set
            {
                if (_ValueData != value)
                {
                    _ValueData = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int QueueCount => queue.Count;
        public bool QueueIsEmpty => queue.IsEmpty;
        public string QueueCurrentElement => queue.CurrentElement;
        public string QueueContent => "Queue: " + queue.Print();

        public ICommand EnqueueCommand { get; }
        public ICommand DequeueCommand { get; }
        public ICommand ClearCommand { get; }

        private string? _errorMessage;

        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    RaisePropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public SimpleViewModel()
        {
            EnqueueCommand = new RelayCommand(Enqueue);
            DequeueCommand = new RelayCommand(Dequeue);
            ClearCommand = new RelayCommand(Clear);
        }

        private void Enqueue()
        {
            if (int.TryParse(_ValueData, out int value))
            {
                queue.Enqueue(value);
                ErrorMessage = null;
                UpdateQueueState();
            }
        }

        private void Dequeue()
        {
            ErrorMessage = null;
            if (QueueIsEmpty)
            {
                ErrorMessage = "Queue is empty. Unable to delete element.";
                ClearErrorMessageAfterDelay(3000);
            }
            else
            {
                queue.Dequeue();
                UpdateQueueState();
            }
        }

        private void Clear()
        {
            if (!QueueIsEmpty)
            {
                queue.Clear();
                UpdateQueueState();
            }
        }

        private void UpdateQueueState()
        {
            RaisePropertyChanged(nameof(QueueCount));
            RaisePropertyChanged(nameof(QueueIsEmpty));
            RaisePropertyChanged(nameof(QueueCurrentElement));
            RaisePropertyChanged(nameof(QueueContent));
        }

        private async void ClearErrorMessageAfterDelay(int delayMilliseconds)
        {
            await Task.Delay(delayMilliseconds);
            ErrorMessage = null;
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