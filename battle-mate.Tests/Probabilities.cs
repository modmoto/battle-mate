using System.Linq;
using battle_mate.Pages.BattleResult;
using battle_mate.Pages.ProbbilityCalculator;
using NUnit.Framework;

namespace battle_mate.Tests;

public class Probabilities
{
    [Test]
    public void SimpleToHitRoll()
    {
        var probability = new Probability(2, 4);
        Assert.AreEqual(1, probability.SuccessResult);
    }
    
    [Test]
    public void SimpleToHitRoll_1()
    {
        var probability = new Probability(3, 4);
        Assert.AreEqual(1.5, probability.SuccessResult);
    }
    
    [Test]
    public void SimpleToHitRoll_2()
    {
        var probability = new Probability(3, 5);
        Assert.AreEqual(1, probability.SuccessResult);
    }
    
    [Test]
    public void SimpleToHitRoll_3()
    {
        var probability = new Probability(3, 6);
        Assert.AreEqual(0.5, probability.SuccessResult);
    }
    
    [Test]
    public void SimpleToArmorSaveRoll()
    {
        var probability = new Probability(6, 6);
        Assert.AreEqual(5, probability.FailedResult);
        Assert.AreEqual(1, probability.SuccessResult);
    }
    
    [Test]
    public void SimpleToArmorSaveRoll_2()
    {
        var probability = new Probability(3, 6);
        Assert.AreEqual(0.5, probability.SuccessResult);
        Assert.AreEqual(2.5, probability.FailedResult);
    }

    [Test]
    public void Append()
    {
        var probability = new Probability(6, 4).Append(4);
        Assert.AreEqual(1.5, probability.SuccessResult);
    }
    
    [Test]
    public void Append_2()
    {
        var probability = new Probability(3, 4).Append(4);
        Assert.AreEqual(0.75, probability.SuccessResult);
    }
    
    [Test]
    public void Append_3()
    {
        var probability = new Probability(6, 4).Append(5);
        Assert.AreEqual(1, probability.SuccessResult);
    }
    
    [Test]
    public void NegativeAppend_ArmorSave()
    {
        var probability = new Probability(6, 4).NegativeAppend(5);
        Assert.AreEqual(2, probability.FailedResult);
    }
    
    [Test]
    public void NegativeAppend_ArmorSave_2()
    {
        var probability = new Probability(6, 4).NegativeAppend(5).NegativeAppend(4);
        Assert.AreEqual(1, probability.FailedResult);
    }

