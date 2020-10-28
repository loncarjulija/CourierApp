using CourierApp.Enums;
using CourierApp.Factories.Interfaces;
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


        private OrderManager Sut { get; }

        public OrderManagerTests()
        {
            _shippingRates = SetupShippingRates();
            _discounts = SetupDiscounts();

            Sut = new OrderManager();
        }
        
        [Fact]
        public void ProcessOrder_GivenSmallParcel_ShouldReturnTotalCost_3()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel()
                {
                    Type = ParcelType.Small,
                    DeliveryCost = 3     
                }

            };

            var expectedTotalCost = 3;

            //act
            Sut.AddItems(parcels);
            var result = Sut.ProcessOrder();

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(expectedTotalCost, result.TotalCost);
            }
        }

        [Fact]
        public void ProcessOrder_Given_2_MediumParcels_ShouldReturnTotalCost_16()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel()
                {
                    Type = ParcelType.Medium,
                    DeliveryCost = 8
                },
                new Parcel()
                {
                    Type = ParcelType.Medium,
                    DeliveryCost = 8
                }
            };

            var expectedTotalCost = 16;

            //act
            Sut.AddItems(parcels);
            var result = Sut.ProcessOrder();

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(expectedTotalCost, result.TotalCost);
            }
        }      
        
        [Fact]
        public void ProcessOrder_Given_LargeParcel_SpeedyShipping_ShouldReturnTotalCost_30()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel()
                {
                    Type = ParcelType.Large,
                    DeliveryCost = 15
                }
            };

            var expectedTotalCost = 30;
            var expectedSpeedyShippingCost = 15;

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
        }

        [Fact]
        public void ProcessOrder_GivenTwoHeavyParcels_ShouldReturn_TotalCost_128()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel()
                {
                    Type = ParcelType.Heavy,
                    WeightKg = 55,
                    DeliveryCost = 50,
                    OverweightCost = 5
                },
                new Parcel()
                {
                    Type = ParcelType.Heavy,
                    WeightKg = 73,
                    DeliveryCost = 50,
                    OverweightCost = 23
                }
            };

            var expectedTotalCost = 128;

            //act
            Sut.AddItems(parcels);
            var result = Sut.ProcessOrder();

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(expectedTotalCost, result.TotalCost);
            }
        }

        [Fact]
        public void ProcessOrder_GivenFourSmallParcels_ShouldDiscountCheapestOne()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel()
                {
                    Type = ParcelType.Small,
                    DeliveryCost = 3
                },
                new Parcel()
                {
                    Type = ParcelType.Small,
                    DeliveryCost = 3
                },
                new Parcel()
                {
                    Type = ParcelType.Small,
                    WeightKg = 2,
                    DeliveryCost = 3,
                    OverweightCost = 2
                },
                new Parcel()
                {
                    Type = ParcelType.Small,
                    DeliveryCost = 3
                }               
            };

            var expectedTotalCost = 11;
            var expectedDiscount = 3;

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
        }


        [Fact]
        public void ProcessOrder_GivenFourMediumParcels_ShouldDiscountCheapestOne()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel()
                {
                    Type = ParcelType.Medium,
                    DeliveryCost = 8
                },
                new Parcel()
                {
                    Type = ParcelType.Medium,
                    DeliveryCost = 8
                },
                new Parcel()
                {
                    Type = ParcelType.Medium,
                    WeightKg = 5,
                    DeliveryCost = 8,
                    OverweightCost = 4
                },
                new Parcel()
                {
                    Type = ParcelType.Medium,
                    DeliveryCost = 8
                }
            };

            var expectedTotalCost = 28;
            var expectedDiscount = 8;

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
        }


        [Fact]
        public void ProcessOrder_SpeedyShipping_ShouldApplySpeedyShippingAfterDiscountsAreApplied()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel()
                {
                    Type = ParcelType.Medium,
                    DeliveryCost = 8
                },
                new Parcel()
                {
                    Type = ParcelType.Medium,
                    DeliveryCost = 8
                },
                new Parcel()
                {
                    Type = ParcelType.Medium,
                    WeightKg = 5,
                    DeliveryCost = 8,
                    OverweightCost = 4
                },
                new Parcel()
                {
                    Type = ParcelType.Medium,
                    DeliveryCost = 8
                }
            };

            var expectedDiscount = 8;

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
