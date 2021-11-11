namespace battle_mate.Pages.BattleResult
{
    public class PointStrings
    {
        public int Points { get; }
        public string Text { get; }

        public PointStrings(int points, string text)
        {
            Points = points;
            Text = text;
        }
    }
}