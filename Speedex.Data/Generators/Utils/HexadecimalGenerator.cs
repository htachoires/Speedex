namespace Speedex.Data.Generators.Utils;

public static class HexadecimalGenerator
{
    private static readonly char[] HexChars = "0123456789ABCDEF".ToCharArray();
    private static readonly Random Random = new();

    public static string GenerateHexadecimal(int size)
    {
        var result = new char[size];

        for (var i = 0; i < size; i++)
        {
            result[i] = HexChars[Random.Next(0, HexChars.Length)];
        }

        return new string(result);
    }
}