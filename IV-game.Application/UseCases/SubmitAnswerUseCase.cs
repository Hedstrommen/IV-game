using IV_game.Domain.GameSession;
using IV_game.Domain.ProcedureSteps;

namespace IV_game.Application.UseCases;

public sealed class SubmitAnswerUseCase
{
    public ProcedureOutcome Execute(GameSession session, string actionId)
    {
        ArgumentNullException.ThrowIfNull(session);

        return session.SubmitAnswer(actionId);
    }
}
