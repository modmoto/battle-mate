using System.Collections.Generic;
using System.Linq;

namespace battle_mate.Pages
{
    public class DiceResultGroup
    {
        public DiceResultGroup(int min, List<int> results)
        {
            Min = min;
            Amount = results.Count(r => r == min);
        }

        public int Min { get; }
        public int Amount { get; }
    }
}