using NUnit.Framework;

namespace battle_mate.Tests
{
    public class Tests
    {
        [Test]
        public void TestRandom()
        {
            var dice = new Dice(123);
            var rest = dice.Roll(12, 6);
            var roll = rest.RawResults;
            Assert.AreEqual(12, roll.Count);
        }

        [Test]
        public void TestContinueRollBigger()
        {
            var dice = new Dice(123);
            var rest = dice.Roll(12, 6);
            var rollBiggerThan = dice.ContinueRollBiggerThan(3, rest);
            Assert.AreEqual(8, rollBiggerThan.RawResults.Count);
        }

        [Test]
        public void TestContinueRollSmaller()
        {
            var dice = new Dice(123);
            var rest = dice.Roll(12, 6);
            var rollBiggerThan = dice.ContinueRollSmallerThan(3, rest);
            Assert.AreEqual(5, rollBiggerThan.RawResults.Count);
        }
    }
}