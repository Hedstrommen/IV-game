namespace IV_game.Domain.PatientCases;

public enum VenSite
{
    Underarm,
    Handrygg,
    Armveck,
    Overarm,
    NedreExtremitet,
    Ovriga
}

public static class VenSiteText
{
    public static string Name(VenSite site) => site switch
    {
        VenSite.Underarm => "Underarm",
        VenSite.Handrygg => "Handrygg",
        VenSite.Armveck => "Armveck",
        VenSite.Overarm => "Överarm",
        VenSite.NedreExtremitet => "Nedre extremitet",
        VenSite.Ovriga => "Övriga",
        _ => site.ToString()
    };

    public static int Priority(VenSite site) => site switch
    {
        VenSite.Underarm => 1,
        VenSite.Handrygg => 2,
        VenSite.Armveck => 3,
        VenSite.Overarm => 4,
        VenSite.NedreExtremitet => 5,
        VenSite.Ovriga => 6,
        _ => int.MaxValue
    };
}
