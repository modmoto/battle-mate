using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace battle_mate.Pages.BattleResult
{
    public class CombatOptionsRepository
    {
        private readonly ILocalStorageService _localStorageService;
        private string _optionKey = "combat-options";

        public CombatOptionsRepository(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        
        public async Task<CombatOptions> LoadOptions()
        {
            var itemAsync = await _localStorageService.GetItemAsync<CombatOptions>(_optionKey);
            return itemAsync ?? new CombatOptions();
        }
        
        public Task SetOptions(CombatOptions options)
        {
            return _localStorageService.SetItemAsync(_optionKey, options).AsTask();
        }
    }
}