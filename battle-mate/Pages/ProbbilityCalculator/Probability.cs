namespace battle_mate.Pages.BattleResult;

public class Probability
{
    public double StartResult { get; }
    public int Target { get; }
    public RerollDto Rerolls { get; }
    public double Additional { get; }
    public double AdditionalFails { get; }

    public Probability(double startResult, int target, RerollDto rerollDto = default, double additional = default, double additionalFails = default)
    {
        StartResult = startResult;
        Target = target;
        Rerolls = rerollDto;
        Additional = additional;
        AdditionalFails = additionalFails;
    }
    
    public Probability Append(int nextTarget, RerollDto rerolls = default)
    {
        return new Probability(SuccessResult, nextTarget, rerolls);
    }
    
    public Probability NegativeAppend(int nextTarget, RerollDto rerolls = default)
    {
        return new Probability(FailedResult, nextTarget, rerolls);
    }

    public double SuccessResult
    {
        get
        {
            var successResultBase = Additional + StartResult * ((7 - Target) / 6d);
            
            if (Rerolls != null)
            {
                if (Rerolls.Reroll1SChecked)
                {
                    var onesRolled = (Additional + StartResult) / 6;
                    successResultBase += onesRolled * ((7 - Target) / 6d);
                }
                if (Rerolls.RerollFailsChecked)
                {
                    var failed = StartResult - successResultBase + AdditionalFails;
                    var additionalSuccess = failed * ((7 - Target) / 6d);
                    successResultBase += additionalSuccess;
                }
                if (Rerolls.RerollSuccessChecked)
                {
                    successResultBase *= (7 - Target) / 6d;
                }
            }

            return successResultBase;
        }
    }

    public double FailedResult => StartResult - SuccessResult + AdditionalFails;
}