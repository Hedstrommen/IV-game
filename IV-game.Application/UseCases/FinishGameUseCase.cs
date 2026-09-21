using IV_game.Application.Abstractions;
using IV_game.Domain.GameSession;

namespace IV_game.Application.UseCases;

public sealed class FinishGameUseCase
{
    private readonly IScoreRepository _scoreRepository;

    public FinishGameUseCase(IScoreRepository scoreRepository)
    {
        _scoreRepository = scoreRepository;
    }

    public GameResult BuildResult(GameSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        int percent = session.TotalSteps > 0
            ? (int)Math.Round(100.0 * session.CorrectCount / session.TotalSteps)
            : 0;

        string grade = percent switch
        {
            >= 90 => "A – Utmärkt! Du arbetar patientsäkert enligt Vårdhandboken.",
            >= 75 => "B – Bra! Små förbättringar återstår.",
            >= 60 => "C – Godkänt men repetera vissa moment.",
            _ => "D – Repetera hela proceduren i Vårdhandboken."
        };

        return new GameResult
        {
            Score = session.Score,
            CorrectCount = session.CorrectCount,
            TotalSteps = session.TotalSteps,
            CriticalFailures = session.CriticalFailures,
            PercentCorrect = percent,
            Grade = grade,
            PatientName = session.Patient.PatientName
        };
    }

    public void SaveScore(GameSession session, string playerName)
    {
        ArgumentNullException.ThrowIfNull(session);

        _scoreRepository.Save(new ScoreRecord
        {
            PatientCaseId = session.Patient.Id,
            PlayerName = string.IsNullOrWhiteSpace(playerName) ? "Anonym" : playerName.Trim(),
            Score = session.Score,
            CorrectCount = session.CorrectCount,
            TotalSteps = session.TotalSteps
        });
    }

    public IReadOnlyList<ScoreRecord> GetHighScores(int count)
    {
        return _scoreRepository.GetTop(count);
    }

    public IReadOnlyList<StepReview> GetStepReviews(GameSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        return session.Reviews;
    }
}

public sealed class GameResult
{
    public int Score { get; init; }

    public int CorrectCount { get; init; }

    public int TotalSteps { get; init; }

    public int CriticalFailures { get; init; }

    public int PercentCorrect { get; init; }

    public string Grade { get; init; } = string.Empty;

    public string PatientName { get; init; } = string.Empty;
}
