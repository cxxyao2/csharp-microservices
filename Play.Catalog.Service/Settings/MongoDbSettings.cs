namespace Play.Catalog.Service.Settings
{
  public class MongoDbSettings
  {
    // init prevents any piece of code from modifying the value after they have been initialized
    public string Host { get; init; }

    public int Port { get; init; }

    public string ConnectionString => $"mongodb://{Host}:{Port}";
  }
}