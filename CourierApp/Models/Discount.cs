using CourierApp.Enums;
using System.Collections.Generic;
using System.Linq;

namespace CourierApp.Models
{
    public class Discount
    {
        public ParcelType ParcelType { get; set; }

        public int DiscountGroup { get; set; }

        public double CalculateDiscount(IList<Parcel> parcels)
        {
            double discount = 0;

            parcels = parcels
                .Where(x => x.Type == ParcelType)
                .OrderBy(x => x.TotalCost)
                .ToList();

            int noOfParcelsToDiscount = parcels.Count() / DiscountGroup;

            for (int i = 0, discounts = 0; discounts < noOfParcelsToDiscount; i += DiscountGroup)
            {
                discount += parcels[i].TotalCost;
                discounts++;
            }

            return discount;
        }
    }
}
