using CourierApp.Factories.Interfaces;
using CourierApp.Models;
using CourierApp.Services.Interfaces;
using System;
using System.Linq;

namespace CourierApp.Factories
{
    public class ParcelFactory : IParcelFactory
    {
        private readonly IShippingRateProvider _shippingRateProvider;

        public ParcelFactory(IShippingRateProvider shippingRateProvider)
        {
            _shippingRateProvider = shippingRateProvider;
        }

        public Parcel Build(double length, double width, double height, double weightKg = 0)
        {
            if (weightKg > 50)
            {
                var parcel = new Parcel()
                {
                    Type = Enums.ParcelType.Heavy,
                    WeightKg = weightKg
                };

                return LoadCosts(parcel);
            }

            var largestDimension = Math.Max(length, Math.Max(width, height));

            if (largestDimension < 10)
            {
                var parcel = new Parcel()
                {
                    Type = Enums.ParcelType.Small,
                    WeightKg = weightKg
                };

                return LoadCosts(parcel);
            }
            else if (largestDimension < 50)
            {   
                var parcel = new Parcel()
                {
                    Type = Enums.ParcelType.Medium,
                    WeightKg = weightKg
                };

                return LoadCosts(parcel);
            }
            else if (largestDimension < 100)
            {
                var parcel = new Parcel()
                {
                    Type = Enums.ParcelType.Large,
                    WeightKg = weightKg
                };

                return LoadCosts(parcel);
            }
            else
            {
                var parcel = new Parcel()
                {
                    Type = Enums.ParcelType.XL,
                    WeightKg = weightKg
                };

                return LoadCosts(parcel);
            }
        }

        private Parcel LoadCosts(Parcel parcel)
        {
            var shippingRate = _shippingRateProvider.GetShippingRates()?.FirstOrDefault(x => x.ParcelType == parcel.Type);

            if (shippingRate == null)
            {
                throw new Exception($"Shipping rates don't contain rate for parcel type {parcel.Type}.");
            }

            parcel.DeliveryCost = shippingRate.DeliveryCost;
            parcel.OverweightCost = parcel.WeightKg > shippingRate.WeightLimitInKg ? (parcel.WeightKg - shippingRate.WeightLimitInKg) * shippingRate.OverweightCostPerKg : 0;            

            return parcel;
        }
    }   
}
