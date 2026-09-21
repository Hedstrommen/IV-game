namespace IV_game.Application.Abstractions;

public sealed class ScoreRecord
{
    public string PatientCaseId { get; init; } = string.Empty;

    public string PlayerName { get; init; } = string.Empty;

    public int Score { get; init; }

    public int CorrectCount { get; init; }

    public int TotalSteps { get; init; }

    public DateTime CompletedAt { get; init; } = DateTime.UtcNow;
}

public interface IScoreRepository
{
    void Save(ScoreRecord record);

    IReadOnlyList<ScoreRecord> GetTop(int count);
}
