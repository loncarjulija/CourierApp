using CourierApp.Enums;
using CourierApp.Models;
using CourierApp.Services;
using FluentAssertions.Execution;
using System.Collections.Generic;
using Xunit;

namespace CourierApp.Tests
{
    public class OrderManagerTests
    {
        private OrderManager Sut { get; }

        public OrderManagerTests()
        {
            Sut = new OrderManager();
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
        public void Parcel_Given_2_MediumParcels_ShouldReturn_TotalCost_16()
        {
            //arrange
            var parcels = new List<Parcel>
            {
                new Parcel(10, 15, 10),
                new Parcel(10, 15, 10)
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
    }
}
