namespace DynamicDriving.SharedKernel.DomainDriven;

public abstract class BaseId : IEquatable<BaseId>
{
    protected BaseId(Guid value)
    {
        this.Value = value;
    }

    public Guid Value { get; }

    public bool Equals(BaseId? other)
    {
        return this.GetType() == other?.GetType() && this.Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        return obj is BaseId other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }

    public override string ToString()
    {
        return this.Value.ToString();
    }

    public static bool operator ==(BaseId? a, BaseId? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(BaseId a, BaseId b)
    {
        return !(a == b);
    }
}
