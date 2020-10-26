using CourierApp.Models;
using System.Collections.Generic;

namespace CourierApp.Services.Interfaces
{
    public interface IOrderManager
    {
        void AddItems(IList<Parcel> parcels);

        Order ProcessOrder(bool isSpeedyShipping, IList<Discount> discounts);
    }
}
