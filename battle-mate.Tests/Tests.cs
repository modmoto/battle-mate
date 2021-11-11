using NUnit.Framework;

namespace battle_mate.Tests
{
    public class Tests
    {
        [Test]
        public void TestRandom()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4, false, false, false, false, false);
            var roll = rest.RawResults;
            Assert.AreEqual(12, roll.Count);
        }

        [Test]
        public void TestToHitPoison()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4, false, false, true, false, false);

            var toWOund = dice.ToWound(3, rest);

            Assert.AreEqual(3, rest.PoisonHits);
            Assert.AreEqual(7, rest.SucessfullRolls);
            Assert.AreEqual(RollState.ToWound, toWOund.RollState);
            Assert.AreEqual(5, toWOund.SucessfullRolls);
            Assert.AreEqual(2, toWOund.FailedRolls);
            Assert.AreEqual(4, toWOund.RawResults.Count);
        }

        [Test]
        public void TestToHitPoison5()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4, false, false, true, true, false);

            var toWOund = dice.ToWound(3, rest);

            Assert.AreEqual(6, rest.PoisonHits);
            Assert.AreEqual(7, rest.SucessfullRolls);
            Assert.AreEqual(RollState.ToWound, toWOund.RollState);
            Assert.AreEqual(6, toWOund.SucessfullRolls);
            Assert.AreEqual(1, toWOund.FailedRolls);
            Assert.AreEqual(1, toWOund.RawResults.Count);
        }
        
        [Test]
        public void TestToHitBattleFocus()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4, false, true, false, false, false);

            var toWOund = dice.ToWound(3, rest);

            Assert.AreEqual(3, rest.BattleFocusHits);
            Assert.AreEqual(10, rest.SucessfullRolls);
            Assert.AreEqual(RollState.ToWound, toWOund.RollState);
            Assert.AreEqual(4, toWOund.SucessfullRolls);
            Assert.AreEqual(6, toWOund.FailedRolls);
            Assert.AreEqual(10, toWOund.RawResults.Count);
        }

        [Test]
        public void TestToWound()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4, false, false, false, false, false);

            var toWOund = dice.ToWound(3, rest);

            Assert.AreEqual(3, toWOund.DiceGoal);
            Assert.AreEqual(RollState.ToWound, toWOund.RollState);
            Assert.AreEqual(3, toWOund.SucessfullRolls);
            Assert.AreEqual(4, toWOund.FailedRolls);
            Assert.AreEqual(7, toWOund.RawResults.Count);
        }

        [Test]
        public void TestAutoHit()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4, true, false, false, false, false);

            Assert.AreEqual(12, rest.SucessfullRolls);
            Assert.AreEqual(12, rest.RawResults.Count);
            Assert.AreEqual(RollState.AutomaticHit, rest.RollState);

            var toWOund = dice.ToWound(3, rest);

            Assert.AreEqual(3, toWOund.DiceGoal);
            Assert.AreEqual(RollState.ToWound, toWOund.RollState);
            Assert.AreEqual(5, toWOund.SucessfullRolls);
            Assert.AreEqual(7, toWOund.FailedRolls);
            Assert.AreEqual(12, toWOund.RawResults.Count);
        }

        [Test]
        public void TestArmorSave()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4, false, false, false, false, false);

            var toWOund = dice.ToWound(3, rest);
            var armorSave = dice.ArmorSave(3, toWOund);

            Assert.AreEqual(3, armorSave.DiceGoal);
            Assert.AreEqual(RollState.ArmorSave, armorSave.RollState);
            Assert.AreEqual(1, armorSave.SucessfullRolls);
            Assert.AreEqual(2, armorSave.FailedRolls);
            Assert.AreEqual(3, armorSave.RawResults.Count);
        }
        
        [Test]
        public void TestArmorSaveLethalStrike()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(24, 6, 4, false, false, false, false, true);

            var toWOund = dice.ToWound(3, rest);
            var armorSave = dice.ArmorSave(3, toWOund);

            Assert.AreEqual(9, toWOund.SucessfullRolls);
            Assert.AreEqual(2, toWOund.LethalStrikeHits);
            Assert.AreEqual(RollState.ArmorSave, armorSave.RollState);
            Assert.AreEqual(5, armorSave.SucessfullRolls);
            Assert.AreEqual(4, armorSave.FailedRolls);
            Assert.AreEqual(7, armorSave.RawResults.Count);
        }

        [Test]
        public void TestWardSave()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4, false, false, false, false, false);

            var toWOund = dice.ToWound(3, rest);
            var armorSave = dice.ArmorSave(3, toWOund);
            var wardSave = dice.WardSave(3, armorSave);

            Assert.AreEqual(3, wardSave.DiceGoal);
            Assert.AreEqual(RollState.WardSave, wardSave.RollState);
            Assert.AreEqual(1, wardSave.SucessfullRolls);
            Assert.AreEqual(1, wardSave.FailedRolls);
            Assert.AreEqual(2, wardSave.RawResults.Count);
        }
    }
}