namespace battle_mate.Pages
{
    public class DiceResultGroup
    {
        public DiceResultGroup(int min, int biggerThanAmount)
        {
            Min = min;
            BiggerThanAmount = biggerThanAmount;
        }

        public int Min { get; }
        public int BiggerThanAmount { get; }
    }
}