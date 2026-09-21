using IV_game.Domain.PatientCases;

namespace IV_game.Application.Abstractions;

public interface IPatientCaseRepository
{
    IReadOnlyList<PatientCase> GetAll();

    PatientCase GetById(string id);
}
