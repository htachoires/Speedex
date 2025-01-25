namespace Speedex.Data;

public record GenerateOptions
{
    public const string SectionName = "GenerateOptions";

    public int NbProductElements { get; init; }
    public int NbOrderElements { get; init; }
    public int NbParcelElements { get; init; }
    public int NbReturnElements { get; init; }
}