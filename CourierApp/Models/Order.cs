using System.Collections.Generic;
using System.Linq;

namespace CourierApp.Models
{
    public class Order
    {
        public IList<Parcel> Parcels { get; set; }

        public bool IsSpeedyShipping { get; set; }

        public double Discount { get; set; }

        public double TotalCost => GetTotalCost(); 

        public double SpeedyShippingCost => IsSpeedyShipping ? TotalCost / 2 : 0;

        private double GetTotalCost()
        {
            var totalCost = Parcels.Sum(x => x.TotalCost) - Discount;

            return IsSpeedyShipping ? 2 * totalCost : totalCost;
        }
    }
}
