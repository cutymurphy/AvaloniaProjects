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
        private readonly TransportModel _model;

        public ObservableCollection<VehicleViewModel> Vehicles { get; }
        public ObservableCollection<PassengerViewModel> Passengers { get; }
        public ObservableCollection<StopViewModel> Stops { get; }
        public double RoadY => _model.RoadY;
        public double RoadHeight => _model.RoadHeight;
        public double SidewalkTopY => _model.SidewalkTopY;
        public double SidewalkBottomY => _model.SidewalkBottomY;

        public MainWindowViewModel()
        {
            _model = new TransportModel();
            Vehicles = new ObservableCollection<VehicleViewModel>();
            Passengers = new ObservableCollection<PassengerViewModel>();
            Stops = new ObservableCollection<StopViewModel>();

            foreach (var stop in _model.Stops)
            {
                Stops.Add(new StopViewModel(stop));
            }

            _model.Vehicles.CollectionChanged += VehiclesCollectionChanged;
            _model.Passengers.CollectionChanged += PassengersCollectionChanged;
        }

        private void VehiclesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (e.NewItems != null)
                {
                    foreach (Vehicle vehicle in e.NewItems)
                    {
                        VehicleViewModel vm = vehicle is Bus
                            ? new BusViewModel((Bus)vehicle)
                            : new CarViewModel((Car)vehicle);
                        Vehicles.Add(vm);
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (Vehicle vehicle in e.OldItems)
                    {
                        var vmToRemove = Vehicles.FirstOrDefault(vm => vm.Model == vehicle);
                        if (vmToRemove != null)
                        {
                            Vehicles.Remove(vmToRemove);
                        }
                    }
                }

                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    Vehicles.Clear();
                }
            });
        }

        private void PassengersCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (e.NewItems != null)
                {
                    foreach (IPassenger passenger in e.NewItems)
                    {
                        var vm = new PassengerViewModel(passenger);
                        Passengers.Add(vm);
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (IPassenger passenger in e.OldItems)
                    {
                        var vmToRemove = Passengers.FirstOrDefault(vm => vm.Model == passenger);
                        if (vmToRemove != null)
                        {
                            Passengers.Remove(vmToRemove);
                        }
                    }
                }

                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    Passengers.Clear();
                }
            });
        }
    }
}