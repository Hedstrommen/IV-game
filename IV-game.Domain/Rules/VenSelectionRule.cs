using IV_game.Domain.PatientCases;

namespace IV_game.Domain.Rules;

public static class VenSelectionRule
{
    public static bool IsSiteForbidden(VenCandidate ven)
    {
        return ven.IsExcludedArm
            || ven.SkinInflamedOrDamaged
            || ven.IsHardOrIrritated
            || !ven.IsAvailable;
    }

    public static bool IsRecommended(VenCandidate ven)
    {
        return ven.IsPalpable && ven.IsStraight && !IsSiteForbidden(ven);
    }

    public static VenCandidate? FindBest(IEnumerable<VenCandidate> vens)
    {
        return vens
            .Where(IsRecommended)
            .OrderBy(v => VenSiteText.Priority(v.Site))
            .ThenByDescending(v => v.IsPalpable)
            .FirstOrDefault();
    }
}
