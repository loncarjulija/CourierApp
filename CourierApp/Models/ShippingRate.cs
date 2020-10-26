using CourierApp.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierApp.Models
{
    public class ShippingRate
    {
        public ParcelType ParcelType{ get; set; }

        public double DeliveryCost { get; set; }

        public int WeightLimitInKg { get; set; }

        public double OverweightCostPerKg { get; set; }
    }
}
