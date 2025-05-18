using System;
using System.Linq;

namespace Task3_2.Models
{
    public class WaitingPassenger : IPassenger
    {
        private readonly TransportModel _model;
        private readonly Random _random = new Random();
        public double X { get; private set; }
        public double Y { get; private set; }
        private readonly double _speed;
        private bool _isWaiting;
        private Stop _targetStop;

        public WaitingPassenger(TransportModel model)
        {
            _model = model;
            _speed = 1.5 + _random.NextDouble() * 2;
            bool movingRight = _random.Next(0, 2) == 0;
            X = movingRight ? -20 : 920;
            bool isTopSidewalk = _random.Next(0, 2) == 0;
            Y = isTopSidewalk
                ? _model.SidewalkTopY + 10 + _random.NextDouble() * 20
                : _model.SidewalkBottomY + 10 + _random.NextDouble() * 20;
            _targetStop = Y < _model.SidewalkBottomY
                ? _model.Stops.First(s => s.Y == _model.SidewalkTopY)
                : _model.Stops.First(s => s.Y == _model.SidewalkBottomY);
        }

        public void Update()
        {
            if (_isWaiting)
            {
                return;
            }

            if (Math.Abs(X - _targetStop.X) < 10)
            {
                _isWaiting = true;
                X = _targetStop.X + _random.Next(-20, 21);
                if (Y < _targetStop.Y)
                    Y = _targetStop.Y + _random.NextDouble() * 40;
                else if (Y > _targetStop.Y + 40)
                    Y = _targetStop.Y + _random.NextDouble() * 40;
                _targetStop.AddPassenger(this);
                return;
            }

            // Move toward the stop
            X += X < _targetStop.X ? _speed : -_speed;
        }
    }
}