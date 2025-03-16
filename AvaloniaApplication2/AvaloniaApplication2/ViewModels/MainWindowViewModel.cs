using System.Collections.ObjectModel;
using AvaloniaApplication2.Models;

namespace AvaloniaApplication2.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainViewModel MainViewModel { get; } = new MainViewModel();
    public ObservableCollection<FileSystemItem> FileSystemItems => MainViewModel.FileSystemItems;
}