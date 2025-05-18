using Task3_2.Models;

namespace Task3_2.ViewModels
{
    public class StopViewModel : ViewModelBase
    {
        public Stop Model { get; }

        public double X => Model.X;
        public double Y => Model.Y;

        public StopViewModel(Stop model)
        {
            Model = model;
        }
    }
}