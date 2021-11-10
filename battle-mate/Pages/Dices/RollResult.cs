using System.Collections.Generic;
using System.Linq;
using battle_mate.Pages;

namespace battle_mate
{
    public class RollResult
    {
        public RollResult(List<int> rawResults, List<int> rerollResults, int sides, RollState state, int diceGoal)
        {
            RawResults = rawResults.OrderBy(r => r).ToList();
            RerollResults = rerollResults.OrderBy(r => r).ToList();
            SucessfullRolls = rawResults.Count(r => r >= diceGoal);
            FailedRolls = rawResults.Count(r => r < diceGoal);
            DiceSides = sides;
            DiceGoal = diceGoal;
            RollState = state;
            GroupedResults = GroupDice(rawResults, sides);
        }

        public List<int> RawResults { get; }
        public List<int> RerollResults { get; }
        public int DiceSides { get; }
        public int DiceGoal { get; }
        public int SucessfullRolls { get; }
        public int FailedRolls { get; }
        public List<DiceResultGroup> GroupedResults { get; }
        public RollState RollState { get; }

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
    }
}