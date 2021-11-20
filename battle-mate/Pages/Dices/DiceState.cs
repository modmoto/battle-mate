using System.Collections.Generic;
using battle_mate.Pages;

namespace battle_mate
{
    public class DiceState
    {
        public List<DiceMessage> DiceMessages { get; } = new();
        public int? DiceAmount { get; set; }
        public int SelectedDice { get; set; }
    }
}