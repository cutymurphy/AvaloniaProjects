using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Threading;
using Task3_2.Models;

namespace Task3_2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly CrossingModel _crossingModel;
        
        public ObservableCollection<CarViewModel> Cars { get; }
        public ObservableCollection<PedestrianViewModel> Pedestrians { get; }
        public double RoadY => _crossingModel?.RoadY ?? 200;
        public double RoadHeight => _crossingModel?.RoadHeight ?? 100;

        public MainWindowViewModel()
        {
            Cars = new ObservableCollection<CarViewModel>();
            Pedestrians = new ObservableCollection<PedestrianViewModel>();
            
            _crossingModel = new CrossingModel();
            
            _crossingModel.Cars.CollectionChanged += CarsCollectionChanged;
            _crossingModel.Pedestrians.CollectionChanged += PedestriansCollectionChanged;
        }

        private void CarsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (e.NewItems != null)
                {
                    foreach (Car car in e.NewItems)
                    {
                        var carViewModel = new CarViewModel(car);
                        Cars.Add(carViewModel);
                    }
                }
                
                if (e.OldItems != null)
                {
                    foreach (Car car in e.OldItems)
                    {
                        var vmToRemove = Cars.FirstOrDefault(vm => vm.Model == car);
                        if (vmToRemove != null)
                        {
                            Cars.Remove(vmToRemove);
                        }
                    }
                }
                
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    Cars.Clear();
                }
            });
        }
        
        private void PedestriansCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (e.NewItems != null)
                {
                    foreach (Pedestrian pedestrian in e.NewItems)
                    {
                        var pedestrianViewModel = new PedestrianViewModel(pedestrian);
                        Pedestrians.Add(pedestrianViewModel);
                    }
                }
                
                if (e.OldItems != null)
                {
                    foreach (Pedestrian pedestrian in e.OldItems)
                    {
                        var vmToRemove = Pedestrians.FirstOrDefault(vm => vm.Model == pedestrian);
                        if (vmToRemove != null)
                        {
                            Pedestrians.Remove(vmToRemove);
                        }
                    }
                }
                
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    Pedestrians.Clear();
                }
            });
        }
    }

    public class CarViewModel : ViewModelBase
    {
        public Car Model { get; }
        
        private double _x;
        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }
        
        private double _y;
        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }
        
        public CarViewModel(Car model)
        {
            Model = model;
            UpdatePosition();
        }
        
        public void UpdatePosition()
        {
            X = Model.X;
            Y = Model.Y;
        }
    }
    
    public class PedestrianViewModel : ViewModelBase
    {
        public Pedestrian Model { get; }
        
        private double _x;
        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }
        
        private double _y;
        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }
        
        public PedestrianViewModel(Pedestrian model)
        {
            Model = model;
            UpdatePosition();
        }
        
        public void UpdatePosition()
        {
            X = Model.X;
            Y = Model.Y;
        }
    }
}