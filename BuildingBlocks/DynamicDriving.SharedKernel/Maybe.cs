namespace DynamicDriving.SharedKernel;

public readonly struct Maybe<T> : IEquatable<Maybe<T>>
    where T : class
{
    private readonly T value;
    private readonly bool hasValue;

    private Maybe(T value)
    {
        this.value = value;
        this.hasValue = true;
    }

    public static implicit operator Maybe<T>(T value)
    {
        return value == null! ? new Maybe<T>() : new Maybe<T>(value);
    }

    public bool TryGetValue(out T tryValue)
    {
        if (this.hasValue)
        {
            tryValue = this.value;
            return true;
        }

        tryValue = default!;

        return false;
    }

    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
    {
        if (some == null)
        {
            throw new ArgumentNullException(nameof(some));
        }

        if (none == null)
        {
            throw new ArgumentNullException(nameof(none));
        }

        return this.hasValue ?
            some(this.value) :
            none();
    }

    public T ValueOr(T defaultValue)
    {
        return this.hasValue ? this.value : defaultValue;
    }

    public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> convert)
        where TResult : class
    {
        if (convert == null)
        {
            throw new ArgumentNullException(nameof(convert));
        }

        return !this.hasValue ? new Maybe<TResult>() : convert(this.value);
    }

    public Maybe<T> ToMaybe()
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        return EqualityComparer<T>.Default.GetHashCode(this.value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Maybe<T> other && this.Equals(other);
    }

    public static bool operator ==(Maybe<T> left, Maybe<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Maybe<T> left, Maybe<T> right)
    {
        return !(left == right);
    }

    public bool Equals(Maybe<T> other)
    {
        return EqualityComparer<T>.Default.Equals(this.value, other.value);
    }
}
