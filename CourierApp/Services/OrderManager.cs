using CourierApp.Models;
using CourierApp.Services.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CourierApp.Services
{
    //Note: one instance of OrderManager is connected with one Order
    public class OrderManager : IOrderManager
    {
        private readonly Order _order = new Order();

        public void AddItems(IList<Parcel> parcels)
        {
            _order.Parcels = parcels;
        }

        public Order ProcessOrder(bool isSpeedyShipping = false, IList<Discount> discounts = null)
        {
            _order.IsSpeedyShipping = isSpeedyShipping;

            _order.Discount = discounts != null ? discounts.Sum(x => x.CalculateDiscount(_order.Parcels)) : 0;

            LogOrder();
            return _order;
        }

        //TODO: move to another service with interface
        private void LogOrder()
        {
            var us = new CultureInfo("en-US");
            
            foreach (var parcel in _order.Parcels)
            {
                System.Diagnostics.Debug.WriteLine($"{ parcel.Type} Parcel: {parcel.TotalCost.ToString("C", us)}");
            }

            System.Diagnostics.Debug.WriteLine("---------------------------------------------------------------");
            System.Diagnostics.Debug.WriteLine($"Discount: {_order.Discount.ToString("C", us)}");
            System.Diagnostics.Debug.WriteLine($"Speedy Shipping: {_order.SpeedyShippingCost.ToString("C", us)}");
            System.Diagnostics.Debug.WriteLine($"Total: {_order.TotalCost.ToString("C", us)}");
        }
    }
}
