using System.Collections.Generic;
using battle_mate.Pages.BattleResult;

namespace battle_mate.Pages.ProbbilityCalculator;

public class ProbabilityState
{
    public List<ProbabilityChain> ProbabilityChains { get; set; } = new();
    public BuildState BuildState { get; set; }
    public ProbabilityChain BuildingChain { get; set; }
    public int? DiceAmount { get; set; }

    public void ResetBuilding()
    {
        DiceAmount = null;
        BuildingChain = null;
        BuildState = BuildState.IsNothing;
    }

    public void AddBuildStep(int target)
    {
        if (DiceAmount == null) return;
        
        switch (BuildState)
        {
            case BuildState.IsNothing:
                BuildingChain = new ProbabilityChain
                {
                    StartDice = DiceAmount ?? 0,
                    ToHit = target
                };
                BuildState = BuildState.IsHitting;
                break;
            case BuildState.IsHitting:
                BuildingChain.ToWound = target;
                BuildState = BuildState.IsWounding;
                break;
            case BuildState.IsWounding:
                BuildingChain.ToArmorSave = target;
                BuildState = BuildState.IsArmorSave;
                break;
            case BuildState.IsArmorSave:
                BuildingChain.ToWardSave = target;
                ProbabilityChains.Insert(0, BuildingChain);
                BuildState = BuildState.IsNothing;
                BuildingChain = null;
                break;
        }
    }

    public void Delete(ProbabilityChain prob)
    {
        ProbabilityChains.Remove(prob);
    }

    public void SkipNextStep()
    {
        if (BuildState is BuildState.IsNothing or BuildState.IsHitting)
        {
            AddBuildStep(1);    
        }
        else
        {
            AddBuildStep(7);
        }
        
    }
}

public enum BuildState
{
    IsNothing, IsHitting, IsWounding, IsArmorSave
}