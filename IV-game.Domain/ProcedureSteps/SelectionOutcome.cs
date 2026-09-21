using System.Text;

namespace IV_game.Domain.ProcedureSteps;

public sealed class SelectionItemResult
{
    public required StepAction Action { get; init; }
    public bool WasChosen { get; init; }
    public ItemVerdict Verdict { get; init; }

    public enum ItemVerdict
    {
        CorrectPick,
        MissingRequired,
        WrongExtra,
        CriticalWrong
    }
}

public sealed class SelectionOutcome
{
    public required ProcedureStep Step { get; init; }
    public required IReadOnlyList<SelectionItemResult> ItemResults { get; init; }
    public bool IsCorrect { get; init; }
    public bool IsCriticalFailure { get; init; }
    public int PointsAwarded { get; init; }
    public string SourceReference { get; init; } = string.Empty;

    public string BuildFeedback()
    {
        StringBuilder builder = new();
        foreach (SelectionItemResult result in ItemResults)
        {
            if (result.Verdict == SelectionItemResult.ItemVerdict.CorrectPick)
            {
                continue;
            }
            builder.AppendLine(result.Action.Feedback);
        }
        if (builder.Length == 0)
        {
            builder.AppendLine("Perfekt! Du valde exakt rätt.");
        }
        return builder.ToString().TrimEnd();
    }
}
