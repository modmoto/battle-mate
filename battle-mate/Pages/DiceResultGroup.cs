namespace battle_mate.Pages
{
    public class DiceResultGroup
    {
        public DiceResultGroup(int min, int amount, int biggerThanAmount, int countSmallerAmount)
        {
            Min = min;
            Amount = amount;
            BiggerThanAmount = biggerThanAmount;
            CountSmallerAmount = countSmallerAmount;
        }

        public int Min { get; }
        public int Amount { get; }
        public int BiggerThanAmount { get; }
        public int CountSmallerAmount { get; }
    }
}