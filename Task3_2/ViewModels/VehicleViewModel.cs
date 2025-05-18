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
        private bool _isStopped;
        public bool IsOvercrowded
        {
            get => _isOvercrowded;
            set => SetProperty(ref _isOvercrowded, value);
        }
        public bool IsStopped
        {
            get => _isStopped;
            set => SetProperty(ref _isStopped, value);
        }

        public BusViewModel(Bus model) : base(model)
        {
            _bus = model;
            _isOvercrowded = _bus.IsOvercrowded;
            _bus.BusOvercrowded += (s, e) => IsOvercrowded = true;
            _bus.BusStopped += (s, e) => IsStopped = true;
            _bus.BusDeparted += (s, e) => IsStopped = false;
        }
    }
}