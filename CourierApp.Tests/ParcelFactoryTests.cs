using CourierApp.Enums;
using CourierApp.Factories;
using CourierApp.Models;
using CourierApp.Services.Interfaces;
using FluentAssertions.Execution;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CourierApp.Tests
{
    public class ParcelFactoryTests
    {
        private readonly Mock<IShippingRateProvider> _shippingRateProviderMock;
        private readonly IList<ShippingRate> _shippingRates;

        private ParcelFactory Sut { get; }

        public ParcelFactoryTests()
        {
            _shippingRateProviderMock = new Mock<IShippingRateProvider>(MockBehavior.Strict);
            _shippingRates = SetupShippingRates();

            Sut = new ParcelFactory(_shippingRateProviderMock.Object);
        }

        [Fact]
        public void Parcel_GivenDimensions_1x1x1_ShouldReturn_SmallParcel_DeliveryCost_3()
        {
            //arrange
            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            var result = Sut.Build(1, 1, 1);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(ParcelType.Small, result.Type);
                Assert.Equal(3, result.DeliveryCost);
            }
        }

        [Fact]
        public void Parcel_GivenLargestDimensionLessThan50_ShouldReturn_MediumParcel_DeliveryCost_8()
        {
            //arrange
            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            var result = Sut.Build(10, 15, 20);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(ParcelType.Medium, result.Type);
                Assert.Equal(8, result.DeliveryCost);
            }
        }

        [Fact]
        public void Parcel_GivenLargestDimensionLessThan100_ShouldReturn_LargeParcel_DeliveryCost_15()
        {
            //arrange
            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            var result = Sut.Build(85, 25, 20);
           
            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(ParcelType.Large, result.Type);
                Assert.Equal(15, result.DeliveryCost);
            }
        }

        [Fact]
        public void Parcel_GivenLargestDimensionGreaterThan100_ShouldReturn_LargeParcel_DeliveryCost_25()
        {
            //arrange
            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            var result = Sut.Build(110, 50, 20);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(ParcelType.XL, result.Type);
                Assert.Equal(25, result.DeliveryCost);
            }
        }

        [Fact]
        public void Parcel_Given_LargeParcel_Overweight_2kg_ShouldReturnTotalCost_19()
        {
            //arrange
            var expectedDeliveryCost = 15;
            var expectedOverweightCost = 4;
            var expectedTotalCost = 19;

            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            var result = Sut.Build(75, 50, 10, 8);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(ParcelType.Large, result.Type);
                Assert.Equal(expectedDeliveryCost, result.DeliveryCost);
                Assert.Equal(expectedOverweightCost, result.OverweightCost);
                Assert.Equal(expectedTotalCost, result.TotalCost);
            }
        }

        [Fact]
        public void Parcel_GivenParcelWeight_55kg_ShouldReturnH_HeavyParcel_TotalCost_55()
        {
            //arrange
            var expectedDeliveryCost = 50;
            var expectedOverweightCost = 5;
            var expectedTotalCost = 55;

            _shippingRateProviderMock.Setup(m => m.GetShippingRates())
                .Returns(_shippingRates);

            //act
            var result = Sut.Build(75, 50, 10, 55);          

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                Assert.Equal(ParcelType.Heavy, result.Type);
                Assert.Equal(expectedDeliveryCost, result.DeliveryCost);
                Assert.Equal(expectedOverweightCost, result.OverweightCost);
                Assert.Equal(expectedTotalCost, result.TotalCost);
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
    }
}
