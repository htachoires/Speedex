namespace Speedex.Data;

public interface IDataGenerator
{
    public void GenerateData();
}

public interface IDataGenerator<TKey, TValue> where TKey : notnull
{
    public void GenerateData(int nbElements);

    public Dictionary<TKey, TValue> Data { get; }
}