using CourierApp.Models;
using System.Collections.Generic;

namespace CourierApp.Services.Interfaces
{
    public interface IShippingRateProvider
    {
        IList<ShippingRate> GetShippingRates();
    }
}
