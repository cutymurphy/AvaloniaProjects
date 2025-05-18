using System;

namespace Task3_2.Models
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
            X = _movingRight ? -20 : 920; // Появление с левого или правого края
            // Случайный Y в пределах тротуара
            bool isTopSidewalk = _random.Next(0, 2) == 0;
            Y = isTopSidewalk
                ? _model.SidewalkTopY + _random.NextDouble() * 40
                : _model.SidewalkBottomY + _random.NextDouble() * 40;
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