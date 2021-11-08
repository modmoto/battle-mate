using NUnit.Framework;

namespace battle_mate.Tests
{
    public class Tests
    {
        [Test]
        public void TestRandom()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4);
            var roll = rest.RawResults;
            Assert.AreEqual(12, roll.Count);
        }

        [Test]
        public void TestToWound()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4);

            var toWOund = dice.ToWound(3, rest);

            Assert.AreEqual(3, toWOund.DiceGoal);
            Assert.AreEqual(3, toWOund.SucessfullRolls);
            Assert.AreEqual(4, toWOund.FailedRolls);
            Assert.AreEqual(7, toWOund.RawResults.Count);
        }
        
        [Test]
        public void TestArmorSave()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4);

            var toWOund = dice.ToWound(3, rest);
            var armorSave = dice.ArmorSave(3, toWOund);

            Assert.AreEqual(3, armorSave.DiceGoal);
            Assert.AreEqual(1, armorSave.SucessfullRolls);
            Assert.AreEqual(2, armorSave.FailedRolls);
            Assert.AreEqual(3, armorSave.RawResults.Count);
        }
        
        [Test]
        public void TestWardSave()
        {
            var dice = new Dice(123);
            var rest = dice.ToHit(12, 6, 4);

            var toWOund = dice.ToWound(3, rest);
            var armorSave = dice.ArmorSave(3, toWOund);
            var wardSave = dice.ArmorSave(3, armorSave);

            Assert.AreEqual(3, wardSave.DiceGoal);
            Assert.AreEqual(1, wardSave.SucessfullRolls);
            Assert.AreEqual(0, wardSave.FailedRolls);
            Assert.AreEqual(1, wardSave.RawResults.Count);
        }
    }
}