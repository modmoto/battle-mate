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

    public double RerollCount
    {
        get
        {
            if (Rerolls != null)
            {
                if (Rerolls.Reroll1SChecked)
                {
                    var onesRolled = (Additional + StartResult) / 6;
                    return onesRolled * ((7 - Target) / 6d);
                }

                if (Rerolls.RerollFailsChecked)
                {
                    var failed = StartResult - SuccessResultBase + AdditionalFails;
                    var additionalSuccess = failed * ((7 - Target) / 6d);
                    return additionalSuccess;
                }

                if (Rerolls.RerollSuccessChecked)
                {
                    var pureSuccess = StartResult * ((7 - Target) / 6d);
                    return -(pureSuccess * ((7 - Target) / 6d));
                }
            }

            return 0;
        }
    }

    private double SuccessResultBase => Additional + StartResult * ((7 - Target) / 6d);
    public double SuccessResult => SuccessResultBase + RerollCount;
    public double FailedResult => StartResult - SuccessResult + AdditionalFails;
}