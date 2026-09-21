using IV_game.Application.Abstractions;
using IV_game.Domain.GameSession;
using IV_game.Domain.PatientCases;
using IV_game.Domain.ProcedureSteps;

namespace IV_game.Application.UseCases;

public sealed class StartGameUseCase
{
    private readonly IPatientCaseRepository _patientCaseRepository;
    private readonly IProcedureStepRepository _procedureStepRepository;

    public StartGameUseCase(
        IPatientCaseRepository patientCaseRepository,
        IProcedureStepRepository procedureStepRepository)
    {
        _patientCaseRepository = patientCaseRepository;
        _procedureStepRepository = procedureStepRepository;
    }

    public IReadOnlyList<PatientCase> GetAvailableCases()
    {
        return _patientCaseRepository.GetAll();
    }

    public GameSession Start(string patientCaseId)
    {
        PatientCase patient = _patientCaseRepository.GetById(patientCaseId);
        IReadOnlyList<ProcedureStep> steps = _procedureStepRepository.GetAll()
            .OrderBy(s => s.OrderIndex)
            .ToList();

        return new GameSession(patient, steps);
    }
}
