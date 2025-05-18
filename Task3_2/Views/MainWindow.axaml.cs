using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Task3_2.ViewModels;

namespace Task3_2.Views
{
    public partial class MainWindow : Window
    {
        private Canvas _mainCanvas;

        private readonly Dictionary<VehicleViewModel, Rectangle> _vehicleShapes =
            new Dictionary<VehicleViewModel, Rectangle>();

        private readonly Dictionary<PassengerViewModel, Ellipse> _passengerShapes =
            new Dictionary<PassengerViewModel, Ellipse>();

        private readonly DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            _mainCanvas = this.FindControl<Canvas>("MainCanvas") ?? throw new Exception("MainCanvas не найден");
            DataContextChanged += MainWindow_DataContextChanged;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                UpdateVehicles(viewModel);
                UpdatePassengers(viewModel);
            }
        }

        private void MainWindow_DataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.Vehicles.CollectionChanged += Vehicles_CollectionChanged;
                viewModel.Passengers.CollectionChanged += Passengers_CollectionChanged;
                UpdateVehicles(viewModel);
                UpdatePassengers(viewModel);
            }
        }

        private void Vehicles_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (DataContext is not MainWindowViewModel viewModel) return;

            if (e.NewItems != null)
            {
                foreach (VehicleViewModel vehicle in e.NewItems)
                {
                    AddVehicleVisual(vehicle);
                }
            }

            if (e.OldItems != null)
            {
                foreach (VehicleViewModel vehicle in e.OldItems)
                {
                    RemoveVehicleVisual(vehicle);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ClearAllVehicles();
                UpdateVehicles(viewModel);
            }
        }

        private void Passengers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (DataContext is not MainWindowViewModel viewModel) return;

            if (e.NewItems != null)
            {
                foreach (PassengerViewModel passenger in e.NewItems)
                {
                    AddPassengerVisual(passenger);
                }
            }

            if (e.OldItems != null)
            {
                foreach (PassengerViewModel passenger in e.OldItems)
                {
                    RemovePassengerVisual(passenger);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ClearAllPassengers();
                UpdatePassengers(viewModel);
            }
        }

        private void UpdateVehicles(MainWindowViewModel viewModel)
        {
            foreach (var vehicle in viewModel.Vehicles.ToList())
            {
                vehicle.UpdatePosition();
                if (!_vehicleShapes.ContainsKey(vehicle))
                {
                    AddVehicleVisual(vehicle);
                }
                else
                {
                    UpdateVehiclePosition(vehicle);
                }
            }
        }

        private void UpdatePassengers(MainWindowViewModel viewModel)
        {
            foreach (var passenger in viewModel.Passengers.ToList())
            {
                passenger.UpdatePosition();
                if (!_passengerShapes.ContainsKey(passenger))
                {
                    AddPassengerVisual(passenger);
                }
                else
                {
                    UpdatePassengerPosition(passenger);
                }
            }
        }

        private void AddVehicleVisual(VehicleViewModel vehicle)
        {
            var rectangle = new Rectangle
            {
                Width = vehicle is BusViewModel ? 60 : 40,
                Height = 20,
                Fill = vehicle is BusViewModel busVm
                    ? (busVm.IsOvercrowded ? Brushes.Red : Brushes.Yellow)
                    : Brushes.Teal,
                RadiusX = 8,
                RadiusY = 8,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(rectangle, vehicle.X);
            Canvas.SetTop(rectangle, vehicle.Y);

            _mainCanvas.Children.Add(rectangle);
            _vehicleShapes[vehicle] = rectangle;
        }

        private void UpdateVehiclePosition(VehicleViewModel vehicle)
        {
            if (_vehicleShapes.TryGetValue(vehicle, out var rectangle))
            {
                Canvas.SetLeft(rectangle, vehicle.X);
                Canvas.SetTop(rectangle, vehicle.Y);
                if (vehicle is BusViewModel busVm)
                {
                    rectangle.Fill = busVm.IsOvercrowded ? Brushes.Red :
                        busVm.IsStopped ? Brushes.DeepPink : Brushes.HotPink;
                }
            }
        }

        private void RemoveVehicleVisual(VehicleViewModel vehicle)
        {
            if (_vehicleShapes.TryGetValue(vehicle, out var rectangle))
            {
                _mainCanvas.Children.Remove(rectangle);
                _vehicleShapes.Remove(vehicle);
            }
        }

        private void ClearAllVehicles()
        {
            foreach (var rectangle in _vehicleShapes.Values)
            {
                _mainCanvas.Children.Remove(rectangle);
            }

            _vehicleShapes.Clear();
        }

        private void AddPassengerVisual(PassengerViewModel passenger)
        {
            var ellipse = new Ellipse
            {
                Width = 12,
                Height = 12,
                Fill = passenger.IsWaiting ? Brushes.OrangeRed : Brushes.LimeGreen,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(ellipse, passenger.X);
            Canvas.SetTop(ellipse, passenger.Y);

            _mainCanvas.Children.Add(ellipse);
            _passengerShapes[passenger] = ellipse;
        }

        private void UpdatePassengerPosition(PassengerViewModel passenger)
        {
            if (_passengerShapes.TryGetValue(passenger, out var ellipse))
            {
                Canvas.SetLeft(ellipse, passenger.X);
                Canvas.SetTop(ellipse, passenger.Y);
                ellipse.Fill = passenger.IsWaiting ? Brushes.OrangeRed : Brushes.LimeGreen;
            }
        }

        private void RemovePassengerVisual(PassengerViewModel passenger)
        {
            if (_passengerShapes.TryGetValue(passenger, out var ellipse))
            {
                _mainCanvas.Children.Remove(ellipse);
                _passengerShapes.Remove(passenger);
            }
        }

        private void ClearAllPassengers()
        {
            foreach (var ellipse in _passengerShapes.Values)
            {
                _mainCanvas.Children.Remove(ellipse);
            }

            _passengerShapes.Clear();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}