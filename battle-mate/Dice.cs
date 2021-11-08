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

        public RollResult ToHit(int count, int sides, int border)
        {
            return InternalRoll(count, sides, RollType.ToHit, border);
        }

        private RollResult InternalRoll(int count, int sides, RollType rollType, int border)
        {
            var rolls = new List<int>();
            for (int i = 0; i < count; i++)
            {
                rolls.Add(_random.Next(1, sides + 1));
            }

            return new RollResult(rolls, new List<int>(), sides, rollType, border);
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
            var newRoll = InternalRoll(count, sides, rollType, border).RawResults;
            resulstThatStay.AddRange(newRoll);
            resulstThatStay.Sort();
            return new RollResult(resulstThatStay, newRoll, sides, rollType, border);
        }

        public RollResult ToWound(int border, RollResult oldRolls)
        {
            return InternalRoll(oldRolls.SucessfullRolls, oldRolls.DiceSides, RollType.ToWound, border);
        }

        public RollResult ArmorSave(int border, RollResult oldRolls)
        {
            return InternalRoll(oldRolls.SucessfullRolls, oldRolls.DiceSides, RollType.ArmorSave, border);
        }

        public RollResult WardSave(int border, RollResult oldRolls)
        {
            return InternalRoll(oldRolls.FailedRolls, oldRolls.DiceSides, RollType.WardSave, border);
        }

        public RollResult JustRoll(int diceAmount, int selectedDice)
        {
            return InternalRoll(diceAmount, selectedDice, RollType.None, 0);
        }
    }
}