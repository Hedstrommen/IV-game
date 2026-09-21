using IV_game.Domain.PatientCases;
using IV_game.Domain.ProcedureSteps;
using IV_game.Domain.Rules;

namespace IV_game.Domain.GameSession;

public class GameSession
{
    private readonly IReadOnlyList<ProcedureStep> _steps;
    private int _currentIndex;
    private int _score;

    public PatientCase Patient { get; }

    public GameSession(PatientCase patient, IReadOnlyList<ProcedureStep> steps)
    {
        Patient = patient ?? throw new ArgumentNullException(nameof(patient));
        _steps = steps ?? throw new ArgumentNullException(nameof(steps));
        _currentIndex = 0;
        _score = 0;
    }

    public int Score => _score;

    public ProcedureStep? CurrentStep => _currentIndex < _steps.Count ? _steps[_currentIndex] : null;

    public int StepNumber => _currentIndex + 1;

    public int TotalSteps => _steps.Count;

    public bool IsFinished => _currentIndex >= _steps.Count;

    public int CorrectCount { get; private set; }

    public int CriticalFailures { get; private set; }

    public IReadOnlyList<StepReview> Reviews => _reviews;

    private readonly List<StepReview> _reviews = new();

    public ProcedureOutcome SubmitAnswer(string actionId)
    {
        ProcedureStep? step = CurrentStep;
        if (step is null)
        {
            throw new InvalidOperationException("Spelet är slutfört, det finns inget aktivt steg.");
        }

        StepAction action = step.Actions.FirstOrDefault(a => a.Id == actionId)
            ?? throw new ArgumentException($"Okänd svarsalternativ: {actionId}", nameof(actionId));

        ProcedureOutcome outcome = ScoreRule.Evaluate(step, action);

        _score += outcome.PointsAwarded;

        if (outcome.IsCorrect)
        {
            CorrectCount++;
        }

        if (outcome.IsCriticalFailure)
        {
            CriticalFailures++;
        }

        _reviews.Add(new StepReview(
            step,
            action.Label,
            outcome.IsCorrect,
            outcome.IsCriticalFailure,
            outcome.Feedback,
            outcome.SourceReference));

        _currentIndex++;

        return outcome;
    }

    public SelectionOutcome SubmitSelection(IReadOnlyList<string> chosenActionIds)
    {
        ProcedureStep? step = CurrentStep;
        if (step is null)
        {
            throw new InvalidOperationException("Spelet är slutfört, det finns inget aktivt steg.");
        }

        SelectionOutcome outcome = step.Kind switch
        {
            StepKind.OrderSelection => SelectionScoreRule.EvaluateOrder(step, chosenActionIds),
            _ => SelectionScoreRule.Evaluate(step, chosenActionIds)
        };

        _score += outcome.PointsAwarded;

        if (outcome.IsCorrect)
        {
            CorrectCount++;
        }

        if (outcome.IsCriticalFailure)
        {
            CriticalFailures++;
        }

        string chosenSummary = string.Join(", ", step.Actions
            .Where(a => chosenActionIds.Contains(a.Id))
            .Select(a => a.Label));

        _reviews.Add(new StepReview(
            step,
            chosenSummary,
            outcome.IsCorrect,
            outcome.IsCriticalFailure,
            outcome.BuildFeedback(),
            outcome.SourceReference));

        _currentIndex++;

        return outcome;
    }
}
