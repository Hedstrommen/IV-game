namespace IV_game.Domain.ProcedureSteps;

public sealed class ProcedureStep
{
    public string Id { get; init; } = string.Empty;

    public ProcedurePhase Phase { get; init; }

    public int OrderIndex { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Prompt { get; init; } = string.Empty;

    public StepKind Kind { get; init; } = StepKind.MultipleChoice;

    public string SourceReference { get; init; } = string.Empty;

    public IReadOnlyList<StepAction> Actions { get; init; } = Array.Empty<StepAction>();
}
