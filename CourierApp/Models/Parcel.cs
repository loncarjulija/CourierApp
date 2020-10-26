using CourierApp.Enums;
using System;

namespace CourierApp.Models
{
    public class Parcel
    {
        public Parcel(double length, double width, double height)
        {
            Type = SetType(length, width, height);
        }

        public ParcelType Type { get; private set; }

        public double DeliveryCost => SetCost();

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

        private double SetCost()
        {
            switch (Type)
            {
                case ParcelType.Small :
                    return 3;
                case ParcelType.Medium :
                    return 8;
                case ParcelType.Large :
                    return 15;
                case ParcelType.XL :
                    return 25;
                default:
                    return 0;
            }            
        }
    }
}
