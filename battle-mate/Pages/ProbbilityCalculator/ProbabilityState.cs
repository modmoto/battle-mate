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
    
    public bool PoisonChecked { get; set; }
    public bool Poison5Checked { get; set; }
    public bool LethalStrikeChecked { get; set; }
    public bool BattleFocusChecked { get; set; }
    public bool RerollSuccessChecked { get; set; }
    public bool RerollFailsChecked { get; set; }
    public bool Reroll1SChecked { get; set; }

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
                SetOptions();
                break;
            case BuildState.IsHitting:
                BuildingChain.ToWound = target;
                BuildState = BuildState.IsWounding;
                SetOptions();
                break;
            case BuildState.IsWounding:
                BuildingChain.ToArmorSave = target;
                BuildState = BuildState.IsArmorSave;
                SetOptions();
                break;
            case BuildState.IsArmorSave:
                BuildingChain.ToWardSave = target;
                SetOptions();
                ProbabilityChains.Insert(0, BuildingChain);
                BuildState = BuildState.IsNothing;
                BuildingChain = null;
                RerollSuccessChecked = false;
                RerollFailsChecked = false;
                Reroll1SChecked = false;
                PoisonChecked = false;
                Poison5Checked = false;
                LethalStrikeChecked = false;
                BattleFocusChecked = false;
                OnProbabilitiesChanged?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    private void SetOptions()
    {
        BuildingChain.PoisonChecked = PoisonChecked;
        BuildingChain.Poison5Checked = Poison5Checked;
        BuildingChain.BattleFocusChecked = BattleFocusChecked;
        BuildingChain.LethalStrikeChecked = LethalStrikeChecked;
        BuildingChain.RerollSuccessChecked = RerollSuccessChecked;
        BuildingChain.RerollFailsChecked = RerollFailsChecked;
        BuildingChain.Reroll1SChecked = Reroll1SChecked;
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