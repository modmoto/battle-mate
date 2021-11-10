using System;
using System.Collections.Generic;
using System.Linq;

namespace battle_mate
{
    public class Dice
    {
        private readonly Random _random;

        public Dice(int seed = 0)
        {
            _random = seed == default ? new Random() : new Random(seed);
        }

        public RollResult ToHit(int count, int sides, int border, bool automaticHits)
        {
            if (automaticHits)
            {
                return InternalRoll(count, sides, RollState.AutomaticHit, 0);
            }
            return InternalRoll(count, sides, RollState.ToHit, border);
        }

        private RollResult InternalRoll(int count, int sides, RollState rollState, int border)
        {
            var rolls = new List<int>();
            for (var i = 0; i < count; i++) rolls.Add(_random.Next(1, sides + 1));

            return new RollResult(rolls, new List<int>(), sides, rollState, border);
        }

        public RollResult RerRollSmallerThan(int border, RollResult oldRolls)
        {
            var count = oldRolls.RawResults.Count(d => d <= border);
            var resulstThatStay = oldRolls.RawResults.Where(d => d > border).ToList();
            return ReRoll(count, resulstThatStay, oldRolls.DiceSides, RollState.RerollSmallerThan, border);
        }

        public RollResult RerRollBiggerThan(int border, RollResult oldRolls)
        {
            var count = oldRolls.RawResults.Count(d => d >= border);
            var resulstThatStay = oldRolls.RawResults.Where(d => d < border).ToList();
            return ReRoll(count, resulstThatStay, oldRolls.DiceSides, RollState.RerollBiggerThan, border);
        }

        private RollResult ReRoll(int count, List<int> resulstThatStay, int sides, RollState rollState, int border)
        {
            var newRoll = InternalRoll(count, sides, rollState, border).RawResults;
            resulstThatStay.AddRange(newRoll);
            resulstThatStay.Sort();
            return new RollResult(resulstThatStay, newRoll, sides, rollState, border);
        }

        public RollResult ToWound(int border, RollResult oldRolls)
        {
            return InternalRoll(oldRolls.SucessfullRolls, oldRolls.DiceSides, RollState.ToWound, border);
        }

        public RollResult ArmorSave(int border, RollResult oldRolls)
        {
            return InternalRoll(oldRolls.SucessfullRolls, oldRolls.DiceSides, RollState.ArmorSave, border);
        }

        public RollResult WardSave(int border, RollResult oldRolls)
        {
            return InternalRoll(oldRolls.FailedRolls, oldRolls.DiceSides, RollState.WardSave, border);
        }

        public RollResult JustRoll(int diceAmount, int selectedDice)
        {
            return InternalRoll(diceAmount, selectedDice, RollState.None, 0);
        }
    }
}