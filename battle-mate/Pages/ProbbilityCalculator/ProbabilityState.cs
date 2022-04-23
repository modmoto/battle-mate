using System;
using System.Collections.Generic;
using battle_mate.Pages.BattleResult;

namespace battle_mate.Pages.ProbbilityCalculator;

public class ProbabilityState
{
    public List<ProbabilityChain> ProbabilityChains { get; set; } = new();
    public BuildState BuildState { get; set; }
    public ProbabilityChain BuildingChain { get; set; }
    public int? DiceAmount { get; set; }
    public event EventHandler OnProbabilitiesChanged;

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
                OnProbabilitiesChanged?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    public void Delete(ProbabilityChain prob)
    {
        ProbabilityChains.Remove(prob);
        OnProbabilitiesChanged?.Invoke(this, EventArgs.Empty);
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

    public void Init(List<ProbabilityChain> probabilities)
    {
        ProbabilityChains = probabilities;
    }
}

public enum BuildState
{
    IsNothing, IsHitting, IsWounding, IsArmorSave
}