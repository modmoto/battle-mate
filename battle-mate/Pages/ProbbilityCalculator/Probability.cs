namespace battle_mate.Pages.BattleResult;

public class Probability
{
    public double StartResult { get; }
    public int Target { get; }
    public double Additional { get; }
    public double AdditionalFails { get; }

    public Probability(double startResult, int target, double additional = default, double additionalFails = default)
    {
        StartResult = startResult;
        Target = target;
        Additional = additional;
        AdditionalFails = additionalFails;
    }
    
    public Probability Append(int nextTarget)
    {
        return new Probability(SuccessResult, nextTarget);
    }
    
    public Probability NegativeAppend(int nextTarget)
    {
        return new Probability(FailedResult, nextTarget);
    }

    public double SuccessResult => Additional + StartResult * ((7 - Target) / 6d);

    public double FailedResult => StartResult - SuccessResult + AdditionalFails;

    
}