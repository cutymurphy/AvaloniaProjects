using System;
using System.Linq;
using System.Threading.Tasks;

namespace Task3_2.Models
{
    public class Bus : Vehicle
    {
        private readonly int _maxCapacity = 30;
        private int _currentPassengers;
        private bool _isOvercrowded;
        private readonly double _overcrowdedProbability = 0.2;
        private readonly double _stopProbability = 0.9;

        public event EventHandler BusStopped;
        public event EventHandler BusOvercrowded;
        public event EventHandler BusDeparted;

        public bool IsOvercrowded => _isOvercrowded;

        public Bus(TransportModel model) : base(model)
        {
            _currentPassengers = _random.Next(0, _maxCapacity);
            _isOvercrowded = _random.NextDouble() < _overcrowdedProbability;
        }

        public override async void Update()
        {
            if (_isOvercrowded)
            {
                BusOvercrowded?.Invoke(this, EventArgs.Empty);
                base.Update();
                return;
            }

            Stop stop = FindNearestStop();
            if (stop != null && Math.Abs(X - stop.X) < 20 && !_isOvercrowded)
            {
                bool isStopOccupied = _model.Vehicles.OfType<Bus>()
                    .Any(v => v != this && Math.Abs(v.X - stop.X) < 20 && v._speed == 0);
                if (!isStopOccupied && _random.NextDouble() < _stopProbability)
                {
                    _speed = 0;
                    BusStopped?.Invoke(this, EventArgs.Empty);
                    await PerformBoardingAsync(stop);
                    BusDeparted?.Invoke(this, EventArgs.Empty);
                }
            }

            base.Update();
        }

        private Stop FindNearestStop()
        {
            foreach (var stop in _model.Stops)
            {
                if (stop.IsRightDirection == _movingRight && Math.Abs(X - stop.X) < 50)
                {
                    return stop;
                }
            }
            return null;
        }

        private async Task PerformBoardingAsync(Stop stop)
        {
            var passengers = stop.WaitingPassengers.ToList();
            foreach (var passenger in passengers)
            {
                if (_currentPassengers >= _maxCapacity)
                {
                    _isOvercrowded = true;
                    BusOvercrowded?.Invoke(this, EventArgs.Empty);
                    break;
                }

                if (_random.NextDouble() < 0.7)
                {
                    _currentPassengers++;
                    stop.RemovePassenger(passenger);
                    _model.RemovePassenger(passenger);
                    await Task.Delay(500);
                }
            }

            await Task.Delay(2000);
        }
    }
}