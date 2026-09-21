namespace IV_game.Domain.PatientCases;

public sealed class PatientCase
{
    public string Id { get; init; } = string.Empty;

    public string PatientName { get; init; } = string.Empty;

    public int Age { get; init; }

    public string Presentation { get; init; } = string.Empty;

    public IReadOnlyList<PatientRiskFlag> RiskFlags { get; init; } = Array.Empty<PatientRiskFlag>();

    public IReadOnlyList<VenCandidate> Vens { get; init; } = Array.Empty<VenCandidate>();
}
