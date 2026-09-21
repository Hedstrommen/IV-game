using IV_game.Domain.ProcedureSteps;

namespace IV_game.Domain.GameSession;

public sealed record StepReview(
    ProcedureStep Step,
    string ChosenSummary,
    bool WasCorrect,
    bool WasCriticalFailure,
    string Feedback,
    string SourceReference);
