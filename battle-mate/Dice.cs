using System;
using System.Collections.Generic;
using System.Linq;

namespace battle_mate
{
    public class Dice
    {
        private Random _random;
        
        public Dice(int seed = 0)
        {
            _random = seed == default ? new Random() : new Random(seed);
        }        
        
        public RollResult Roll(int count, int sides)
        {
            var rolls = new List<int>();
            for (int i = 0; i < count; i++)
            {
                rolls.Add(_random.Next(1, sides + 1));
            }

            return new RollResult(rolls, sides, RollType.Start, 0);
        }

        public RollResult RerRollSmallerThan(int border, RollResult oldRolls)
        {
            var count = oldRolls.RawResults.Count(d => d <= border);
            var resulstThatStay = oldRolls.RawResults.Where(d => d > border).ToList();
            return ReRoll(count, resulstThatStay, oldRolls.DiceSides, RollType.RerollSmallerThan, border);
        }

        public RollResult RerRollBiggerThan(int border, RollResult oldRolls)
        {
            var count = oldRolls.RawResults.Count(d => d >= border);
            var resulstThatStay = oldRolls.RawResults.Where(d => d < border).ToList();
            return ReRoll(count, resulstThatStay, oldRolls.DiceSides, RollType.RerollBiggerThan, border);
        }

        private RollResult ReRoll(int count, List<int> resulstThatStay, int sides, RollType rollType, int border)
        {
            var newRoll = Roll(count, sides).RawResults;
            resulstThatStay.AddRange(newRoll);
            resulstThatStay.Sort();
            return new RollResult(resulstThatStay, sides, rollType, border);
        }
    }
}