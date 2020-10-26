using CourierApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

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

        public Order ProcessOrder(bool isSpeedyShipping = false)
        {
            _order.IsSpeedyShipping = isSpeedyShipping;

            LogOrder();
            return _order;
        }

        //TODO: move to another service with interface
        private void LogOrder()
        {
            var us = new CultureInfo("en-US");
            
            foreach (var parcel in _order.Parcels)
            {
                System.Diagnostics.Debug.WriteLine($"{ parcel.Type} Parcel: {parcel.DeliveryCost.ToString("C", us)}");
            }

            System.Diagnostics.Debug.WriteLine("---------------------------------------------------------------");
            System.Diagnostics.Debug.WriteLine($"Speedy Shipping: {_order.SpeedyShippingCost.ToString("C", us)}");
            System.Diagnostics.Debug.WriteLine($"Total: {_order.TotalCost.ToString("C", us)}");
        }
    }
}
