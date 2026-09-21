using IV_game.Domain.PatientCases;

namespace IV_game.Domain.Rules;

public enum TromboflebitGrade
{
    Ingen,
    Misstankt,
    Konstaterad,
    Allvarlig
}

public static class TromboflebitGradeText
{
    public static string Name(TromboflebitGrade grade) => grade switch
    {
        TromboflebitGrade.Ingen => "Ingen tromboflebit",
        TromboflebitGrade.Misstankt => "Misstänkt tromboflebit",
        TromboflebitGrade.Konstaterad => "Konstaterad tromboflebit",
        TromboflebitGrade.Allvarlig => "Allvarlig tromboflebit",
        _ => grade.ToString()
    };
}

public sealed class PhlebitSymptom
{
    public bool Rodnad { get; init; }

    public bool Omdhet { get; init; }

    public bool Svullnad { get; init; }

    public bool Smarta { get; init; }

    public bool PalpabelHardhet { get; init; }

    public bool Feber { get; init; }
}

public static class TromboflebitRule
{
    public static TromboflebitGrade Grade(PhlebitSymptom symptom)
    {
        int localSigns = CountLocalSigns(symptom);

        if (symptom.Feber && localSigns >= 2)
        {
            return TromboflebitGrade.Allvarlig;
        }

        if (localSigns >= 4)
        {
            return TromboflebitGrade.Allvarlig;
        }

        if (localSigns >= 3)
        {
            return TromboflebitGrade.Konstaterad;
        }

        if (localSigns >= 1)
        {
            return TromboflebitGrade.Misstankt;
        }

        return TromboflebitGrade.Ingen;
    }

    public static string Action(PhlebitSymptom symptom)
    {
        TromboflebitGrade grade = Grade(symptom);

        return grade switch
        {
            TromboflebitGrade.Ingen => "Fortsatt observation enligt rutin.",
            TromboflebitGrade.Misstankt => "Ökad observation, inspektera insticksstället och utväridera behovet av PVK.",
            TromboflebitGrade.Konstaterad => "Avlägsna PVK, kontakta ansvarig läkare och dokumentera.",
            TromboflebitGrade.Allvarlig => "Avlägsna PVK omedelbart, kontakta ansvarig läkare och dokumentera. Bedöm behov av odling.",
            _ => string.Empty
        };
    }

    private static int CountLocalSigns(PhlebitSymptom symptom)
    {
        int count = 0;
        if (symptom.Rodnad) count++;
        if (symptom.Omdhet) count++;
        if (symptom.Svullnad) count++;
        if (symptom.Smarta) count++;
        if (symptom.PalpabelHardhet) count++;
        return count;
    }
}
