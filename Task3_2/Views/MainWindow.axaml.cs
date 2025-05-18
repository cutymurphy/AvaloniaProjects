using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Task3_2.ViewModels;
using System.Linq;

namespace Task3_2.Views
{
    public partial class MainWindow : Window
    {
        private Canvas? _mainCanvas;
        private readonly Dictionary<CarViewModel, Rectangle> _carShapes = new Dictionary<CarViewModel, Rectangle>();

        private readonly Dictionary<PedestrianViewModel, Ellipse> _pedestrianShapes =
            new Dictionary<PedestrianViewModel, Ellipse>();

        private readonly DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            _mainCanvas = this.FindControl<Canvas>("MainCanvas");
            if (_mainCanvas == null)
            {
                throw new Exception("MainCanvas не найден в MainWindow.axaml");
            }

            this.DataContextChanged += MainWindow_DataContextChanged;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!(DataContext is MainWindowViewModel viewModel))
                return;

            UpdateCars(viewModel);
            UpdatePedestrians(viewModel);
        }

        private void MainWindow_DataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.Cars.CollectionChanged += Cars_CollectionChanged;
                viewModel.Pedestrians.CollectionChanged += Pedestrians_CollectionChanged;

                UpdateCars(viewModel);
                UpdatePedestrians(viewModel);
            }
        }

        private void Cars_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(DataContext is MainWindowViewModel viewModel))
                return;

            if (e.NewItems != null)
            {
                foreach (CarViewModel car in e.NewItems)
                {
                    AddCarVisual(car);
                }
            }

            if (e.OldItems != null)
            {
                foreach (CarViewModel car in e.OldItems)
                {
                    RemoveCarVisual(car);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ClearAllCars();
                UpdateCars(viewModel);
            }
        }

        private void Pedestrians_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(DataContext is MainWindowViewModel viewModel))
                return;

            if (e.NewItems != null)
            {
                foreach (PedestrianViewModel pedestrian in e.NewItems)
                {
                    AddPedestrianVisual(pedestrian);
                }
            }

            if (e.OldItems != null)
            {
                foreach (PedestrianViewModel pedestrian in e.OldItems)
                {
                    RemovePedestrianVisual(pedestrian);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ClearAllPedestrians();
                UpdatePedestrians(viewModel);
            }
        }

        private void UpdateCars(MainWindowViewModel viewModel)
        {
            var carsToUpdate = viewModel.Cars.ToList();
            foreach (var car in carsToUpdate)
            {
                car.UpdatePosition();
                if (!_carShapes.ContainsKey(car))
                {
                    AddCarVisual(car);
                }
                else
                {
                    UpdateCarPosition(car);
                }
            }
        }

        private void UpdatePedestrians(MainWindowViewModel viewModel)
        {
            var pedestriansToUpdate = viewModel.Pedestrians.ToList();
            foreach (var pedestrian in pedestriansToUpdate)
            {
                pedestrian.UpdatePosition();
                if (!_pedestrianShapes.ContainsKey(pedestrian))
                {
                    AddPedestrianVisual(pedestrian);
                }
                else
                {
                    UpdatePedestrianPosition(pedestrian);
                }
            }
        }

        private void AddCarVisual(CarViewModel car)
        {
            var rectangle = new Rectangle
            {
                Width = 40,
                Height = 20,
                Fill = Brushes.Teal,
                RadiusX = 8,
                RadiusY = 8,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(rectangle, car.X);
            Canvas.SetTop(rectangle, car.Y);

            _mainCanvas?.Children.Add(rectangle);
            _carShapes[car] = rectangle;
        }

        private void UpdateCarPosition(CarViewModel car)
        {
            if (_carShapes.TryGetValue(car, out Rectangle? rectangle) && rectangle != null)
            {
                Canvas.SetLeft(rectangle, car.X);
                Canvas.SetTop(rectangle, car.Y);
            }
        }

        private void RemoveCarVisual(CarViewModel car)
        {
            if (_carShapes.TryGetValue(car, out Rectangle? rectangle) && rectangle != null)
            {
                _mainCanvas?.Children.Remove(rectangle);
                _carShapes.Remove(car);
            }
        }

        private void ClearAllCars()
        {
            foreach (var rectangle in _carShapes.Values)
            {
                _mainCanvas?.Children.Remove(rectangle);
            }

            _carShapes.Clear();
        }

        private void AddPedestrianVisual(PedestrianViewModel pedestrian)
        {
            var ellipse = new Ellipse
            {
                Width = 12,
                Height = 12,
                Fill = Brushes.LimeGreen,
                Stroke = Brushes.DarkGreen,
                StrokeThickness = 2
            };

            Canvas.SetLeft(ellipse, pedestrian.X);
            Canvas.SetTop(ellipse, pedestrian.Y);

            _mainCanvas?.Children.Add(ellipse);
            _pedestrianShapes[pedestrian] = ellipse;
        }

        private void UpdatePedestrianPosition(PedestrianViewModel pedestrian)
        {
            if (_pedestrianShapes.TryGetValue(pedestrian, out Ellipse? ellipse) && ellipse != null)
            {
                Canvas.SetLeft(ellipse, pedestrian.X);
                Canvas.SetTop(ellipse, pedestrian.Y);
            }
        }

        private void RemovePedestrianVisual(PedestrianViewModel pedestrian)
        {
            if (_pedestrianShapes.TryGetValue(pedestrian, out Ellipse? ellipse) && ellipse != null)
            {
                _mainCanvas?.Children.Remove(ellipse);
                _pedestrianShapes.Remove(pedestrian);
            }
        }

        private void ClearAllPedestrians()
        {
            foreach (var ellipse in _pedestrianShapes.Values)
            {
                _mainCanvas?.Children.Remove(ellipse);
            }

            _pedestrianShapes.Clear();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}