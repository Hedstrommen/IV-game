namespace IV_game.Domain.ProcedureSteps;

public enum ProcedurePhase
{
    Forberedelser,
    ValAvVen,
    Inlaggning,
    FixeringOchDokumentation,
    Avlagsnande,
    Komplikationer
}

public static class ProcedurePhaseText
{
    public static string Name(ProcedurePhase phase) => phase switch
    {
        ProcedurePhase.Forberedelser => "Förberedelser",
        ProcedurePhase.ValAvVen => "Val av ven och punktionsställe",
        ProcedurePhase.Inlaggning => "Inläggning",
        ProcedurePhase.FixeringOchDokumentation => "Fixering och dokumentation",
        ProcedurePhase.Avlagsnande => "Avlägsnande",
        ProcedurePhase.Komplikationer => "Komplikationer",
        _ => phase.ToString()
    };
}
