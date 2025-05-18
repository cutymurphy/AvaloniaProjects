using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;

namespace Task3_2.Models
{
    public class TransportModel : IDisposable
    {
        public double RoadY { get; } = 200;
        public double RoadHeight { get; } = 100;
        public double SidewalkTopY { get; } = 160;
        public double SidewalkBottomY { get; } = 300;

        public ObservableCollection<Vehicle> Vehicles { get; }
        public ObservableCollection<IPassenger> Passengers { get; }
        public ObservableCollection<Stop> Stops { get; }

        private readonly Timer _updateTimer;
        private readonly Timer _vehicleSpawnTimer;
        private readonly Timer _passengerSpawnTimer;
        private readonly Random _random = new Random();

        public TransportModel()
        {
            Vehicles = new ObservableCollection<Vehicle>();
            Passengers = new ObservableCollection<IPassenger>();
            Stops = new ObservableCollection<Stop>
            {
                new Stop(this, 450, SidewalkTopY, false),
                new Stop(this, 450, SidewalkBottomY, true)
            };

            _updateTimer = new Timer(50);
            _updateTimer.Elapsed += UpdateObjects;
            _updateTimer.AutoReset = true;
            _updateTimer.Start();

            _vehicleSpawnTimer = new Timer(2000);
            _vehicleSpawnTimer.Elapsed += SpawnVehicle;
            _vehicleSpawnTimer.AutoReset = true;
            _vehicleSpawnTimer.Start();

            _passengerSpawnTimer = new Timer(3000);
            _passengerSpawnTimer.Elapsed += SpawnPassenger;
            _passengerSpawnTimer.AutoReset = true;
            _passengerSpawnTimer.Start();
        }

        private void UpdateObjects(object? sender, ElapsedEventArgs e)
        {
            foreach (var vehicle in Vehicles.ToList())
            {
                vehicle.Update();
            }

            foreach (var passenger in Passengers.ToList())
            {
                passenger.Update();
            }
        }

        private void SpawnVehicle(object? sender, ElapsedEventArgs e)
        {
            if (Vehicles.Count >= 10) return;
            Vehicle vehicle = _random.NextDouble() < 0.3 ? new Bus(this) : new Car(this);
            Vehicles.Add(vehicle);
        }

        private void SpawnPassenger(object? sender, ElapsedEventArgs e)
        {
            if (Passengers.Count >= 8) return;

            bool spawnWaitingPassenger = _random.NextDouble() < 0.6;
            Type passengerType = spawnWaitingPassenger ? typeof(WaitingPassenger) : typeof(RegularPassenger);

            var passenger = (IPassenger)Activator.CreateInstance(passengerType, this);
            Passengers.Add(passenger);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            Vehicles.Remove(vehicle);
        }

        public void RemovePassenger(IPassenger passenger)
        {
            Passengers.Remove(passenger);
        }

        public void Dispose()
        {
            _updateTimer.Stop();
            _updateTimer.Dispose();
            _vehicleSpawnTimer.Stop();
            _vehicleSpawnTimer.Dispose();
            _passengerSpawnTimer.Stop();
            _passengerSpawnTimer.Dispose();
        }
    }
}