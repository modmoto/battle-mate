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
}