using System;

namespace DynamicDriving.BuildingBlocks.Tests.DomainDriven;

public readonly struct OrderId : IEquatable<OrderId>
{
    public OrderId()
    {
        this.Value = Guid.NewGuid();
    }

    public OrderId(Guid value)
    {
        this.Value = value;
    }

    public Guid Value { get; }

    public bool Equals(OrderId other)
    {
        return this.Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        return obj is OrderId other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }

    public static bool operator ==(OrderId left, OrderId right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(OrderId left, OrderId right)
    {
        return !Equals(left, right);
    }
}
