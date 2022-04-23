using System.Collections.Generic;

namespace battle_mate.Pages;

public class DiceMessage
{
    public DiceMessage(RollResult result)
    {
        Results.Add(result);
    }

    public List<RollResult> Results { get; } = new();

}