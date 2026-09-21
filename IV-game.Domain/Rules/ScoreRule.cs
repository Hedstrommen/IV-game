using IV_game.Domain.ProcedureSteps;

namespace IV_game.Domain.Rules;

public static class ScoreRule
{
    public const int PointsPerCorrectStep = 10;

    public const int PointsPerCriticalFailure = -15;

    public const int MaxScoreReductionPerStep = 10;

    public static ProcedureOutcome Evaluate(ProcedureStep step, StepAction action)
    {
        if (action.IsCorrect)
        {
            return new ProcedureOutcome
            {
                IsCorrect = true,
                Feedback = action.Feedback,
                PointsAwarded = PointsPerCorrectStep,
                SourceReference = step.SourceReference
            };
        }

        return new ProcedureOutcome
        {
            IsCorrect = false,
            IsCriticalFailure = action.IsCritical,
            Feedback = action.Feedback,
            PointsAwarded = action.IsCritical ? PointsPerCriticalFailure : -PointsPerCorrectStep,
            SourceReference = step.SourceReference
        };
    }
}
