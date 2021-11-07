using NUnit.Framework;

namespace battle_mate.Tests
{
    public class Tests
    {
        [Test]
        public void TestRandom()
        {
            var dice = new Dice(123);
            var roll = dice.Roll(5, 6);
            Assert.AreEqual(5, roll.Count);
            Assert.AreEqual(4, roll[0]);
            Assert.AreEqual(4, roll[1]);
            Assert.AreEqual(5, roll[2]);
            Assert.AreEqual(5, roll[3]);
            Assert.AreEqual(5, roll[4]);
        }
    }
}