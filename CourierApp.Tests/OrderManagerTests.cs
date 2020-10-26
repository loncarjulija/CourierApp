using CourierApp.Enums;
using CourierApp.Models;
using CourierApp.Services;
using CourierApp.Services.Interfaces;
using FluentAssertions.Execution;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CourierApp.Tests
{
    public class OrderManagerTests
    {
        private readonly Mock<IShippingRateProvider> _shippingRateProviderMock;
        private readonly IList<ShippingRate> _shippingRates;

        private OrderManager Sut { get; }

        public OrderManagerTests()
        {
            _shippingRateProviderMock = new Mock<IShippingRateProvider>(MockBehavior.Strict);
            _shippingRates = LoadShippingRates();

            Sut = new OrderManager(_shippingRateProviderMock.Object);
        }
        
        [Fact]
        public void ProcessOrder_GivenSmallParcel_ShouldReturnTotalCost_3()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel(1, 1, 1)
            };

            var expectedTotalCost = 3;

            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            Sut.AddItems(parcels);
            var result = Sut.ProcessOrder();

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(expectedTotalCost, result.TotalCost);
            }

            _shippingRateProviderMock.VerifyAll();
        }

        [Fact]
        public void ProcessOrder_Given_2_MediumParcels_ShouldReturnTotalCost_16()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel(10, 15, 10),
                new Parcel(10, 15, 10)
            };

            var expectedTotalCost = 16;

            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            Sut.AddItems(parcels);
            var result = Sut.ProcessOrder();

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(expectedTotalCost, result.TotalCost);
            }

            _shippingRateProviderMock.VerifyAll();
        }      
        
        [Fact]
        public void ProcessOrder_Given_LargeParcel_SpeedyShipping_ShouldReturnTotalCost_30()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel(75, 50, 10)
            };

            var expectedTotalCost = 30;
            var expectedSpeedyShippingCost = 15;

            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            Sut.AddItems(parcels);
            var result = Sut.ProcessOrder(true);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(expectedTotalCost, result.TotalCost);
                Assert.Equal(expectedSpeedyShippingCost, result.SpeedyShippingCost);
            }

            _shippingRateProviderMock.VerifyAll();
        }
        
        private static IList<ShippingRate> LoadShippingRates()
        {
            return new List<ShippingRate>
            {
                new ShippingRate 
                {
                    ParcelType = ParcelType.Small,
                    DeliveryCost = 3,
                    WeightLimitInKg = 1,
                    OverweightCostPerKg = 2
                },
                new ShippingRate 
                {
                    ParcelType = ParcelType.Medium,
                    DeliveryCost = 8,
                    WeightLimitInKg = 3,
                    OverweightCostPerKg = 2
                },
                new ShippingRate 
                {
                    ParcelType = ParcelType.Large,
                    DeliveryCost = 15,
                    WeightLimitInKg = 6,
                    OverweightCostPerKg = 2
                },
                new ShippingRate 
                {
                    ParcelType = ParcelType.XL,
                    DeliveryCost = 25,
                    WeightLimitInKg = 10,
                    OverweightCostPerKg = 2
                }
            };
        }
    }
}
