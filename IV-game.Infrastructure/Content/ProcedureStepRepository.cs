using System.Text.Json;
using IV_game.Application.Abstractions;
using IV_game.Domain.ProcedureSteps;

namespace IV_game.Infrastructure.Content;

public sealed class ProcedureStepRepository : IProcedureStepRepository
{
    private readonly IReadOnlyList<ProcedureStep> _steps;

    public ProcedureStepRepository()
        : this(Path.Combine(AppContext.BaseDirectory, "Content", "procedure-steps.json"))
    {
    }

    public ProcedureStepRepository(string contentPath)
    {
        JsonSerializerOptions options = PatientCaseRepository.CreateOptions();

        using FileStream stream = File.OpenRead(contentPath);
        List<ProcedureStep>? steps = JsonSerializer.Deserialize<List<ProcedureStep>>(stream, options);

        _steps = steps?
            .OrderBy(s => s.OrderIndex)
            .ToList() ?? new List<ProcedureStep>();
    }

    public IReadOnlyList<ProcedureStep> GetAll() => _steps;

    public IReadOnlyList<ProcedureStep> GetByPhase(ProcedurePhase phase)
    {
        return _steps.Where(s => s.Phase == phase).ToList();
    }
}
