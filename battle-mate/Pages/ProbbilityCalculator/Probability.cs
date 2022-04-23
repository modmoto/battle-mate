namespace battle_mate.Pages.BattleResult;

public class Probability
{
    public double StartResult { get; }
    public int Target { get; }

    public Probability(double startResult, int target)
    {
        StartResult = startResult;
        Target = target;
    }
    
    public Probability Append(int nextTarget)
    {
        return new Probability(SuccessResult, nextTarget);
    }
    
    public Probability NegativeAppend(int nextTarget)
    {
        return new Probability(FailedResult, nextTarget);
    }

    public double SuccessResult => StartResult * ((7 - Target) / 6d);

    public double FailedResult => StartResult - SuccessResult;

    
}