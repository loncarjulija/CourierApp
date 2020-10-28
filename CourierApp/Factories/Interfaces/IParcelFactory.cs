using CourierApp.Models;

namespace CourierApp.Factories.Interfaces
{
    public interface IParcelFactory
    {
        Parcel Build(double length, double width, double height, double weightKg = 0);
    }
}
