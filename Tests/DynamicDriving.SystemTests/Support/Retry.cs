namespace DynamicDriving.SystemTests.Support;

public class Retry
{
    public Retry(
        int milliseconds,
        int retries)
    {
        this.Milliseconds = milliseconds;
        this.Retries = retries;
    }

    public int Milliseconds { get; }

    public int Retries { get; }

    public static RetryOfT<T> Handle<T>(int milliseconds, int tries)
        where T : Exception
    {
        return new RetryOfT<T>(milliseconds, tries);
    }
}

public class RetryOfT<T> : Retry
    where T : Exception
{
    public RetryOfT(int milliseconds, int retries)
        : base(milliseconds, retries)
    {
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        var retries = 1;

        while (true)
        {
            try
            {
                await action();
                return;
            }
            catch (T e)
            {
                if (retries == this.Retries)
                {
                    throw new InvalidOperationException(e.Message, e);
                }

                retries++;
                await Task.Delay(this.Milliseconds);
            }
        }
    }
}

