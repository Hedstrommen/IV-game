using System.Text.Json;
using System.Text.Json.Serialization;
using IV_game.Application.Abstractions;
using IV_game.Domain.PatientCases;

namespace IV_game.Infrastructure.Content;

public sealed class PatientCaseRepository : IPatientCaseRepository
{
    private readonly IReadOnlyList<PatientCase> _cases;

    public PatientCaseRepository()
        : this(Path.Combine(AppContext.BaseDirectory, "Content", "patient-cases.json"))
    {
    }

    public PatientCaseRepository(string contentPath)
    {
        JsonSerializerOptions options = CreateOptions();

        using FileStream stream = File.OpenRead(contentPath);
        List<PatientCase>? cases = JsonSerializer.Deserialize<List<PatientCase>>(stream, options);

        _cases = cases ?? new List<PatientCase>();
    }

    public IReadOnlyList<PatientCase> GetAll() => _cases;

    public PatientCase GetById(string id)
    {
        PatientCase? match = _cases.FirstOrDefault(c => c.Id == id);

        return match ?? throw new KeyNotFoundException($"Ingen patient med id '{id}' hittades.");
    }

    internal static JsonSerializerOptions CreateOptions() => new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        Converters = { new JsonStringEnumConverter() }
    };
}
