using System.Text.Json;
using IV_game.Application.Abstractions;

namespace IV_game.Infrastructure.Persistence;

public sealed class JsonScoreRepository : IScoreRepository
{
    private readonly string _filePath;
    private readonly object _lock = new();
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    public JsonScoreRepository()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "IV-game",
            "scores.json"))
    {
    }

    public JsonScoreRepository(string filePath)
    {
        _filePath = filePath;

        string? directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public void Save(ScoreRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);

        lock (_lock)
        {
            List<ScoreRecord> records = ReadAll().ToList();
            records.Add(record);
            File.WriteAllText(_filePath, JsonSerializer.Serialize(records, SerializerOptions));
        }
    }

    public IReadOnlyList<ScoreRecord> GetTop(int count)
    {
        lock (_lock)
        {
            return ReadAll()
                .OrderByDescending(r => r.Score)
                .ThenByDescending(r => r.CompletedAt)
                .Take(count)
                .ToList();
        }
    }

    private IEnumerable<ScoreRecord> ReadAll()
    {
        if (!File.Exists(_filePath))
        {
            return Array.Empty<ScoreRecord>();
        }

        try
        {
            using FileStream stream = File.OpenRead(_filePath);
            List<ScoreRecord>? records = JsonSerializer.Deserialize<List<ScoreRecord>>(stream, SerializerOptions);
            return records ?? new List<ScoreRecord>();
        }
        catch (IOException)
        {
            return Array.Empty<ScoreRecord>();
        }
        catch (JsonException)
        {
            return Array.Empty<ScoreRecord>();
        }
    }
}
