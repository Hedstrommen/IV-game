using IV_game.Application.UseCases;
using IV_game.Infrastructure.Content;
using IV_game.Infrastructure.Persistence;

namespace IV_game.Presentation;

public sealed class AppServices
{
    public static AppServices Instance { get; } = new();

    public StartGameUseCase StartGame { get; }

    public SubmitAnswerUseCase SubmitAnswer { get; }

    public FinishGameUseCase FinishGame { get; }

    private AppServices()
    {
        PatientCaseRepository patientCaseRepository = new();
        ProcedureStepRepository procedureStepRepository = new();
        JsonScoreRepository scoreRepository = new();

        StartGame = new StartGameUseCase(patientCaseRepository, procedureStepRepository);
        SubmitAnswer = new SubmitAnswerUseCase();
        FinishGame = new FinishGameUseCase(scoreRepository);
    }
}
