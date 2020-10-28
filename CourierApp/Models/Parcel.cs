using CourierApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourierApp.Models
{
    public class Parcel
    {
        public ParcelType Type { get; set; }

        public double WeightKg { get; set; }

        public double DeliveryCost { get; set; }

        public double OverweightCost { get; set; }

        public double TotalCost => DeliveryCost + OverweightCost;
    }
}
