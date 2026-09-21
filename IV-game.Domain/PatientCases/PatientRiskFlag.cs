namespace IV_game.Domain.PatientCases;

public enum PatientRiskFlag
{
    DialysfistelVansterArm,
    DialysfistelHogerArm,
    LymfadenektomiVansterArm,
    LymfadenektomiHogerArm,
    TidigareTrombosVansterArm,
    TidigareTrombosHogerArm,
    Stickradda,
    NedsattKansel,
    Dehydrering
}

public static class PatientRiskFlagText
{
    public static string Describe(PatientRiskFlag flag) => flag switch
    {
        PatientRiskFlag.DialysfistelVansterArm => "Dialysfistel på vänster arm",
        PatientRiskFlag.DialysfistelHogerArm => "Dialysfistel på höger arm",
        PatientRiskFlag.LymfadenektomiVansterArm => "Axillär lymfkörtelutrymning på vänster sida",
        PatientRiskFlag.LymfadenektomiHogerArm => "Axillär lymfkörtelutrymning på höger sida",
        PatientRiskFlag.TidigareTrombosVansterArm => "Tidigare trombos i vänster arm",
        PatientRiskFlag.TidigareTrombosHogerArm => "Tidigare trombos i höger arm",
        PatientRiskFlag.Stickradda => "Patienten har stickrädsla",
        PatientRiskFlag.NedsattKansel => "Nedsatt känsel i extremiteten",
        PatientRiskFlag.Dehydrering => "Dehydrering",
        _ => flag.ToString()
    };
}
