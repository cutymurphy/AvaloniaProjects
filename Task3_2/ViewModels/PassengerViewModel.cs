using Task3_2.Models;

namespace Task3_2.ViewModels
{
    public class PassengerViewModel : ViewModelBase
    {
        public IPassenger Model { get; }

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

        public bool IsWaiting => Model is WaitingPassenger;

        public PassengerViewModel(IPassenger model)
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