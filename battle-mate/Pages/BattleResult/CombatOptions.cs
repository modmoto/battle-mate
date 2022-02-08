namespace battle_mate.Pages.BattleResult
{
    public class CombatOptions
    {
        public bool HasAdditionalPoints { get; set; }
        public bool HasVerminSwarmPoints { get; set; }
        public bool HasResetRanksAndBanners { get; set; } = true;
        public int DefaultBanners { get; set; } = 1;
        public int DefaultRanks { get; set; } = 3;
    }
}