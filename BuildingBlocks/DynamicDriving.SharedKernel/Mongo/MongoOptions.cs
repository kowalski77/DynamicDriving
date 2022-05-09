namespace DynamicDriving.SharedKernel.Mongo;

public class MongoOptions
{
    public string Client { get; set; } = "mongodb://localhost:27017";

    public string Database { get; set; } = default!;
}
