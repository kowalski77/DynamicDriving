using System;
using FluentAssertions;
using Xunit;

namespace DynamicDriving.BuildingBlocks.Tests.DomainDriven;

public class BaseIdEqualityTests
{
    [Theory]
    [InlineData("FBB6142E-798F-4D42-B210-F5C8509C1C0D", "FBB6142E-798F-4D42-B210-F5C8509C1C0D", true)]
    [InlineData("FBB6142E-798F-4D42-B210-F5C8509C1C0D", "FBB6142E-798F-4D42-B210-F5C8509C1C0E", false)]
    public void Two_BaseId_equality_should_be_correct(string id1, string id2, bool result)
    {
        // Arrange
        var order1 = new OrderId(Guid.Parse(id1));
        var order2 = new OrderId(Guid.Parse(id2));

        // Act
        var equality = order1 == order2;

        // Assert
        equality.Should().Be(result);
    }

    [Fact]
    public void Equality_with_same_object_success()
    {
        // Arrange
        var order1 = new OrderId(Guid.NewGuid());
        var order2 = order1;

        // Act
        var equality = order1 == order2;

        // Assert
        equality.Should().BeTrue();
    }

    [Fact]
    public void Equality_with_null_object_fails()
    {
        // Arrange
        var order = new OrderId(Guid.NewGuid());

        // Act
        var equality = order.Equals(null);

        // Assert
        equality.Should().BeFalse();
    }

    [Fact]
    public void Two_BaseId_equality_with_different_types_fails()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = new OrderId(id);
        var productId = new ProductId(id);

        // Act
        var equality = orderId == productId;

        // Assert
        equality.Should().BeFalse();
    }
}
