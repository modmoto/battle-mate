namespace battle_mate;

public enum RollState
{
    None,
    ToHit,
    ToWound,
    ArmorSave,
    WardSave,
    AutomaticHit,
    AutomaticWound,
    NoArmorSave,
    NoWardSave
}

public enum RerollState
{
    NoReroll,
    Reroll1s,
    RerollBiggerThan,
    RerollSmallerThan
}