namespace IV_game.Domain.ProcedureSteps;

public sealed class StepAction
{
    public string Id { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public bool IsCorrect { get; init; }

    public bool IsCritical { get; init; }

    public string Feedback { get; init; } = string.Empty;
}
