using System.Collections.ObjectModel;
using AvaloniaApplication4.Models;

namespace AvaloniaApplication4.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainViewModel MainViewModel { get; } = new MainViewModel();
    public ObservableCollection<FileSystemItem> FileSystemItems => MainViewModel.FileSystemItems;
}