using System.Collections.Generic;

namespace AvaloniaApplication3.Models
{
    public class Stop
    {
        public double X { get; }
        public double Y { get; }
        public bool IsRightDirection { get; }
        public List<WaitingPassenger> WaitingPassengers { get; } = new List<WaitingPassenger>();

        public Stop(TransportModel model, double x, double y, bool isRightDirection)
        {
            X = x;
            Y = y;
            IsRightDirection = isRightDirection;
        }

        public void AddPassenger(WaitingPassenger passenger)
        {
            WaitingPassengers.Add(passenger);
        }

        public void RemovePassenger(WaitingPassenger passenger)
        {
            WaitingPassengers.Remove(passenger);
        }
    }
}