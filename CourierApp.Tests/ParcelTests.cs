using CourierApp.Enums;
using CourierApp.Models;
using FluentAssertions.Execution;
using Xunit;

namespace CourierApp.Tests
{
    public class ParcelTests
    {
        [Fact]
        public void Parcel_GivenDimensions_1x1x1_ShouldReturn_SmallParcel_DeliveryCost_3()
        {
            //act
            var sut = new Parcel(1, 1, 1);

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
            //act
            var sut = new Parcel(10, 15, 20);

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
            //act
            var sut = new Parcel(85, 25, 20);

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
            //act
            var sut = new Parcel(110, 50, 20);

            //assert
            using (new AssertionScope())
            {
                Assert.NotNull(sut);
                Assert.Equal(ParcelType.XL, sut.Type);
                Assert.Equal(25, sut.DeliveryCost);
            }
        }
    }
}
