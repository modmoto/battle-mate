using System.Collections.Generic;
using System.Linq;

namespace battle_mate.Pages
{
    public class DiceResultGroup
    {
        public DiceResultGroup(int min, List<int> results)
        {
            Min = min;
            BiggerThanAmount = results.Count(r => r >= min);
            SmallerThanAmount = results.Count(r => r <= min);
        }

        public int Min { get; }
        public int BiggerThanAmount { get; }
        public int SmallerThanAmount { get; }
    }
}