using System;

namespace Task3_2.Models
{
    public class Pedestrian
    {
        private readonly Random _random = new Random();
        private readonly CrossingModel _crossing;

        public double X { get; private set; }
        public double Y { get; private set; }

        private bool _movingRight; // Для перехода через вертикальную дорогу
        private readonly double _speed;
        private bool _isWaiting;
        private bool _crossingStarted;

        public Pedestrian(CrossingModel crossing)
        {
            _crossing = crossing;
            _speed = _random.Next(1, 3);

            _movingRight = _random.Next(0, 2) == 0;
            X = _movingRight ? _crossing.RoadX - 20 : _crossing.RoadX + _crossing.RoadWidth + 5;
            Y = _crossing.CrossingY + _random.Next(0, (int)_crossing.CrossingHeight);

            _crossingStarted = false;
        }

        public void Update()
        {
            if (!_isWaiting)
            {
                X += _movingRight ? _speed : -_speed;
                if (X < 0 || X > 900)
                {
                    _crossing.RemovePedestrian(this);
                }
            }
        }
    }
}