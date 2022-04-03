using System;
using FluentAssertions;
using Xunit;

namespace DynamicDriving.BuildingBlocks.Tests.DomainDriven;

public class StronglyTypeIdTests
{
    [Fact]
    public void Same_values_are_equal()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId1 = new OrderId(id);
        var orderId2 = new OrderId(id);

        // Act
        var result = orderId1.Equals(orderId2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Different_values_are_not_equal()
    {
        // Arrange
        var orderId1 = new OrderId(Guid.NewGuid());
        var orderId2 = new OrderId(Guid.NewGuid());

        // Act
        var result = orderId1.Equals(orderId2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equal_operator_works_correctly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId1 = new OrderId(id);
        var orderId2 = new OrderId(id);

        // Act
        var result = orderId1 == orderId2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void NotEqual_operator_works_correctly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId1 = new OrderId(id);
        var orderId2 = new OrderId(id);

        // Act
        var result = orderId1 != orderId2;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Parameterless_constructor_initializes_value_with_new_guid()
    {
        // Arrange
        var orderId = new OrderId();

        // Act
        var result = orderId.Value;

        // Assert
        result.Should().NotBeEmpty();
    }
}
