using System;

namespace AvaloniaApplication3.Models
{
    public class RegularPassenger : IPassenger
    {
        private readonly TransportModel _model;
        private readonly Random _random = new Random();
        public double X { get; private set; }
        public double Y { get; private set; }
        private readonly double _speed;
        private readonly bool _movingRight;

        public RegularPassenger(TransportModel model)
        {
            _model = model;
            _speed = _random.Next(1, 3);
            _movingRight = _random.Next(0, 2) == 0;
            X = _movingRight ? -20 : 920;
            bool isTopSidewalk = _random.Next(0, 2) == 0;
            Y = isTopSidewalk
                ? _model.SidewalkTopY + 10 + _random.NextDouble() * 20
                : _model.SidewalkBottomY + 10 + _random.NextDouble() * 20;
        }

        public void Update()
        {
            X += _movingRight ? _speed : -_speed;
            if (X < -20 || X > 920)
            {
                _model.RemovePassenger(this);
            }
        }
    }
}