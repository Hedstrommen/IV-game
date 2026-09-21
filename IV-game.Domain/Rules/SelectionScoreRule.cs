using IV_game.Domain.ProcedureSteps;

namespace IV_game.Domain.Rules;

public static class SelectionScoreRule
{
    public const int PointsPerCorrectItem = 5;
    public const int PointsDeductionPerMissedItem = 5;
    public const int PointsPerCriticalFailure = -15;

    public static SelectionOutcome Evaluate(ProcedureStep step, IReadOnlyList<string> chosenActionIds)
    {
        ArgumentNullException.ThrowIfNull(step);
        ArgumentNullException.ThrowIfNull(chosenActionIds);

        List<SelectionItemResult> results = new();
        foreach (StepAction action in step.Actions)
        {
            bool wasChosen = chosenActionIds.Contains(action.Id);
            SelectionItemResult.ItemVerdict verdict;
            if (wasChosen && action.IsCorrect)
            {
                verdict = SelectionItemResult.ItemVerdict.CorrectPick;
            }
            else if (wasChosen && action.IsCritical)
            {
                verdict = SelectionItemResult.ItemVerdict.CriticalWrong;
            }
            else if (!wasChosen && action.IsCorrect)
            {
                verdict = SelectionItemResult.ItemVerdict.MissingRequired;
            }
            else if (wasChosen && !action.IsCorrect)
            {
                verdict = SelectionItemResult.ItemVerdict.WrongExtra;
            }
            else
            {
                verdict = SelectionItemResult.ItemVerdict.CorrectPick;
            }
            results.Add(new SelectionItemResult
            {
                Action = action,
                WasChosen = wasChosen,
                Verdict = verdict
            });
        }

        int requiredCount = step.Actions.Count(a => a.IsCorrect);
        int correctPicks = results.Count(r => r.Verdict == SelectionItemResult.ItemVerdict.CorrectPick && r.Action.IsCorrect);
        int missedItems = results.Count(r => r.Verdict == SelectionItemResult.ItemVerdict.MissingRequired);
        int wrongExtras = results.Count(r => r.Verdict == SelectionItemResult.ItemVerdict.WrongExtra);
        int criticalWrongs = results.Count(r => r.Verdict == SelectionItemResult.ItemVerdict.CriticalWrong);

        int points = correctPicks * PointsPerCorrectItem
            - missedItems * PointsDeductionPerMissedItem
            - wrongExtras * PointsDeductionPerMissedItem
            + criticalWrongs * PointsPerCriticalFailure;

        bool isCorrect = missedItems == 0 && wrongExtras == 0 && criticalWrongs == 0;

        return new SelectionOutcome
        {
            Step = step,
            ItemResults = results,
            IsCorrect = isCorrect,
            IsCriticalFailure = criticalWrongs > 0,
            PointsAwarded = points,
            SourceReference = step.SourceReference
        };
    }

    public static SelectionOutcome EvaluateOrder(ProcedureStep step, IReadOnlyList<string> chosenActionIds)
    {
        ArgumentNullException.ThrowIfNull(step);
        ArgumentNullException.ThrowIfNull(chosenActionIds);

        List<SelectionItemResult> results = new();
        List<StepAction> required = step.Actions
            .Where(a => a.IsCorrect)
            .OrderBy(a => a.Order)
            .ToList();

        int correctlyPlaced = 0;
        foreach (StepAction expected in required)
        {
            int position = required.IndexOf(expected);
            bool wasChosenAtPosition = position < chosenActionIds.Count
                && chosenActionIds[position] == expected.Id;
            if (wasChosenAtPosition)
            {
                correctlyPlaced++;
            }
            results.Add(new SelectionItemResult
            {
                Action = expected,
                WasChosen = chosenActionIds.Contains(expected.Id),
                Verdict = wasChosenAtPosition
                    ? SelectionItemResult.ItemVerdict.CorrectPick
                    : SelectionItemResult.ItemVerdict.WrongExtra
            });
        }

        foreach (StepAction decoy in step.Actions.Where(a => !a.IsCorrect))
        {
            if (!chosenActionIds.Contains(decoy.Id))
            {
                continue;
            }
            results.Add(new SelectionItemResult
            {
                Action = decoy,
                WasChosen = true,
                Verdict = decoy.IsCritical
                    ? SelectionItemResult.ItemVerdict.CriticalWrong
                    : SelectionItemResult.ItemVerdict.WrongExtra
            });
        }

        int wrongPositions = required.Count - correctlyPlaced;
        int criticalWrongs = results.Count(r => r.Verdict == SelectionItemResult.ItemVerdict.CriticalWrong);
        int points = correctlyPlaced * PointsPerCorrectItem
            - wrongPositions * PointsDeductionPerMissedItem
            + criticalWrongs * PointsPerCriticalFailure;

        return new SelectionOutcome
        {
            Step = step,
            ItemResults = results,
            IsCorrect = wrongPositions == 0 && chosenActionIds.Count == required.Count,
            IsCriticalFailure = criticalWrongs > 0,
            PointsAwarded = points,
            SourceReference = step.SourceReference
        };
    }
}
