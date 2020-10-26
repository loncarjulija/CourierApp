using System.Collections.Generic;
using System.Linq;

namespace CourierApp.Models
{
    public class Order
    {
        public IList<Parcel> Parcels { get; set; }

        public double TotalCost => Parcels.Sum(p => p.DeliveryCost);
    }
}
