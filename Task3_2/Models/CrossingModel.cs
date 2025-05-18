using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Threading.Tasks;

namespace Task3_2.Models
{
    public class CrossingModel : IDisposable
    {
        public double RoadY { get; } = 200;
        public double RoadHeight { get; } = 100;

        // Параметры вертикальной дороги и перехода
        public double RoadX { get; } = 400;
        public double RoadWidth { get; } = 100;
        public double CrossingY { get; } = 100;
        public double CrossingHeight { get; } = 80;

        private readonly Timer _updateTimer;
        private readonly Timer _carSpawnTimer;
        private readonly Timer _carSpawnTimerVertical;
        private readonly Timer _pedestrianSpawnTimer;

        public ObservableCollection<Car> Cars { get; }
        public ObservableCollection<Pedestrian> Pedestrians { get; }

        public CrossingModel()
        {
            Cars = new ObservableCollection<Car>();
            Pedestrians = new ObservableCollection<Pedestrian>();

            _updateTimer = new Timer(50);
            _updateTimer.Elapsed += UpdateObjects;
            _updateTimer.AutoReset = true;
            _updateTimer.Start();

            _carSpawnTimer = new Timer(2000);
            _carSpawnTimer.Elapsed += SpawnCarHorizontal;
            _carSpawnTimer.AutoReset = true;
            _carSpawnTimer.Start();

            _pedestrianSpawnTimer = new Timer(3000);
            _pedestrianSpawnTimer.Elapsed += SpawnPedestrian;
            _pedestrianSpawnTimer.AutoReset = true;
            _pedestrianSpawnTimer.Start();
        }

        private void UpdateObjects(object? sender, ElapsedEventArgs e)
        {
            for (int i = Cars.Count - 1; i >= 0; i--)
            {
                if (i < Cars.Count)
                {
                    Cars[i].Update();
                }
            }

            for (int i = Pedestrians.Count - 1; i >= 0; i--)
            {
                if (i < Pedestrians.Count)
                {
                    Pedestrians[i].Update();
                }
            }
        }

        private void SpawnCarHorizontal(object? sender, ElapsedEventArgs e)
        {
            if (Cars.Count >= 10) return;
            var car = new Car(this);
            Cars.Add(car);
        }

        private void SpawnPedestrian(object? sender, ElapsedEventArgs e)
        {
            if (Pedestrians.Count >= 8) return;

            var pedestrian = new Pedestrian(this);
            Pedestrians.Add(pedestrian);
        }

        public void RemoveCar(Car car)
        {
            if (Cars.Contains(car))
            {
                Cars.Remove(car);
            }
        }

        public void RemovePedestrian(Pedestrian pedestrian)
        {
            if (Pedestrians.Contains(pedestrian))
            {
                Pedestrians.Remove(pedestrian);
            }
        }

        public void Dispose()
        {
            _updateTimer.Stop();
            _updateTimer.Dispose();
            _carSpawnTimer.Stop();
            _carSpawnTimer.Dispose();
            _carSpawnTimerVertical.Stop();
            _carSpawnTimerVertical.Dispose();
            _pedestrianSpawnTimer.Stop();
            _pedestrianSpawnTimer.Dispose();
        }
    }
}