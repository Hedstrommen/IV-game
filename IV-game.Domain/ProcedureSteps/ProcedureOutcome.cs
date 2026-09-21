namespace IV_game.Domain.ProcedureSteps;

public sealed class ProcedureOutcome
{
    public bool IsCorrect { get; init; }

    public bool IsCriticalFailure { get; init; }

    public string Feedback { get; init; } = string.Empty;

    public int PointsAwarded { get; init; }

    public string SourceReference { get; init; } = string.Empty;
}
