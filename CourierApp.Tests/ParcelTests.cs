using CourierApp.Enums;
using CourierApp.Models;
using FluentAssertions.Execution;
using System.Collections.Generic;
using Xunit;

namespace CourierApp.Tests
{
    public class ParcelTests
    {
        private readonly IList<ShippingRate> _shippingRates = new List<ShippingRate>();

        public ParcelTests()
        {
            _shippingRates = LoadShippingRates();
        }
        [Fact]
        public void Parcel_GivenDimensions_1x1x1_ShouldReturn_SmallParcel_DeliveryCost_3()
        {
            //arrange
            var sut = new Parcel(1, 1, 1);

            //act
            sut.LoadCosts(_shippingRates);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(sut);
                Assert.Equal(ParcelType.Small, sut.Type);
                Assert.Equal(3, sut.DeliveryCost);
            }
        }

        [Fact]
        public void Parcel_GivenLargestDimensionLessThan50_ShouldReturn_MediumParcel_DeliveryCost_8()
        {
            //arrange
            var sut = new Parcel(10, 15, 20);

            //act
            sut.LoadCosts(_shippingRates);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(sut);
                Assert.Equal(ParcelType.Medium, sut.Type);
                Assert.Equal(8, sut.DeliveryCost);
            }
        }

        [Fact]
        public void Parcel_GivenLargestDimensionLessThan100_ShouldReturn_LargeParcel_DeliveryCost_15()
        {
            //arrange
            var sut = new Parcel(85, 25, 20);

            //act
            sut.LoadCosts(_shippingRates);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(sut);
                Assert.Equal(ParcelType.Large, sut.Type);
                Assert.Equal(15, sut.DeliveryCost);
            }
        }

        [Fact]
        public void Parcel_GivenLargestDimensionGreaterThan100_ShouldReturn_LargeParcel_DeliveryCost_25()
        {
            //arrange
            var sut = new Parcel(110, 50, 20);

            //act
            sut.LoadCosts(_shippingRates);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(sut);
                Assert.Equal(ParcelType.XL, sut.Type);
                Assert.Equal(25, sut.DeliveryCost);
            }
        }

        [Fact]
        public void Parcel_Given_LargeParcel_Overweight_2kg_ShouldReturnTotalCost_19()
        {
            //arrange
            var expectedDeliveryCost = 15;
            var expectedOverweightCost = 4;
            var expectedTotalCost = 19;

            var sut = new Parcel(75, 50, 10, 8);

            //act
            sut.LoadCosts(_shippingRates);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(sut);
                Assert.Equal(ParcelType.Large, sut.Type);
                Assert.Equal(expectedDeliveryCost, sut.DeliveryCost);
                Assert.Equal(expectedOverweightCost, sut.OverweightCost);
                Assert.Equal(expectedTotalCost, sut.TotalCost);
            }
        }

        [Fact]
        public void Parcel_GivenParcelWeight_55kg_ShouldReturnH_HeavyParcel_TotalCost_55()
        {
            //arrange
            var expectedDeliveryCost = 50;
            var expectedOverweightCost = 5;
            var expectedTotalCost = 55;

            var sut = new Parcel(75, 50, 10, 55);

            //act
            sut.LoadCosts(_shippingRates);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(sut);
                Assert.Equal(ParcelType.Heavy, sut.Type);
                Assert.Equal(expectedDeliveryCost, sut.DeliveryCost);
                Assert.Equal(expectedOverweightCost, sut.OverweightCost);
                Assert.Equal(expectedTotalCost, sut.TotalCost);
            }
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
    }
}
