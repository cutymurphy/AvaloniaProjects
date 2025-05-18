using System;

namespace Task3_2.Models
{
    public class Car
    {
        private readonly Random _random = new Random();
        private readonly CrossingModel? _crossing; // Добавляем ?

        public double X { get; private set; }
        public double Y { get; private set; }

        private double _speed;
        private readonly double _maxSpeed;

        private bool _movingRight;

        private int _lane;

        private readonly double _stopDistance = 150;

        public Car(CrossingModel? crossing)
        {
            _crossing = crossing;
            _maxSpeed = _random.Next(3, 5);
            _lane = _random.Next(0, 2);

            _movingRight = _lane == 0;
            X = _movingRight ? -50 : 950;
            double laneOffset = _lane == 0 ? 20 : 60;
            Y = _crossing != null ? _crossing.RoadY + laneOffset : 200;

            _speed = _maxSpeed;
        }

        public void Update()
        {
            if (_crossing == null)
            {
                X += _movingRight ? _speed : -_speed;
                return;
            }

            _speed = _maxSpeed;

            Car? carAhead = FindCarAhead();
            if (carAhead != null)
            {
                double distance = _movingRight ? carAhead.X - X - 40 : X - carAhead.X - 40;

                if (distance < 20)
                {
                    _speed = 0;
                }
            }

            X += _movingRight ? _speed : -_speed;
            if (X < -60 || X > 960)
            {
                _crossing.RemoveCar(this);
                return;
            }
        }

        private Car? FindCarAhead()
        {
            Car? closest = null;
            double minDistance = double.MaxValue;
            return closest;
        }
    }
}