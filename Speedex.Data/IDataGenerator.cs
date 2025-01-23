namespace Speedex.Data;

public interface IDataGenerator
{
    void GenerateData();
}

public interface IDataGenerator<TKey, TValue> where TKey : notnull
{
    Dictionary<TKey, TValue> Data { get; }
    void GenerateData(int nbElements);
}