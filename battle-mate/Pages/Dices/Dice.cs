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

        public RollResult ToHit(int count,
            int sides,
            int border,
            bool automaticHits,
            bool battleFocus,
            bool poison,
            bool poison5Checked,
            bool lethalStrike)
        {
            if (automaticHits)
            {
                return InternalRoll(count, sides, RollState.AutomaticHit, 0);
            }
            
            return InternalRoll(count, sides, RollState.ToHit, border, true, battleFocus, false, poison, poison5Checked, 0, false, lethalStrike, 0);
        }

        private RollResult InternalRoll(int count, int sides, RollState rollState, int border, bool forHit, bool battleFocus, bool forWound, bool poison, bool poison5Checked, int poisonHits, bool forArmorSave, bool lethalStrike, int lethalStrikeHits)
        {
            var rolls = GetRandomRolls(count, sides);

            return new RollResult(rolls, new List<int>(), sides, rollState, RerollState.NoReroll, border, forHit, battleFocus, forWound, poison, poison5Checked, poisonHits, forArmorSave, lethalStrike, lethalStrikeHits);
        }

        private RollResult InternalRoll(int count, int sides, RollState rollState, int border)
        {
            var rolls = GetRandomRolls(count, sides);

            return new RollResult(rolls, new List<int>(), sides, rollState, RerollState.NoReroll, border, false, false, false, false, false, 0, false, false, 0);
        }

        private List<int> GetRandomRolls(int count, int sides)
        {
            var rolls = new List<int>();
            for (var i = 0; i < count; i++)
            {
                rolls.Add(_random.Next(1, sides + 1));
            }

            return rolls;
        }

        public RollResult RerRollSmallerThan(int border, RollResult oldRolls)
        {
            var count = oldRolls.RawResults.Count(d => d <= border);
            var resulstThatStay = oldRolls.RawResults.Where(d => d > border).ToList();
            return ReRoll(count, resulstThatStay, oldRolls.DiceSides, oldRolls.RollState, RerollState.RerollSmallerThan, oldRolls.DiceGoal, oldRolls.BattleFocus, oldRolls.Poison, oldRolls.Poison5Up, oldRolls.PoisonHits, oldRolls.LethalStrike, oldRolls.LethalStrikeHits);
        }

        public RollResult RerRollBiggerThan(int border, RollResult oldRolls)
        {
            var count = oldRolls.RawResults.Count(d => d >= border);
            var resulstThatStay = oldRolls.RawResults.Where(d => d < border).ToList();
            return ReRoll(count, resulstThatStay, oldRolls.DiceSides, oldRolls.RollState, RerollState.RerollBiggerThan, oldRolls.DiceGoal, oldRolls.BattleFocus, oldRolls.Poison, oldRolls.Poison5Up, oldRolls.PoisonHits, oldRolls.LethalStrike, oldRolls.LethalStrikeHits);
        }

        private RollResult ReRoll(int count, List<int> resulstThatStay, int sides, RollState rollState, RerollState rerollState, int border, bool battleFocus, bool poison, bool poison5Checked, int poisonHits, bool lethalStrike, int lethalStrikeHits)
        {
            var newRoll = InternalRoll(count, sides, rollState, border).RawResults;
            resulstThatStay.AddRange(newRoll);
            resulstThatStay.Sort();
            return new RollResult(resulstThatStay, newRoll, sides, rollState, rerollState, border, rollState == RollState.ToHit, battleFocus, rollState == RollState.ToWound, poison, poison5Checked, poisonHits, rollState == RollState.ArmorSave, lethalStrike, lethalStrikeHits);
        }

        public RollResult ToWound(int border, RollResult oldRolls)
        {
            return InternalRoll(oldRolls.SucessfullRolls - oldRolls.PoisonHits, oldRolls.DiceSides, RollState.ToWound, border, false, oldRolls.BattleFocus, true, oldRolls.Poison, oldRolls.Poison5Up, oldRolls.PoisonHits, false, oldRolls.LethalStrike, oldRolls.LethalStrikeHits);
        }

        public RollResult ArmorSave(int border, RollResult oldRolls)
        {
            return InternalRoll(oldRolls.SucessfullRolls - oldRolls.LethalStrikeHits, oldRolls.DiceSides, RollState.ArmorSave, border, false, oldRolls.BattleFocus, false, oldRolls.Poison, oldRolls.Poison5Up, oldRolls.PoisonHits, true, oldRolls.LethalStrike, oldRolls.LethalStrikeHits);
        }

        public RollResult WardSave(int border, RollResult oldRolls)
        {
            return InternalRoll(oldRolls.FailedRolls, oldRolls.DiceSides, RollState.WardSave, border);
        }

        public RollResult JustRoll(int diceAmount, int selectedDice)
        {
            return InternalRoll(diceAmount, selectedDice, RollState.None, 0);
        }

        public RollResult RerRollOnes(RollResult oldRolls)
        {
            var count = oldRolls.RawResults.Count(d => d == 1);
            var resulstThatStay = oldRolls.RawResults.Where(d => d > 1).ToList();
            return ReRoll(count, resulstThatStay, oldRolls.DiceSides, oldRolls.RollState, RerollState.Reroll1s, oldRolls.DiceGoal, oldRolls.BattleFocus, oldRolls.Poison, oldRolls.Poison5Up, oldRolls.PoisonHits, oldRolls.LethalStrike, oldRolls.LethalStrikeHits);
        }
    }
}