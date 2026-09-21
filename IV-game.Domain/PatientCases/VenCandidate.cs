namespace IV_game.Domain.PatientCases;

public sealed class VenCandidate
{
    public VenSite Site { get; init; }

    public ArmSide Side { get; init; }

    public string Description { get; init; } = string.Empty;

    public bool IsPalpable { get; init; }

    public bool IsStraight { get; init; }

    public bool IsOverJoint { get; init; }

    public bool IsHardOrIrritated { get; init; }

    public bool SkinInflamedOrDamaged { get; init; }

    public bool IsExcludedArm { get; init; }

    public bool IsAvailable { get; init; } = true;
}
