using System.Collections.Generic;
using System.Linq;
using battle_mate.Pages;

namespace battle_mate
{
    public class RollResult
    {
        public RollResult(List<int> rawResults, List<int> rerollResults, int sides, RollType type, int diceBorder)
        {
            RawResults = rawResults.OrderBy(r => r).ToList();
            RerollResults = rerollResults.OrderBy(r => r).ToList();
            DiceSides = sides;
            DiceBorder = diceBorder;
            RollType = type;
            GroupedResults = GroupDice(rawResults, sides);
        }

        private List<DiceResultGroup> GroupDice(List<int> results, int sides)
        {
            var resultGroups = new List<DiceResultGroup>();

            for (int i = 1; i <= sides; i++)
            {
                var countBiggerEquals = results.Count(r => r >= i);
                var resultGroup = new DiceResultGroup(i, countBiggerEquals);
                resultGroups.Add(resultGroup);
            }

            return resultGroups;
        }

        public List<int> RawResults { get; }
        public List<int> RerollResults { get; }
        public int DiceSides { get; }
        public int DiceBorder { get; }
        public List<DiceResultGroup> GroupedResults { get; }
        public RollType RollType { get; } = RollType.Start;
    }
}