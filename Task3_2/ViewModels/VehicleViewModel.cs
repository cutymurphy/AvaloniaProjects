using Task3_2.Models;

namespace Task3_2.ViewModels
{
    public abstract class VehicleViewModel : ViewModelBase
    {
        public Vehicle Model { get; }

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

        protected VehicleViewModel(Vehicle model)
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

    public class CarViewModel : VehicleViewModel
    {
        public CarViewModel(Car model) : base(model)
        {
        }
    }

    public class BusViewModel : VehicleViewModel
    {
        private readonly Bus _bus;

        private bool _isOvercrowded;
        public bool IsOvercrowded
        {
            get => _isOvercrowded;
            set => SetProperty(ref _isOvercrowded, value);
        }

        public BusViewModel(Bus model) : base(model)
        {
            _bus = model;
            _isOvercrowded = _bus.IsOvercrowded;
            _bus.BusOvercrowded += (s, e) => IsOvercrowded = true;
        }
    }
}