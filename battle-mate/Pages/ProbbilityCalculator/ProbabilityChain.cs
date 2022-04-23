namespace battle_mate.Pages.BattleResult;

public class ProbabilityChain
{
    public int StartDice { get; set; }
    public int ToHit { get; set; }
    public int ToWound { get; set; }
    public int ToArmorSave { get; set; }
    public int ToWardSave { get; set; }
    public Probability ExpectedHits => new(StartDice, ToHit);
    public Probability ExpectedWounds => ExpectedHits.Append(ToWound);
    public Probability ExpectedArmorSaves => ExpectedWounds.Append(ToArmorSave);
    public Probability ExpectedWardSaves => ExpectedArmorSaves.NegativeAppend(ToWardSave);
    public double ExpectedFailedWardSaves => ExpectedWardSaves.FailedResult;

    public double CurrentWounds
    {
        get
        {
            if (ToWardSave != default)
            {
                return ExpectedFailedWardSaves;
            }
            if (ToArmorSave != default)
            {
                return ExpectedArmorSaves.FailedResult;
            }
            if (ToWound != default)
            {
                return ExpectedWounds.SuccessResult;
            }
            if (ToHit != default)
            {
                return ExpectedHits.SuccessResult;
            }

            return 0;
        }
    }
}