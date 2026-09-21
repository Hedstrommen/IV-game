using IV_game.Domain.ProcedureSteps;

namespace IV_game.Application.Abstractions;

public interface IProcedureStepRepository
{
    IReadOnlyList<ProcedureStep> GetAll();

    IReadOnlyList<ProcedureStep> GetByPhase(ProcedurePhase phase);
}
