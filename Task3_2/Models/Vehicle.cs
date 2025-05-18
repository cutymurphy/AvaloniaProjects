using System;
using System.Linq;

namespace Task3_2.Models
{
    public abstract class Vehicle
    {
        protected readonly Random _random = new Random();
        protected readonly TransportModel _model;

        public double X { get; protected set; }
        public double Y { get; protected set; }
        protected double _speed;
        protected readonly double _maxSpeed;
        protected bool _movingRight;
        protected int _lane;
        protected readonly double _stopDistance = 150;

        protected Vehicle(TransportModel model)
        {
            _model = model;
            _maxSpeed = _random.Next(3, 5);
            _lane = _random.Next(0, 2); // Случайная полоса для автобусов и машин
            _movingRight = _lane == 1; // lane 0: справа налево, lane 1: слева направо
            X = _movingRight ? -50 : 950;
            Y = _model.RoadY + (_lane == 0 ? 20 : 60); // lane 0: Y=220, lane 1: Y=260
            _speed = _maxSpeed;
        }

        public virtual void Update()
        {
            _speed = _maxSpeed;

            Vehicle vehicleAhead = FindVehicleAhead();
            if (vehicleAhead != null)
            {
                double distance = _movingRight ? vehicleAhead.X - X - 40 : X - vehicleAhead.X - 40;
                if (distance < 20)
                {
                    _speed = 0;
                }
            }

            X += _movingRight ? _speed : -_speed;
            if (X < -60 || X > 960)
            {
                _model.RemoveVehicle(this);
            }
        }

        protected Vehicle FindVehicleAhead()
        {
            Vehicle closest = null;
            double minDistance = double.MaxValue;

            foreach (var vehicle in _model.Vehicles.ToList())
            {
                if (vehicle == this || vehicle._lane != _lane) continue;
                double distance = _movingRight ? vehicle.X - X : X - vehicle.X;
                if (distance > 0 && distance < minDistance)
                {
                    minDistance = distance;
                    closest = vehicle;
                }
            }

            return closest;
        }
    }
}