    [Test]
    public void WholeChain()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 6,
            ToArmorSave = 4,
            ToWardSave = 4,
        };
        
        Assert.AreEqual(12, probabilityChain.ExpectedHits.SuccessResult);
        Assert.AreEqual(2, probabilityChain.ExpectedWounds.SuccessResult);
        Assert.AreEqual(1, probabilityChain.ExpectedArmorSaves.FailedResult);
        Assert.AreEqual(1, probabilityChain.ExpectedArmorSaves.SuccessResult);
        Assert.AreEqual(0.5, probabilityChain.ExpectedWardSaves.SuccessResult);
        Assert.AreEqual(0.5, probabilityChain.ExpectedWardSaves.FailedResult);
        Assert.AreEqual(0.5, probabilityChain.ExpectedWardSaves.FailedResult);
        Assert.AreEqual(0.5, probabilityChain.ExpectedFailedWardSaves);
    }

    [Test]
    public void BuildingWorksUntilWound()
    {
        var probabilityState = new ProbabilityState();
        probabilityState.DiceAmount = 24;
        probabilityState.AddBuildStep(4);
        probabilityState.AddBuildStep(4);
        
        Assert.AreEqual(BuildState.IsWounding, probabilityState.BuildState);
        Assert.AreEqual(6, probabilityState.BuildingChain.ExpectedWounds.SuccessResult);
    }
    
    [Test]
    public void AutoHit()
    {
        var probabilityState = new ProbabilityState();
        probabilityState.DiceAmount = 24;
        probabilityState.SkipNextStep();
        probabilityState.AddBuildStep(4);
        
        Assert.AreEqual(12, probabilityState.BuildingChain.ExpectedWounds.SuccessResult);
    }
    
    [Test]
    public void AutoWound()
    {
        var probabilityState = new ProbabilityState();
        probabilityState.DiceAmount = 24;
        probabilityState.AddBuildStep(4);
        probabilityState.SkipNextStep();
        
        Assert.AreEqual(12, probabilityState.BuildingChain.ExpectedWounds.SuccessResult);
    }
    
    [Test]
    public void NoArmorAndWard()
    {
        var probabilityState = new ProbabilityState();
        probabilityState.DiceAmount = 24;
        probabilityState.AddBuildStep(4);
        probabilityState.AddBuildStep(4);
        probabilityState.SkipNextStep();
        probabilityState.SkipNextStep();
        
        Assert.AreEqual(6, probabilityState.ProbabilityChains.Single().ExpectedFailedWardSaves);
    }
    
    [Test]
    public void NoArmorButWard()
    {
        var probabilityState = new ProbabilityState();
        probabilityState.DiceAmount = 24;
        probabilityState.AddBuildStep(4);
        probabilityState.AddBuildStep(4);
        probabilityState.SkipNextStep();
        probabilityState.AddBuildStep(4);
        
        Assert.AreEqual(3, probabilityState.ProbabilityChains.Single().ExpectedFailedWardSaves);
    }
    
    [Test]
    public void NoWardButArmor()
    {
        var probabilityState = new ProbabilityState();
        probabilityState.DiceAmount = 24;
        probabilityState.AddBuildStep(4);
        probabilityState.AddBuildStep(4);
        probabilityState.AddBuildStep(4);
        probabilityState.SkipNextStep();
        
        Assert.AreEqual(3, probabilityState.ProbabilityChains.Single().ExpectedFailedWardSaves);
    }

    [Test]
    public void Poison()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            PoisonChecked = true
        };
        
        Assert.AreEqual(8, probabilityChain.ExpectedWounds.SuccessResult);
        Assert.AreEqual(4, probabilityChain.ExpectedArmorSaves.SuccessResult);
        Assert.AreEqual(2, probabilityChain.ExpectedWardSaves.SuccessResult);
    }
    
    [Test]
    public void Poison_Hitting6es()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 6,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            PoisonChecked = true
        };
        
        Assert.AreEqual(4, probabilityChain.ExpectedHits.SuccessResult);
        Assert.AreEqual(4, probabilityChain.ExpectedWounds.SuccessResult);
        Assert.AreEqual(2, probabilityChain.ExpectedArmorSaves.SuccessResult);
        Assert.AreEqual(1, probabilityChain.ExpectedWardSaves.SuccessResult);
    }
    
    [Test]
    public void LethalStrikes()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            LethalStrikeChecked = true
        };
        
        Assert.AreEqual(12, probabilityChain.ExpectedHits.SuccessResult);
        Assert.AreEqual(6, probabilityChain.ExpectedWounds.SuccessResult);
        Assert.AreEqual(3.5, probabilityChain.ExpectedArmorSaves.FailedResult);
        Assert.AreEqual(2.5, probabilityChain.ExpectedArmorSaves.SuccessResult);
        Assert.AreEqual(1.75, probabilityChain.ExpectedWardSaves.SuccessResult);
        Assert.AreEqual(1.75, probabilityChain.ExpectedWardSaves.FailedResult);
    }
    
    [Test]
    public void LethalStrikes_6sToSave()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 6,
            ToWardSave = 4,
            LethalStrikeChecked = true
        };
        
        Assert.AreEqual(6, probabilityChain.ExpectedWounds.SuccessResult);
        Assert.AreEqual(5.166666666666667d, probabilityChain.ExpectedArmorSaves.FailedResult);
        Assert.AreEqual(0.83333333333333326d, probabilityChain.ExpectedArmorSaves.SuccessResult);
    }
    
    [Test]
    public void NoLethalStrikes_6sToSave()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 6,
            ToWardSave = 4
        };
        
        Assert.AreEqual(6, probabilityChain.ExpectedWounds.SuccessResult);
        Assert.AreEqual(5, probabilityChain.ExpectedArmorSaves.FailedResult);
        Assert.AreEqual(1, probabilityChain.ExpectedArmorSaves.SuccessResult);
    }
    
    [Test]
    public void Poison5()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            Poison5Checked = true
        };
        
        Assert.AreEqual(10, probabilityChain.ExpectedWounds.SuccessResult);
        Assert.AreEqual(5, probabilityChain.ExpectedArmorSaves.SuccessResult);
        Assert.AreEqual(2.5, probabilityChain.ExpectedWardSaves.SuccessResult);
    }

    [Test]
    public void Poison5And6()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            Poison5Checked = true,
            PoisonChecked = true,
        };
        
        Assert.AreEqual(10, probabilityChain.ExpectedWounds.SuccessResult);
        Assert.AreEqual(5, probabilityChain.ExpectedArmorSaves.SuccessResult);
        Assert.AreEqual(2.5, probabilityChain.ExpectedWardSaves.SuccessResult);
    }
    
    [Test]
    public void BattleFocus()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            BattleFocusChecked = true
        };
        
        Assert.AreEqual(16, probabilityChain.ExpectedHits.SuccessResult);
        Assert.AreEqual(8, probabilityChain.ExpectedWounds.SuccessResult);
    }
    
    [Test]
    public void BattleFocus_14Bug()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 14,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            BattleFocusChecked = true
        };
        
        Assert.AreEqual(9.3333333333333339d, probabilityChain.ExpectedHits.SuccessResult);
        Assert.AreEqual(4.666666666666667d, probabilityChain.ExpectedWounds.SuccessResult);
    }
    
    [Test]
    public void BattleFocus_6es()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 6,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            BattleFocusChecked = true
        };
        
        Assert.AreEqual(8, probabilityChain.ExpectedHits.SuccessResult);
        Assert.AreEqual(4, probabilityChain.ExpectedWounds.SuccessResult);
    }
    
    [Test]
    public void BattleFocusAndPoison()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            BattleFocusChecked = true,
            PoisonChecked = true
        };
        
        Assert.AreEqual(16, probabilityChain.ExpectedHits.SuccessResult);
        Assert.AreEqual(10, probabilityChain.ExpectedWounds.SuccessResult);
    }
    
    [Test]
    public void BattleFocusAndPoison5()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            BattleFocusChecked = true,
            Poison5Checked = true
        };
        
        Assert.AreEqual(16, probabilityChain.ExpectedHits.SuccessResult);
        Assert.AreEqual(12, probabilityChain.ExpectedWounds.SuccessResult);
    }

    [Test]
    public void RerollFailsCheckedToHit()
    {
        var probabilityChain = new ProbabilityChain()
        {
            StartDice = 24,
            ToHit = 4,
            ToWound = 4,
            ToArmorSave = 4,
            ToWardSave = 4,
            
            RerollsHit = new RerollDto
            {
                RerollFailsChecked = true
            }
        };
        
        Assert.AreEqual(18, probabilityChain.ExpectedHits.SuccessResult);
        Assert.AreEqual(9, probabilityChain.ExpectedWounds.SuccessResult);
    }

    [Test]
    public void PropWithRerollFailedSet()
    {
        var probability = new Probability(12, 4, new RerollDto { RerollFailsChecked = true });
        
        Assert.AreEqual(9, probability.SuccessResult);
    }
    
    [Test]
    public void PropWithRerollSuccessSet()
    {
        var probability = new Probability(12, 4, new RerollDto { RerollSuccessChecked = true });
        
        Assert.AreEqual(3, probability.SuccessResult);
    }
    
    [Test]
    public void PropWithReroll1sSet()
    {
        var probability = new Probability(12, 4, new RerollDto { Reroll1SChecked = true });
        
        Assert.AreEqual(6 + 2 * 1 / 6, probability.SuccessResult);
    }
}