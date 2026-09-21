using IV_game.Domain.GameSession;
using IV_game.Domain.ProcedureSteps;

namespace IV_game.Application.UseCases;

public sealed class SubmitSelectionUseCase
{
    public SelectionOutcome Execute(GameSession session, IReadOnlyList<string> chosenActionIds)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(chosenActionIds);
        return session.SubmitSelection(chosenActionIds);
    }
}
