namespace battle_mate.Pages
{
    public class DiceResultGroup
    {
        public DiceResultGroup(int min, int amount)
        {
            Min = min;
            Amount = amount;
        }

        public int Min { get; }
        public int Amount { get; }
    }
}