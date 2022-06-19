using System.Text.Json.Serialization;

namespace battle_mate.Pages.BattleResult;

public class ProbabilityChain
{
    public bool PoisonChecked { get; set; }
    public bool Poison5Checked { get; set; }
    public bool LethalStrikeChecked { get; set; }
    public bool BattleFocusChecked { get; set; }
    
    public RerollDto RerollsHit  { get; set; }
    public RerollDto RerollsWound { get; set; }
    public RerollDto RerollsArmorSaves  { get; set; }
    public RerollDto RerollsWardSaves  { get; set; }
    
    public int StartDice { get; set; }
    public int ToHit { get; set; }
    public int ToWound { get; set; }
    public int ToArmorSave { get; set; }
    public int ToWardSave { get; set; }
    [JsonIgnore]
    public Probability ExpectedHits
    {
        get
        {
            if (BattleFocusChecked)
            {
                var battleFocusHits = StartDice / 6d;
                return new Probability(StartDice, ToHit, battleFocusHits);
            }
            
            return new(StartDice, ToHit);
        }
    }

    [JsonIgnore]
    public Probability ExpectedWounds
    {
        get
        {
            if (Poison5Checked)
            {
                var poison5Hits = ExpectedHits.StartResult / 3d;
                var hitsWithOutPoison5 = ExpectedHits.SuccessResult - poison5Hits;
                return new Probability(hitsWithOutPoison5, ToWound, poison5Hits);
            }
            
            if (PoisonChecked)
            {
                var poisonHits = ExpectedHits.StartResult / 6d;
                var hitsWithOutPoison = ExpectedHits.SuccessResult - poisonHits;
                return new Probability(hitsWithOutPoison, ToWound, poisonHits);
            }
            
            return ExpectedHits.Append(ToWound);
        }
    }

    [JsonIgnore]
    public Probability ExpectedArmorSaves
    {
        get
        {
            if (LethalStrikeChecked)
            {
                var lethalHits = ExpectedWounds.SuccessResult / 6d;
                var hitsWithOutLethal = ExpectedWounds.SuccessResult - lethalHits;
                return new Probability(hitsWithOutLethal, ToArmorSave, 0, lethalHits);
            }
            
            return ExpectedWounds.Append(ToArmorSave);
        }
    }

    [JsonIgnore]
    public Probability ExpectedWardSaves => ExpectedArmorSaves.NegativeAppend(ToWardSave);
    [JsonIgnore]
    public double ExpectedFailedWardSaves => ExpectedWardSaves.FailedResult;
    [JsonIgnore]
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

public class RerollDto
{
    public bool RerollSuccessChecked { get; set; }
    public bool RerollFailsChecked { get; set; }
    public bool Reroll1SChecked { get; set; }
}