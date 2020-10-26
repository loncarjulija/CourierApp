using CourierApp.Enums;
using CourierApp.Models;
using CourierApp.Services;
using CourierApp.Services.Interfaces;
using FluentAssertions.Execution;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CourierApp.Tests
{
    public class OrderManagerTests
    {
        private readonly IList<ShippingRate> _shippingRates;
        private readonly IList<Discount> _discounts;
        private readonly Mock<IShippingRateProvider> _shippingRateProviderMock;


        private OrderManager Sut { get; }

        public OrderManagerTests()
        {
            _shippingRateProviderMock = new Mock<IShippingRateProvider>(MockBehavior.Strict);
            _shippingRates = SetupShippingRates();
            _discounts = SetupDiscounts();

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

        [Fact]
        public void ProcessOrder_GivenTwoHeavyParcels_ShouldReturn_TotalCost_128()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel(75, 50, 10, 55),
                new Parcel(75, 50, 10, 73)
            };

            var expectedTotalCost = 128;

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
        public void ProcessOrder_GivenFourSmallParcels_ShouldDiscountCheapestOne()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel(1, 1, 3),  //3
                new Parcel(1, 2, 3),  //3
                new Parcel(3, 4, 5, 2), //5
                new Parcel(5, 6, 7)  //3
            };

            var expectedTotalCost = 11;
            var expectedDiscount = 3;

            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            Sut.AddItems(parcels);
            var result = Sut.ProcessOrder(false, _discounts);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(expectedTotalCost, result.TotalCost);
                Assert.Equal(expectedDiscount, result.Discount);
            }

            _shippingRateProviderMock.VerifyAll();
        }


        [Fact]
        public void ProcessOrder_GivenFourMediumParcels_ShouldDiscountCheapestOne()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel(12, 1, 3), //8
                new Parcel(13, 2, 3),  //8
                new Parcel(33, 4, 5, 5), //12
                new Parcel(12, 6, 7) //8
            };

            var expectedTotalCost = 28;
            var expectedDiscount = 8;

            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            Sut.AddItems(parcels);
            var result = Sut.ProcessOrder(false, _discounts);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(expectedTotalCost, result.TotalCost);
                Assert.Equal(expectedDiscount, result.Discount);
            }

            _shippingRateProviderMock.VerifyAll();
        }


        [Fact]
        public void ProcessOrder_SpeedyShipping_ShouldApplySpeedyShippingAfterDiscountsAreApplied()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel(12, 1, 3), //8
                new Parcel(13, 2, 3),  //8
                new Parcel(33, 4, 5, 5), //12
                new Parcel(12, 6, 7) //8
            };

            var expectedDiscount = 8;

            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            Sut.AddItems(parcels);
            var result = Sut.ProcessOrder(true, _discounts);

             //assert
            var expectedTotalCost = (result.Parcels.Sum(p => p.TotalCost) * 2) - (expectedDiscount * 2);

            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(expectedTotalCost, result.TotalCost);
                Assert.Equal(expectedDiscount, result.Discount);
            }

            _shippingRateProviderMock.VerifyAll();
        }

        
        private static IList<ShippingRate> SetupShippingRates()
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
                },
                new ShippingRate 
                {
                    ParcelType = ParcelType.Heavy,
                    DeliveryCost = 50,
                    WeightLimitInKg = 50,
                    OverweightCostPerKg = 1
                },
            };
        }

        private IList<Discount> SetupDiscounts()
        {
            return new List<Discount>
            {
                new Discount()
                {
                    ParcelType = ParcelType.Small,
                    DiscountGroup = 4
                },
                new Discount()
                {
                    ParcelType = ParcelType.Medium,
                    DiscountGroup = 3
                }
            };
        }
    }
}
