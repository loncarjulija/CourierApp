using CourierApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourierApp.Models
{
    public class Parcel
    {
        public Parcel(double length, double width, double height, double weight = 0)
        {
            WeightKg = weight;
            Type = SetType(length, width, height);
        }

        public ParcelType Type { get; private set; }

        public double WeightKg { get; private set; }

        public double DeliveryCost { get; private set; }

        public double OverweightCost { get; private set; }

        public double TotalCost => DeliveryCost + OverweightCost;

        public void LoadCosts(IList<ShippingRate> shippingRates)
        {
            var shippingRate = shippingRates?.FirstOrDefault(sr => sr.ParcelType == Type);

            if (shippingRate == null)
            {
                throw new Exception($"Provided shipping rates do not contain rate for {Type} parcel.");
            }

            DeliveryCost = shippingRate.DeliveryCost;
            OverweightCost = (WeightKg > shippingRate.WeightLimitInKg) ? (WeightKg - shippingRate.WeightLimitInKg) * shippingRate.OverweightCostPerKg : 0;
        }

        private ParcelType SetType(double length, double width, double height)
        {
            var largestDimension = Math.Max(length, Math.Max(width, height));

            if (largestDimension < 10)
            {
                return ParcelType.Small;
            }
            else if (largestDimension < 50)
            {
                return ParcelType.Medium;
            }
            else if (largestDimension < 100)
            {
                return ParcelType.Large;
            }
            else
            {
                return ParcelType.XL;
            }
        }
    }
}
