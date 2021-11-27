using System;
using System.Collections.Generic;
using System.Linq;
using battle_mate.Pages;

namespace battle_mate
{
    public class RollResult
    {
        public RollResult(
            List<int> rawResults,
            List<int> rerollResults,
            List<int> oldResults,
            int sides,
            RollState state,
            RerollState rerollState,
            int diceGoal,
            bool forHit,
            bool battleFocus,
            bool forWound,
            bool poison,
            bool poison5Up,
            int poisonHits,
            bool forArmorSave,
            bool lethalStrike,
            int lethalStrikeHits)
        {
            RawResults = rawResults.OrderBy(r => r).ToList();
            OldResults = oldResults.OrderBy(r => r).ToList();
            RerollResults = rerollResults.OrderBy(r => r).ToList();
            FailedRerolls = rerollResults.Count(r => r < diceGoal);
            SucessfullRerolls = rerollResults.Count(r => r >= diceGoal);
            SucessfullRolls = rawResults.Count(r => r >= diceGoal);
            FailedRolls = rawResults.Count(r => r < diceGoal);
            Poison = poison;
            Poison5Up = poison5Up;
            if (forHit && poison5Up)
            {
                PoisonHits = rawResults.Count(r => r >= 5);
            }
            else if (forHit && poison)
            {
                PoisonHits = rawResults.Count(r => r == 6);
            }
            else
            {
                PoisonHits = poisonHits;
            }
            LethalStrike = lethalStrike;
            LethalStrikeHits = forWound && lethalStrike ? rawResults.Count(r => r == 6) : lethalStrikeHits;
            BattleFocus = battleFocus;
            BattleFocusHits = battleFocus ? rawResults.Count(r => r == 6) : 0;
            DiceSides = sides;
            DiceGoal = diceGoal;
            RollState = state;
            RerollState = rerollState;
            GroupedResults = GroupDice(rawResults, sides);

            if (forWound && (poison || poison5Up))
            {
                SucessfullRolls += poisonHits;
            }
            
            if (forArmorSave && lethalStrike)
            {
                FailedRolls += lethalStrikeHits;
            }
            
            if (forHit && battleFocus)
            {
                SucessfullRolls += BattleFocusHits;
            }
        }

        public List<int> RawResults { get; }
        public List<int> OldResults { get; }
        public List<int> RerollResults { get; }
        public int DiceSides { get; }
        public int DiceGoal { get; }
        public int SucessfullRolls { get; }
        public int FailedRerolls { get; }
        public int SucessfullRerolls { get; }
        public int FailedRolls { get; }
        public bool Poison { get; }
        public bool Poison5Up { get; }
        public int PoisonHits { get; }
        public bool LethalStrike { get; }
        public int LethalStrikeHits { get; }
        public bool BattleFocus { get; }
        public int BattleFocusHits { get; }
        public List<DiceResultGroup> GroupedResults { get; }
        public RollState RollState { get; }
        public RerollState RerollState { get; }
        public bool ReportedToLeft { get; private set; }
        public bool ReportedToRight { get; private set; }

        public RollResult UndoReroll()
        {
            if (RerollState != RerollState.NoReroll)
            {
                return new RollResult(OldResults, new List<int>(), new List<int>(), DiceSides, RollState,
                    RerollState.NoReroll, DiceGoal, false, BattleFocus, false, Poison, Poison5Up, PoisonHits, false,
                    LethalStrike, LethalStrikeHits);
            }

            return this;
        }
        
        private List<DiceResultGroup> GroupDice(List<int> results, int sides)
        {
            var resultGroups = new List<DiceResultGroup>();

            for (var i = 1; i <= sides; i++)
            {
                var resultGroup = new DiceResultGroup(i, results);
                resultGroups.Add(resultGroup);
            }

            return resultGroups;
        }

        public static RollResult AutoSucess(int ammount, List<int> oldResults, RollState state)
        {
            return new RollResult(Enumerable.Repeat(4, ammount).ToList(), new List<int>(), oldResults, 6,
                state, RerollState.NoReroll, 2, false, false, false, false, false, 0, false, false, 0);
        }
        
        public static RollResult AutoFail(int ammount, List<int> oldResults, RollState state)
        {
            return new RollResult(Enumerable.Repeat(4, ammount).ToList(), new List<int>(), oldResults, 6,
                state, RerollState.NoReroll, 5, false, false, false, false, false, 0, false, false, 0);
        }

        public void ReportRight()
        {
            ReportedToLeft = false;
            ReportedToRight = true;
        }

        public void ReportLeft()
        {
            ReportedToLeft = true;
            ReportedToRight = false;
        }

        public void ResetReport()
        {
            ReportedToLeft = false;
            ReportedToRight = false;
        }
    }
}